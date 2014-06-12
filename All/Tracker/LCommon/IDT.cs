using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using bc = System.BitConverter;


namespace P2P_client
{

    /// <summary>
    /// Инкапсулирует работу с протоколом NDT.
    /// Протокол IDT (Indirect Transactions, протокол непрямых операций) предназначен для передачи команд между
    /// клиентами, которые не могут установить прямые соединения друг с другом.
    /// Класс создает интерфейс, который позвояет принимать команды протокола IDT и преобразовывать их в команды
    /// обычного протокола клиент-клиент, если целевой узел досупен промежуточному компьютеру.
    /// 
    /// Служба выступает в роли сервера, который принимает входящие поделючения, проверяет доступность требуемого
    /// компьютера, если тот не доступен, опрашивает 
    /// </summary>
    public class IDT
    {
        Thread thIDTService;
        List<Client> targets = new List<Client>();

        /// <summary>
        /// Конструктор, создает поток службы
        /// </summary>
        public IDT()
        {
            thIDTService = new Thread(IDTService);
            thIDTService.IsBackground = true;

        }

        /// <summary>
        /// Запускает службу на выполнение 
        /// </summary>
        public void Start()
        {
            if (thIDTService.ThreadState == ThreadState.Unstarted)
                thIDTService.Start();
            else if (thIDTService.ThreadState == ThreadState.Suspended)
                thIDTService.Resume();
        }
        /// <summary>
        /// Останавливает службу
        /// </summary>
        public void Stop()
        {
            thIDTService.Suspend();
        }

        /// <summary>
        /// Ожидает подключение и запускает цикл сообщений IDT
        /// </summary>
        private void IDTService()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            
            sock.Bind(new IPEndPoint(IPAddress.Any, 30095));
            sock.Listen((int)SocketOptionName.MaxConnections);
           Connect:
            Connection con = new Connection();
            con.Socket = sock.Accept();
            con.Thread = new Thread(IDTSession);
            con.Thread.Start(con);
            goto Connect;
           
        }

        /// <summary>
        /// Процедура, содержащая цикл обработки команд протокола IDT
        /// </summary>
        /// <param name="con">Объект подключения с инициатором сеанса</param>
        private void IDTSession(object con)
        {
            Connection c = (Connection)con;
            
            NetworkStream nStream = new NetworkStream(c.Socket);

            /*общие данные*/
            Socket sock = null;
            int material = 0;
            Client targetHost = new Client() ;

            IDT_Protocol com = (IDT_Protocol)nStream.ReadByte();
            nStream.Close();

            switch (com)
            {
                case IDT_Protocol.IsLink:
                    targets.Add(targetHost);
                    sock = TryConnect(c, out targetHost, material);
                    break;
                case IDT_Protocol.IsMaterial:
                    material = CheckMaterial(c,sock, targetHost);
                    break;
                case IDT_Protocol.GetPart:
                    RequestPart(c, sock, material, targetHost);
                    break;
                case IDT_Protocol.TerminateTransaction:
                    targets.Remove(targetHost);
                    break;

            }



        }

        /// <summary>
        /// Метод запрашивает часть данных у подключенного IDT сервиса, либо конечного компьютера и отправляет ее инициатору подключения - 
        /// другому IDT сервису, либо первому компьютеру в цепочке
        /// </summary>
        /// <param name="c">Объект подключения с инициатором</param>
        /// <param name="sock">Сокет, подключенный к следующему компьютеру в цепочке</param>
        /// <param name="material">Идентификатор раздачи</param>
        /// <param name="targetHost">Целевой компьютер, для достижения которого строилась цепочка</param>
        private void RequestPart(Connection c, Socket sock, int material, Client targetHost)
        {
            NetworkStream nStream = new NetworkStream(sock);
            NetworkStream ansStream = new NetworkStream(c.Socket);

            byte[] bcom = new byte[] { (byte)C_C_Protocol.GetPart };
            byte[] bmat = bc.GetBytes(material);
            byte[] bfile = new byte[4];
            byte[] bpart = new byte[4];

            byte[] bsize = new byte[4];

            ansStream.Read(bfile, 0, 4);
            ansStream.Read(bpart, 0, 4);
            ansStream.Read(bsize, 0, 4);

            int size = bc.ToInt32(bsize, 0);

            // Работа в режиме целевого компьютера
            if (IsTarget(sock, targetHost))
            {

                nStream.Write(bcom, 0, 1);
                nStream.Write(bmat, 0, 1);
                nStream.Write(bfile, 0, 4);
                nStream.Write(bpart, 0, 4);

                C_C_Protocol resp = (C_C_Protocol)nStream.ReadByte();
                int ispart = nStream.ReadByte();
                if (resp != C_C_Protocol.SendPart || ispart == 0)
                {
                    ansStream.WriteByte((byte)IDT_Protocol.SendPart);
                    ansStream.WriteByte(0);
                    return;
                }
                nStream.Read(new byte[12], 0, 12);
                
                byte[] buffer = new byte[size];
                for (int i = 0; i != size; )
                {
                    i += nStream.Read(buffer, i, size - i);
                }
                ansStream.WriteByte((byte)IDT_Protocol.SendPart);
                ansStream.WriteByte(0);
                ansStream.Write(buffer, 0, size);
            }
            else//работа в режиме промежуточной ячейки сети
            {
                IDT_Protocol resp = (IDT_Protocol) nStream.ReadByte();
                int status = nStream.ReadByte();
                if (resp != IDT_Protocol.SendPart) return;
                if (status == 0)
                {
                    ansStream.WriteByte((byte)IDT_Protocol.SendPart);
                    ansStream.WriteByte((byte)status);
                    return;
                }
                byte[] buffer = new byte[size];
                for (int i = 0; i != size; )
                {
                    i += nStream.Read(buffer, i, size - i);
                }

                ansStream.WriteByte((byte)IDT_Protocol.SendPart);
                ansStream.WriteByte((byte)status);
                ansStream.Write(buffer, 0, size);
            }
        }

        /// <summary>
        /// Проверяет наличие раздачи на конечном компютере, команда передается по цепочке
        /// </summary>
        /// <param name="c">Объект подключения с инициатором</param>
        /// <param name="sock">Сокет, подключенный к следующему компьютеру в цепочке</param>
        /// <param name="targetHost">Целевой компьютер, для достижения которого строилась цепочка</param>
        /// <returns>Возвращает номер раздачи, полученный от предыдущего компьютера в цепи</returns>
        private int CheckMaterial(Connection con, Socket sock, Client targetHost)
        {
            NetworkStream nStream = new NetworkStream(sock);
            NetworkStream ansStream = new NetworkStream(con.Socket);
            byte[] temp = new byte[4];
            ansStream.Read(temp, 0, 4);
            int material = bc.ToInt32(temp,0);

            if (IsTarget(sock, targetHost)) // работа в режиме целевого хоста
            {
                nStream.Write(new byte[] {(byte)C_C_Protocol.IsMaterial},0,1);
                nStream.Write(temp, 0, 4);
                byte[] resp = new byte[6];
                nStream.Read(resp,0,6);
                if (resp[5] == 1)
                {
                    ansStream.Write(new byte[] { (byte)IDT_Protocol.MaterialConfirmed }, 0, 1);
                    return material;
                }
                else
                {
                    ansStream.Write(new byte[] { (byte)IDT_Protocol.MaterialNotFound }, 0, 1);
                    return 0;
                }
            }
            else // работа в режиме ячейки
            {
                nStream.Write(new byte[] { (byte)C_C_Protocol.IsMaterial }, 0, 1);
                IDT_Protocol resp = (IDT_Protocol)nStream.ReadByte();
                if (resp == IDT_Protocol.MaterialNotFound)
                    return 0;
                else return material;
            }
           
        }

        /// <summary>
        /// Сравнивает IP и порт следующего компьютера в цепочке.
        /// </summary>
        /// <param name="sock">Сокет следующего компьютера в сети</param>
        /// <param name="targetHost">Целевой компьютер</param>
        /// <returns>True, если следующий компьютер в цепи - конечный</returns>
        private static bool IsTarget(Socket sock, Client targetHost)
        {
            IPAddress ip = ((IPEndPoint)sock.RemoteEndPoint).Address;
            if (targetHost.ip == ip)
                return true;
            return false;
        }

        /// <summary>
        /// Пытается произвести рекурсивное подключение от компьютера к компьютеру, пока не установит цепочку связей до конечного компьютера
        /// </summary>
        /// <param name="con">Предыдущий компьютер в сети</param>
        /// <param name="targetHost">Целевой компьютер</param>
        /// <param name="mat">Ид раздачи</param>
        /// <returns>Сокет, подключенный к следующему компьютеру в цепочке, либо null, если маршрут не удалось установить</returns>
        Socket TryConnect(Connection con, out Client targetHost, int mat)
        {

            NetworkStream nStream = new NetworkStream(con.Socket);
            byte[] buf = new byte[6];
            nStream.Read(buf, 0, 6);
            targetHost = new Client();
            targetHost.Deserialize(buf);


            foreach (Client c in targets)
            {
                if (c == targetHost)
                {
                    nStream.WriteByte((byte)(IDT_Protocol.TerminateTransaction));
                    return null;
                }
            }
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            
            try
            {
                sock.Connect(targetHost.ip, targetHost.port);
                sock.Send(new byte[] { (byte)C_C_Protocol.Connect});
                byte[] receive = new byte[2];
                sock.Receive(receive, 0, 1, SocketFlags.None);
                sock.Receive(receive, 1, 1, SocketFlags.None);
                if((C_C_Protocol) receive[0] != C_C_Protocol.Connected || receive[1] ==0)
                    { sock.Close(); throw new SocketException(); }

                sock.Send(new byte[] {(byte)C_C_Protocol.IsMaterial});
                byte[] temp = bc.GetBytes(mat);
                sock.Send(temp);
                sock.Receive(temp, 1, SocketFlags.None);
                if (temp[0] != (byte)C_C_Protocol.MaterialResponse)
                { sock.Close(); throw new SocketException(); }

                return sock;
            }
            catch (SocketException)
            {
                foreach (Material m in ProgramMaterials.MaterialsArray)
                {
                    foreach (Connection c in m.Connections)
                    {
                        IPAddress ip = ((IPEndPoint)c.Socket.RemoteEndPoint).Address;
                        short port = (short)((IPEndPoint)c.Socket.RemoteEndPoint).Port;
                        try { sock.Connect(ip, port); }
                        catch (Exception) { continue; }
                        return Connect(targetHost, mat);

                    }
                }
                return null;
            }

            
        }

// методы работы с исходящим подключением


        /// <summary>
        /// Запускает процесс поиска маршрута
        /// </summary>
        /// <param name="target">Целевой компьютер</param>
        /// <param name="mat">Ид раздачи</param>
        /// <returns>Сокет, подключенный к следующему компьютеру в цепочке, либо null, если маршрут не удалось установить</returns>
        public Socket Connect(Client target, int mat)
        {
            foreach (Material m in ProgramMaterials.MaterialsArray)
            {
                foreach (Connection c in m.Connections)
                {
                    try
                    {
                    IPAddress ip = ((IPEndPoint)c.Socket.RemoteEndPoint).Address;
                    short port = 30095;
                    Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    try { sock.Connect(ip, port); }
                    catch (Exception) { continue; }
                    NetworkStream comStr = new NetworkStream(sock);
                    comStr.WriteByte((byte)IDT_Protocol.Connect);
                    int resp = comStr.ReadByte();

                    if(resp != (int)IDT_Protocol.Connected)
                    { TerminateConnection(comStr); continue; }  
                    
                    byte[] temp = target.Serialize();
                    comStr.WriteByte((byte)IDT_Protocol.IsLink);
                    comStr.Write(temp, 0, 6);
                    resp = comStr.ReadByte();

                    if(resp != (int)IDT_Protocol.LinkAccesible)
                    { TerminateConnection(comStr); continue; }

                    comStr.WriteByte((byte)IDT_Protocol.IsMaterial);
                    comStr.Write(bc.GetBytes(mat), 0, 4);
                    IDT_Protocol res = (IDT_Protocol)comStr.ReadByte();
                    if(res == IDT_Protocol.MaterialNotFound)
                        comStr.WriteByte((byte)IDT_Protocol.TerminateTransaction);

                    return sock;
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// завершает подключение
        /// </summary>
        /// <param name="comStr"></param>
        public static void TerminateConnection(NetworkStream comStr)
        {
            comStr.WriteByte((byte)IDT_Protocol.TerminateTransaction);
        }
        
        /// <summary>
        /// Запрашивает файл по цепочке компьютеров
        /// </summary>
        /// <param name="sock">Адрес следующей ячеки сети</param>
        /// <param name="file">Номер файла в раздаче</param>
        /// <param name="part">Номер части файла</param>
        /// <param name="size">Размер части</param>
        /// <returns>Скачанную часть</returns>
        public byte[] GetPart(Socket sock, int file,int part,int size)
        {
            try
            {
                NetworkStream nStream = new NetworkStream(sock);
                nStream.WriteByte((byte)IDT_Protocol.GetPart);
                byte[] bFile = bc.GetBytes(file);
                byte[] bPart = bc.GetBytes(part);
                byte[] bSize = bc.GetBytes(size);
                nStream.Write(bFile, 0, 4);
                nStream.Write(bPart, 0, 4);
                nStream.Write(bSize, 0, 4);

                IDT_Protocol resp = (IDT_Protocol) nStream.ReadByte();
                int stat = nStream.ReadByte();
                if (resp != IDT_Protocol.SendPart || stat == 0)
                    return new byte[0];
                byte[] buffer = new byte[size];
                for (int i = 0; i != size; )
                {
                    i += nStream.Read(buffer, i, size - i);
                }
                return buffer;
            }
            catch (SocketException)
            {
                return new byte[0];
            }
        }
    }
}
