using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;

namespace P2P_client
{
    [Serializable]

    /// <summary>
    /// Представляет состяния, в котором может находится раздача
    /// </summary>
    public enum MaterialStatus
    {
        /// <summary>
        /// Раздача запущена
        /// </summary>
        Started,
        /// <summary>
        /// Раздача приостановлена
        /// </summary>
        Paused,
        /// <summary>
        /// Раздача остановлена
        /// </summary>
        Stoped,
        /// <summary>
        /// Раздача завершена
        /// </summary>
        Finished,

    }



    [Serializable]

    partial class Material
    {
        public string Name;
        public File[] Files;
        public int IdMaterial;
        public int PartSize;
        public Client server;
        public int IdClient;
        string started = "no";
        public long size;
        private int[] filese_that_need_to_download;
        bool finished = false;

        public MaterialStatus MaterialState;
        public long Stat_Download, Stat_Upload;
        long Stat_Download_, Stat_Upload_;
        public double speed_Download, speed_Upload;

        [NonSerialized()] Thread ServerThread, UpdatingClients;
       

      



        /// <summary>
        /// Конструктор, вызываемый при создании новой раздачи
        /// </summary>
        /// <param name="name">Имя раздачи</param>
        /// <param name="dir">Папка, в которой находится раздача</param>
        /// <param name="partsize">Размер части</param>
        /// <param name="server">Содержит IP и порт сервера, на котором зарегистрирован материал</param>
        /// 
        public Material(string name, string dir, int partsize)
        {
            Name = name;
            PartSize = partsize;
            DirectoryInfo d = new DirectoryInfo(dir);
            List<string> files = GetFiles(d);// получаем список файлов
            Files = new File[files.Count];
            string str_full = d.FullName.Substring(0, d.FullName.Length - d.Name.Length);
            for (int i = 0; i < files.Count; i++)
                Files[i] = new File(str_full + files[i], files[i], partsize);


            SetSize();
            filese_that_need_to_download = new int[Files.Length];
            cread_map_download();
            MaterialState = MaterialStatus.Finished;
        }

        private void SetSize()
        {
            size = 0;
            for (int i = 0; i < Files.Length; i++)
            {
                size += Files[i].File_Size;
            }
        }


        /// <summary>
        /// Конструктор, вызывается при добавлении раздачи из существующего метафайла
        /// </summary>
        /// <param name="PathToMetaFile">Путь к метафайлу</param>
        public Material(string dir, string PathToMetaFile)
        {
            ProgramMaterials.cur_file = 0;
            BinaryFormatter bf = new BinaryFormatter();
            if (!System.IO.File.Exists(PathToMetaFile))
                throw new Exceptions.IsNotMetafile();

            FileStream metafile = new FileStream(PathToMetaFile, FileMode.Open);
            if (dir[dir.Length - 1] != '\\') dir += "\\";
            try
            {
                Meta_File mf = (Meta_File)bf.Deserialize(metafile);
                IdClient = mf.ID_Client;
                IdMaterial = mf.ID_matirial;
                Name = mf.name;
                server = mf.IP_Port_Server;
                Files = new File[mf.files.Length];

                for (int i = 0; i < mf.files.Length; i++)
                {
                    ProgramMaterials.cur_file = i;
                    Files[i] = new File(mf.files[i].path, dir + mf.files[i].path, mf.files[i].File_Size, mf.files[i].Count_Parts_, mf.files[i].Hashs, mf.files[i].Part_size);
                }
                filese_that_need_to_download = new int[Files.Length];

                cread_map_download();
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                throw new Exceptions.WrongMetafileFormat();
            }
            finally
            {
                metafile.Close();
            }
            SetSize();
            SetStatus();
        }

        public void SetStatus()
        {
            MaterialState = MaterialStatus.Finished;
            for (int i = 0; i < Files.Length; i++)
            {
                for (int j = 0; j < Files[i].Count_Parts_; j++)
                {
                    if (!Files[i].IsPart(j))
                    {
                        MaterialState = MaterialStatus.Started;
                        break;
                    }
                }
            }
        }


        private void cread_map_download()
        {
            for (int i = 0; i < Files.Length; i++)
            {
                if (!Files[i].IsFull())
                {
                    filese_that_need_to_download[i] = 1;// Нужно скачать
                }
                else
                    filese_that_need_to_download[i] = 0;// Уже скачан
                // 3- скачивается
            }
        }
        public void SaveMetaFile(string filename)
        {
            Meta_File mf = new Meta_File();
            mf.files = Files;
            mf.Size_part = PartSize;
            mf.Size_treck = size;
            mf.name = Name;
            mf.IP_Port_Server = server;
            mf.ID_Client = IdClient;
            mf.ID_matirial = IdMaterial;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filename, FileMode.Create);
            bf.Serialize(fs, mf);
            fs.Close();

        }


        /// <summary>
        /// Производит рекурсивный проход укзанной директории, возвращает список файлов во всех ее вложенных каталогах.
        /// </summary>
        /// <param name="dir">Экземпляр DirectoryInfo, указывающий на директорию</param>
        /// <returns>Список файлов с полными путями, относительно корневой директории раздачи</returns>
        private List<string> GetFiles(DirectoryInfo dir)
        {
            DirectoryInfo[] d = dir.GetDirectories();// массив подкаталогов данного каталога

            List<string> files = new List<string>();// создаем список для имен файлов

            FileInfo[] f = dir.GetFiles(); // получаем массив вложеных файлов


            for (int i = 0; i < f.Length; i++) // заносим имя каждого файла в список
                files.Add(f[i].Name);

            foreach (DirectoryInfo feDir in d)  // рекурсивно вызываем данную фунцию для каждого вложенного каталога
            {
                List<string> tl = GetFiles(feDir);
                files.AddRange(tl); // добавляем все найденные файлы в список
            }

            for (int i = 0; i < files.Count; i++) // перебираем имена всех вложенных файлов
            {
                files[i] = dir.Name + "\\" + files[i]; // добавляем в начало имя данной директории
            }
            return files;
        }

        /// <summary>
        /// Запускает раздачу
        /// </summary>
        public void Start()
        {

            Stat_Upload_ = 0; // обнуляем статистику 
            Stat_Download_ = 0;
            ServerThread = new Thread(ServerThreadFunction); // создаем 
            UpdatingClients = new Thread(GetClients);
            SetStatus();

            ServerThread.IsBackground = true;
            UpdatingClients.IsBackground = true;
            ServerThread.Start(); // и запускаем поток синхронизации с сервером
            UpdatingClients.Start();

        }
        /// <summary>
        /// Приостанавливает раздачу
        /// </summary>
        public void Pause()
        {
            // Отлаживаем потоки
            ServerThread.Suspend();
            //ClientThreadDownload.Suspend();
            //ClientThreadUpload.Suspend();
            MaterialState = MaterialStatus.Paused; // обновляем статус

        }
        public void Download(object connection)
        {
            Connection con = (Connection)connection;

            Socket socket_download = con.Socket;
            byte[] id_file_to_dowmnload = new byte[4];
            byte[] part_to_download = new byte[4];
            lock (connections)
                connections.Add(con);
            Config.Connections++;
            byte[] comand_send = new byte[1];
            byte[] comand_recive = new byte[1];



        reconnect:
            comand_send[0] = (byte)C_C_Protocol.Connect;
            socket_download.Send(comand_send, 1, SocketFlags.None);
        session:
            socket_download.Receive(comand_recive, 1, SocketFlags.None);
            switch (comand_recive[0])
            {
                case (byte)C_C_Protocol.Connected:
                    goto Connected;
                case (byte)C_C_Protocol.MaterialResponse:
                    goto MaterialResponse;
                case (byte)C_C_Protocol.SendPart:
                    goto RecivePart;
                default:
                    if (filese_that_need_to_download[BitConverter.ToInt32(id_file_to_dowmnload, 0)] != 2)
                    {
                        filese_that_need_to_download[BitConverter.ToInt32(id_file_to_dowmnload, 0)] = 1;
                    }

                    goto exit;
            }



        Connected:
            byte[] result = new byte[1];
            socket_download.Receive(result, 1, SocketFlags.None);
            switch (result[0])
            {
                case 0:
                    goto exit;
                case 1:
                    comand_send[0] = (byte)C_C_Protocol.IsMaterial;
                    socket_download.Send(comand_send, 1, SocketFlags.None);
                    byte[] id_mat = new byte[4];
                    id_mat = BitConverter.GetBytes(IdMaterial);
                    socket_download.Send(id_mat, 4, SocketFlags.None);
                    goto session;
                default:
                    goto reconnect;

            }



        MaterialResponse:
            byte[] result2 = new byte[5];
            socket_download.Receive(result2, 1, SocketFlags.None);
            switch (result2[0])
            {
                case 0:
                    goto exit;
                case 1:
                    socket_download.Receive(result2, 4, SocketFlags.None);
                    if (BitConverter.ToInt32(result2, 0) == IdMaterial)
                    {
                        comand_send[0] = (byte)C_C_Protocol.GetPart;
                        socket_download.Send(comand_send, 1, SocketFlags.None);
                        goto GetPart;
                    }
                    else
                    {
                        goto exit;
                    }

                default:
                    goto reconnect;
            }


        GetPart:
            bool isfile = false;
            for (int i = 0; i < filese_that_need_to_download.Length; i++)
            {
                lock (filese_that_need_to_download)
                {
                    if (filese_that_need_to_download[i] == 1)
                    {
                        id_file_to_dowmnload = BitConverter.GetBytes(i);

                        filese_that_need_to_download[i] = 3;
                        isfile = true;
                        break;
                    }
                }

            }
            byte[] map_parts;
            if (isfile)
            {

                map_parts = Files[BitConverter.ToInt32(id_file_to_dowmnload, 0)].Get_Download_Bit_Map();
                for (int i = 0; i < map_parts.Length; i++)
                {
                    if (map_parts[i] == 0)
                    {
                        part_to_download = BitConverter.GetBytes(i);
                        break;
                    }
                }
                socket_download.Send(BitConverter.GetBytes(IdMaterial), 4, SocketFlags.None);
                socket_download.Send(id_file_to_dowmnload, 4, SocketFlags.None);
                socket_download.Send(part_to_download, 4, SocketFlags.None);
                goto session;
            }
            else
            {
                MaterialState = MaterialStatus.Finished;
                goto exit;
            }

        RecivePart:
            byte[] result3 = new byte[13];
            byte[] resp = new byte[1];
            socket_download.Receive(resp, 1, SocketFlags.None);
            switch (resp[0])
            {
                case 0:
                    goto exit;
                case 1:
                    byte[] idmat_ = new byte[4];
                    byte[] idfile_ = new byte[4];
                    byte[] idpart = new byte[4];
                    socket_download.Receive(idmat_, 4, SocketFlags.None);
                    socket_download.Receive(idfile_, 4, SocketFlags.None);
                    socket_download.Receive(idpart, 4, SocketFlags.None);
                    if (BitConverter.ToInt32(idmat_, 0) == IdMaterial && BitConverter.ToInt32(idfile_, 0) == BitConverter.ToInt32(id_file_to_dowmnload, 0) && BitConverter.ToInt32(idpart, 0) == BitConverter.ToInt32(part_to_download, 0))
                    {
                        int size = Files[BitConverter.ToInt32(result3, 9)].Part_size;

                        byte[] part = new byte[size];
                        long start_recive = DateTime.Now.Ticks;
                        NetworkStream ns = new NetworkStream(socket_download);
                        for (int i = 0; i != size; )
                        {
                            i += ns.Read(part, i, size - i);

                        }
                        long end_recive = DateTime.Now.Ticks;
                        speed_Download = size / (((int)end_recive - (int)start_recive) / 1000000);
                        Stat_Download_ += size;

                        //socket_download.Receive(part);
                        Files[BitConverter.ToInt32(idfile_, 0)].Set_Part(BitConverter.ToInt32(idpart, 0), part);
                        bool state_file = true;
                        map_parts = Files[BitConverter.ToInt32(id_file_to_dowmnload, 0)].Get_Download_Bit_Map();
                        for (int i = 0; i < map_parts.Length; i++)
                        {
                            if (map_parts[i] == 0)
                            {
                                state_file = false;
                                break;
                            }
                        }
                        if (state_file)
                        {
                            lock (filese_that_need_to_download)
                            {
                                filese_that_need_to_download[BitConverter.ToInt32(id_file_to_dowmnload, 0)] = 2;//Скачан
                            }
                            goto exit;

                        }
                        else
                        {
                            lock (filese_that_need_to_download)
                            {
                                filese_that_need_to_download[BitConverter.ToInt32(id_file_to_dowmnload, 0)] = 1;//Недокачан
                            }
                            comand_send[0] = (byte)C_C_Protocol.GetPart;
                            socket_download.Send(comand_send, 1, SocketFlags.None);
                            goto GetPart;
                        }

                    }
                    else
                    {
                        lock (filese_that_need_to_download)
                        {
                            filese_that_need_to_download[BitConverter.ToInt32(id_file_to_dowmnload, 0)] = 1;//Недокачан
                        }
                        goto reconnect;
                    }

                default:
                    goto reconnect;
            }

        exit:
            lock (connections)
            {
                con.Socket.Close();
                connections.Remove(con);
                
                
            }
        }

        /// <summary>
        /// Возобновляем раздачу
        /// </summary>
        public void Resume()
        {
            // Возвращаем потоки в очередь на выполнение
            ServerThread.Resume();

            foreach (Connection c in connections)
            {
                c.Thread.Resume();
            }
            SetStatus();
        }

        /// <summary>
        /// Останавливаем раздачу
        /// </summary>
        public void Stop()
        {
            // Снимаем потоки с выпонения
            ServerThread.Abort();
            foreach (Connection c in connections)
            {
                c.Thread.Interrupt();
            }
            MaterialState = MaterialStatus.Stoped;


        }

        /// <summary>
        /// Реализует точку входа потока синхронизации с сервером
        /// </summary>
        private void ServerThreadFunction()
        {
            
        Session: // Сессия коммуникации с сервером
            lock (started) // получаем монопольный доступ к флагу запуска
            {
                C_S_Exchange ce = null;
                try
                {
                    ce = new C_S_Exchange(server.ip, server.port); // создаем подключение к серверу


                    ce.Register(IdClient, Config.Port); // регистрируем сеанс на сервере
                    ce.GetMessage(); // ждем от сервера подтверждение
                    ce.GetClients(IdMaterial); // посылаем запрос выдать клиентов по материалу
                    ce.GetMessage(); // ждем подтверждение
                    ce.UpdateInfo(IdMaterial, Stat_Upload_, Stat_Download_); // отправляем статистику
                    ce.GetMessage();
                    Stat_Download += Stat_Download_; Stat_Upload += Stat_Upload_;
                    Stat_Upload_ = 0; Stat_Download_ = 0; // обнуляем статистику
                    ce.EndTrasaction(); // завершам сессию общения с сервером
                    ce.GetMessage(); // ожидаем подтверждение
                    
                }
                catch (Exception)
                {
                    goto Session;
                }
                started = "yes";// флаг запуска выставляем
                Thread.Sleep(Config.UpdateTimeout); // отлаживаем поток на заданное значение таймаута обновления

            }
            goto Session; // когда прошел интервал, запускаем новую транзакцию
        }
        private void GetClients()
        {
getinfo:
            if (MaterialState == MaterialStatus.Finished)
            {
                if (finished) return;
                foreach (File f in Files)
                {
                    f.CheckHashes();
                }
                SetStatus();
                if (MaterialState == MaterialStatus.Finished) { finished = true; return; }

            }
            string filename = "Temp/clients" + IdMaterial + ".tmp";
            if (!System.IO.File.Exists(filename))
                return;
            ReadClients(filename);

            filename = "Temp/cte" + IdMaterial + ".tmp";
            if (!System.IO.File.Exists(filename))
                return;
            ReadClients(filename);

            Thread.Sleep(30 * 1000);
            goto getinfo;
        }

        private void ReadClients(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
        Read:
            while (fs.Length > fs.Position)
            {
                byte[] temp = new byte[6];
                fs.Read(temp, 0, 6);
                Client t = new Client();
                t.Deserialize(temp);


                IPAddress IP_Client = t.ip;
                short Port_Client = t.port;
                IPEndPoint ip;
                for (int i = 0; i < connections.Count; i++)
                {
                    ip = (IPEndPoint)(connections[i].Socket.RemoteEndPoint);
                    if (ip.Address.ToString() == IP_Client.ToString())
                        goto Read;
                }

                Socket socket_download = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                try
                {
                    Connection con = new Connection();
                    con.Thread = new Thread(Download);
                    con.Thread.IsBackground = true;
                    con.Socket = socket_download;
                    con.Thread.Start(con);
                    socket_download.Connect(new IPEndPoint(IP_Client, Port_Client));
                }
                catch (SocketException)
                {
                    /*if (Config.IDT_Enaibled)
                    {
                        Client cl = new Client(IP_Client, Port_Client);
                        System.Threading.Thread th = new Thread(IDT_client);
                        th.Start(cl);
                    }*/

                }

                

            }
            fs.Close();
        }
        private void IDT_client(object client)
        {
            Client cl = (Client)client;
            Socket Interface = Config.IDT_Service.Connect(cl, IdMaterial);
            

        }


    }
}
