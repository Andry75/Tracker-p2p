using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using bc = System.BitConverter;
using System.Threading;
using System.IO;

namespace P2P_client
{
    /// <summary>
    /// Протокол обмена таблицами клиентов
    /// Все команды имеют длинну 1 байт + длинна аргументов
    /// </summary>
    public enum CTE_Protocol
    {
        /// <summary>
        /// Уведомляет обработчик входящих подключений о запросе к CTE сервису клиента
        /// </summary>
        Connect = 10,               //no arguments

        /// <summary>
        /// Команда статуса подключения
        /// </summary>
        Connected = 11,             // 1 byte: 1 - connected, 0 - no

        /// <summary>
        /// Запрос у CTE сервиса о наличии определенного материала
        /// </summary>
        IsMaterial = 12,            // 4 bytes: Material ID 

        /// <summary>
        /// Ответ о наличии материала
        /// </summary>
        ResponseMaterial = 13,      // 4 bytes: Material ID, 1 byte: response(1/0)

        /// <summary>
        /// Запрос на выдачу таблицы клиентов
        /// </summary>
        GetClientsTable = 14,       // 4 bytes: Material ID

        /// <summary>
        /// Сигнал о выдаче таблицы клиентов
        /// </summary>
        ReceiveClientsTable = 15,   //   4 bytes: clients number; array of 4 (ip) + 2(port) bytes 
        
        /// <summary>
        /// Команда отключения
        /// </summary>
        Disconnect = 16             // no arguments
    }



    /// <summary>
    /// Класс сервиса обмена таблицами клиентов
    /// Clients Tables Exchange
    /// </summary>
    public class CTE
    {
        /// <summary>
        /// Поток, в котором выполняется сулжба CTE 
        /// </summary>
        Thread thCTEService;
        
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CTE()
        {
            thCTEService = new Thread(CTEService);
            thCTEService.IsBackground = true;
        }

        /// <summary>
        /// Запускает службу CTE на выполнение, либо восстанавливает ее работу
        /// </summary>
        public void Start()
        {
            if (thCTEService.ThreadState == ThreadState.Unstarted)
                thCTEService.Start();
            else if(thCTEService.ThreadState == ThreadState.Suspended)
                thCTEService.Resume();
        }

        /// <summary>
        /// Останавливает службу CTE
        /// </summary>
        public void Stop()
        {
            thCTEService.Suspend();
        }

        /// <summary>
        /// Процедура службы
        /// Выполняет опрос клиентов в отдельном потоке
        /// </summary>
        void CTEService()
        {
            foreach (Material m in ProgramMaterials.MaterialsArray)
            {
                foreach (Connection con in m.Connections)
                {
                    CTERequest(con);
                }
            }
        }

        /// <summary>
        /// Реализация опроса клиентов
        /// </summary>
        /// <param name="con">Объект подключения</param>
        private void CTERequest(Connection con)
        {
        
            try
            {
                //Выполняем подключение к текущему клиенту
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                IPAddress ip = ((IPEndPoint)con.Socket.RemoteEndPoint).Address;
                short port = (short)((IPEndPoint)con.Socket.RemoteEndPoint).Port;
                sock.Connect(ip, port);
                NetworkStream nStream = new NetworkStream(sock);


                //записываем команду подключения
                nStream.WriteByte((byte)CTE_Protocol.Connect);
                //получаем результат
                if (nStream.ReadByte() != (byte)CTE_Protocol.Connected) return;
                if (nStream.ReadByte() != 1) return;
               

                //производим перебор всех раздач
                foreach (Material m in ProgramMaterials.MaterialsArray)
                {
                    nStream.WriteByte((byte)CTE_Protocol.IsMaterial);//запрашиваем наличие раздачи у клиента
                    byte[] t = bc.GetBytes(m.IdMaterial);
                    nStream.Write(t, 0, 4);// отправляем ее номер
                    if (nStream.ReadByte() != (int)CTE_Protocol.ReceiveClientsTable) // если получена не команда получения клиентов
                    {
                        nStream.WriteByte((byte)CTE_Protocol.Disconnect);
                        nStream.Close();// отключаемся
                    }

                    // иначе 
                    //получаем количество клиентов
                    nStream.Read(t, 0, 4);
                    int num = bc.ToInt32(t, 0);
                    // создаем временный файл с именем, содержащим номер раздачи
                    string path = "Temp\\cte"+m.IdMaterial.ToString()+".tmp";

                    FileStream fStream = new FileStream(path, FileMode.Append | FileMode.Create, FileAccess.Write);
                    //получем из сети пары ип/порт и записываем их в файл
                    for (int i = 0; i < num; i++)
                    {
                        byte[] temp = new byte[6];
                        nStream.Read(temp, 0, 6);
                        fStream.Write(temp, 0, 6);
                    }
                    fStream.Close();//закрываем файл и переходим к следующей раздаче
                }
                nStream.Close();
            }
            catch (Exception)
            {
                return;
            }

        }

        



        /// <summary>
        /// Обрабатывает входящее подключение.
        /// Если служба CTE не запущена, вызов метода игнорируется
        /// </summary>
        /// <param name="sock">Открытый сокет, к которому подключен клиент</param>
        public void IncomingConnection(object sock)
        {
            NetworkStream nStream = new NetworkStream((Socket)sock);
            nStream.WriteByte((byte)CTE_Protocol.Connected);
            
            if(Config.Connections < Config.ConnectionsMax && thCTEService.ThreadState == ThreadState.Running)
            {
                nStream.WriteByte((byte)1);
                Config.Connections++;
            }
            else
            {
                nStream.WriteByte((byte)0);
                return;
            }

            //цикл обработки сообщений от подключенного клиента
            while (true)
            {
                byte comm = (byte)nStream.ReadByte();

                switch (comm)
                {
                    case (byte) CTE_Protocol.IsMaterial:
                        IsMaterial(nStream);
                        break;
                    case (byte) CTE_Protocol.GetClientsTable:
                        GetClientsTable(nStream);
                        break;
                    case (byte) CTE_Protocol.Disconnect:
                        nStream.Close();
                        return;
                }
            }

            
        }


        /// <summary>
        /// Производит отправку уведомления о наличии материала
        /// </summary>
        /// <param name="nStream">Подключенный сетевой поток</param>
        private void IsMaterial(NetworkStream nStream)
        {
            byte[] temp = new byte [4];
            nStream.Read(temp,0, 4);
            int IdMaterial = bc.ToInt32(temp,0);
            byte response = 0;
            foreach (Material m in ProgramMaterials.MaterialsArray)
            {
                if (m.IdMaterial == IdMaterial)
                {
                    response = 1;
                    break;
                }
            }
            nStream.WriteByte(response);
        }

        /// <summary>
        /// Выдает таблицу клиентов
        /// </summary>
        /// <param name="nStream">Открытый сетевой поток</param>
        private void GetClientsTable(NetworkStream nStream)
        {
            byte[] temp = new byte[4];
            nStream.Read(temp, 0, 4);
            int IdMaterial = bc.ToInt32(temp, 0);

            nStream.WriteByte((byte)CTE_Protocol.ReceiveClientsTable);

            foreach (Material m in ProgramMaterials.MaterialsArray)
            {
                if (m.IdMaterial == IdMaterial)
                {

                    int number = m.Connections.Count;

                    temp = bc.GetBytes(number);

                    for (int i = 0; i < number; i++)
                    {
                        IPEndPoint ip = (IPEndPoint) m.Connections[i].Socket.RemoteEndPoint;
                        Client cl = new Client(ip.Address, (short)ip.Port);
                        temp = cl.Serialize();
                        nStream.Write(temp, 0, 6);
                    }
                }
            }
        }

        
    }
}
