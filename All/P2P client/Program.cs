using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using bc = System.BitConverter;

namespace P2P_client
{
    class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {



            Config.Open();
            System.Threading.Thread th = new System.Threading.Thread(AcceptConnection);
            th.IsBackground = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ClearTemps();
            GetConfig();
            if (Config.CTE_Enaibled) Config.CTE_Service.Start();
            if (Config.IDT_Enaibled) Config.IDT_Service.Start();
            th.Start();
            Application.Run(new GUI.MainWindow());
            System.Diagnostics.Process.GetCurrentProcess().Close();
            System.Diagnostics.Process.GetCurrentProcess().Dispose();
            
        }

        private static void ClearTemps()
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo("Temp");
            if (!dir.Exists)
            {
                dir.Create();
            }
            System.IO.FileInfo[] files = dir.GetFiles();

            foreach (System.IO.FileInfo file in files)
            {
                file.Delete();
            }
        }

        private static void GetConfig()
        {
            Config.Open();
        }

        /// <summary>
        /// Точка входа потока, выполняющая прием подключения
        /// </summary>
        static void AcceptConnection()
        {

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);// открываем серверный сокет
            sock.Bind(new IPEndPoint(IPAddress.Any, Config.Port));// привязываем к точке
            sock.Listen((int)SocketOptionName.MaxConnections);// ставим на прослушку
        connect://метка подключения
            Socket socketOpened = sock.Accept();//получаем подключенный сокет для входящего подключения
            byte[] buff = new byte[4];//буффер для обмена
            socketOpened.Receive(buff, 1, SocketFlags.None);// считываем  команду
            if (buff[0] == (byte)CTE_Protocol.Connect)
            {
                System.Threading.Thread th = new System.Threading.Thread(Config.CTE_Service.IncomingConnection);
                th.Start(socketOpened);
            }
            else if (buff[0] != (byte)C_C_Protocol.Connect)//если нам пришла не команда подключения - нам пытаются передать что-то явно не то..
            {
                socketOpened.Close();//по-этому отключаемся
                goto connect;//и ждем нового подключения
            }
            buff[0] = (byte)C_C_Protocol.Connected;// иначе высылаем код ответа на подключение
            socketOpened.Send(buff, 1, SocketFlags.None);//


            if (Config.Connections == Config.ConnectionsMax) // если достигнуто максимальное количество подключений
            {
                buff[0] = 0; ;// в буфер заносим респонс о не подключении 
                socketOpened.Send(buff, 1, SocketFlags.None);// высылаем 
                socketOpened.Close();//и отключаемся
                goto connect; //ожидаем новое подключение
            }
            //иначе
            buff[0] = 1;
            socketOpened.Send(buff, 1, SocketFlags.None);// высылаем 
            Config.Connections++;

            socketOpened.Receive(buff, 1, SocketFlags.None);// считываем в буфер 1байт команды 


            if (buff[0] != (byte)C_C_Protocol.IsMaterial) // если пришел не код команды проверки материала - 
            {
                // передача ошибочна - 
                socketOpened.Close();// отключаемся
                goto connect;// к новому подключению
            }
            socketOpened.Receive(buff, 4, SocketFlags.None);// считываем в буфер 4 байта номера раздачи
            int material = BitConverter.ToInt32(buff, 0);// переводим массив байт в целое число 
            byte t = buff[0];
            buff[0] = (byte)C_C_Protocol.MaterialResponse;
            try
            {
                socketOpened.Send(buff, 1, SocketFlags.None);
            }
            catch(Exception)
            {
                goto connect;
            }
            foreach (Material mat in ProgramMaterials.MaterialsArray) // перебор списка раздач
            {
                if (mat.IdMaterial == material) // если обнаружили запрошенную раздачу
                {
                    
                    buff[0] = 1; // респонс - 1(успешно)
                    socketOpened.Send(buff, 1, SocketFlags.None); // высылаем респонс
                    buff[0] = t;
                    socketOpened.Send(buff, 4, SocketFlags.None); // высылаем респонс
                    // передаем объекту материала, открытый сокет, тщетно уповая, что он сам разберется, что с ним делать :)
                    mat.UploadThreadStart(ref socketOpened); 
                    // ждем новое подключение
                    goto connect;
                }
            }
            // иначе 
            buff[0] = 0;// в буфер заносим респонс о наличии раздачи 0(провал)
            socketOpened.Send(buff, 1, SocketFlags.None);// высылаем 
            goto connect;
        }
        


       
    }
}
