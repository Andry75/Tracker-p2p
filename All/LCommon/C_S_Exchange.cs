using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

using bc = System.BitConverter;

namespace P2P_client
{
    public class C_S_Exchange
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);


        public C_S_Exchange(IPAddress ip, short port)
        {
            try
            {
                socket.Connect(ip, port);
            }
            catch (SocketException) { throw new Exceptions.ServerConnection();}
        }

        public C_S_Exchange(string host, short port)
        {
            socket.Connect(host, port);
        }


        /// <summary>
        /// Обрабатывает входящую команду от сервера
        /// </summary>
        public void GetMessage()
        {
            //NetworkStream nStream = new NetworkStream(socket); // сетевой поток ввода-вывода
            byte[] code = new byte[1]; // байт для считываения команды
            //nStream.Read(code, 0, 1); // считываем из потока команду
            socket.Receive(code, 1, SocketFlags.None);
            //nStream.Close(); // закрываем поток ввода/вывода

            switch ((CS_Protocol)code[0]) // перебираем команды
            {
                case CS_Protocol.RegisterFeedback:
                    RegisterFeedback();
                    break;
                case CS_Protocol.ReceiveClients:
                    ReceiveClients();
                    break;
                case CS_Protocol.UpdateInfoFeedback:
                    UpdateInfoFeedback();
                    break;
                case CS_Protocol.EndTrasactionAccepted:
                    socket.Close();
                    break;
                default :
                    throw new Exceptions.NotDocumentedException();
                    
            }


        }

       

        /// <summary>
        /// Инкапсулирует запрос регистрации
        /// </summary>
        /// <param name="Client">Идентификатор клиента</param>
        /// <param name="Port">Номер порта для входящих подключений клиента</param>
        public void Register(int Client, short Port)
        {
            // преобразуем величины в байты/байтовые массивы
            byte[] command = new byte[1]; command[0] = (byte)CS_Protocol.Regiser; // получаем байтовове представление кода команды
            byte[] client = bc.GetBytes(Client); // получаем байтовое представдление идентификатора клиента
            byte[] port = bc.GetBytes(Port); // получаем байтовое представление номера открытого порта   

            try
            {
                //NetworkStream nStream = new NetworkStream(socket); // открываем поток ввода вывода через открытый сокет
                //nStream.WriteByte(command); // записывает код комады
                //nStream.Write(client, 0, 4); // записываем идентификатор клиента
                //nStream.Write(port, 0, 2); // записываем ноер открытого порта
                //nStream.Close();
                socket.Send(command, 1, SocketFlags.None);
                socket.Send(client, 4, SocketFlags.None);
                socket.Send(port, 2, SocketFlags.None);
            }
            catch (IOException)   // если происходит ошибка ввода вывода
            {
                throw new Exceptions.RegisterException(); // генерируем исключительную ситуацию для данной команды
            }

        }
        /// <summary>
        /// Реализует обработку ответа сервера на запрос о регистрации
        /// </summary>
        public void RegisterFeedback()
        {
            try
            {
                //NetworkStream nStream = new NetworkStream(socket); // открываем поток ввода вывода через открытый сокет
                //int code = nStream.ReadByte(); // считываем из потока код завершения операции регистрации
                //nStream.Close();
                byte[] code = new byte[1]; 
                socket.Receive(code, 1, SocketFlags.None);
                if (code[0] == 0) return; // если код - 0 - всё нормально
                else if (code[0] == 1) throw new Exceptions.RegistrationFailed(); // если - 1 - ошибка(данные неверны)
                else if (code[0] == 2) throw new Exceptions.RegistrationFeedBackException(); // если код - 2 - регистрация провалилась по вине сервера
                else throw new Exceptions.NotDocumentedException(); // код завершения операции непредусмотрен
            }
            catch (IOException)//если произошла ошибка ввода-вывода - уведомляем приложение
            {
                throw new Exceptions.RegistrationFeedBackException();
            }
        }

        /// <summary>
        /// Реализует запрос на получение клиентов по раздаче
        /// </summary>
        /// <param name="material">Идентификатор раздачи</param>
        public void GetClients(int material)
        {
            byte[] com = new byte[1]; com[0] = (byte)CS_Protocol.GetClients; // получаем битовое представление комады
            byte[] mat = bc.GetBytes(material); // получаем битовое представление материала
            try
            {
                //NetworkStream nStream = new NetworkStream(socket); // открываем поток сетовоо ввода/вывода
                //nStream.WriteByte(com); // записываем в поток команду
                //nStream.Write(mat, 0, 4); // записываем в поток идентификатор материала
                //nStream.Close(); // закрываем поток
                socket.Send(com, 1, SocketFlags.None);
                socket.Send(mat, 4, SocketFlags.None);
            }
            catch (IOException)
            {
                throw new Exceptions.ClientsException(); // если произошла ошибка уведомляем приложение
            }
        }

        /// <summary>
        /// Реализует получение клиентов
        /// </summary>
        public void ReceiveClients()
        {
            try
            {
                //NetworkStream nStream = new NetworkStream(socket); // открываем поток чтения
                byte[] temp = new byte[6]; // временный массив
                //nStream.Read(temp, 0, 4); // считываем номер материала
                socket.Receive(temp, 4, SocketFlags.None);
                int material = bc.ToInt32(temp, 0); // преобразуем в целое
                if (material == 0) throw new Exceptions.ReceiveClients(); // если получили ид.=0, то такой раздачи нет
                //nStream.Read(temp, 0, 4); // считываем количество клиентов
                socket.Receive(temp, 4, SocketFlags.None);
                int number = bc.ToInt32(temp, 0); // преобразовываем в целое
                Directory.CreateDirectory("Temp");

                FileStream sWriter = new FileStream("Temp/clients" + material + ".tmp",FileMode.Create);// открываем поток чтения
                // поочередно считываем из потока пары ip - порт
                for (int i = 0; i < number; i++)
                {
                    //nStream.Read(temp, 0, 6); // считали пару
                    socket.Receive(temp, 6, SocketFlags.None);
                    sWriter.Write(temp, 0, 6);
                }
                sWriter.Close();// закрываем потоки
                //nStream.Close();
                
                
            }
            catch (IOException)
            {
                throw new Exceptions.ClientsException();
            }
        }
        /// <summary>
        /// Обновляет статистику
        /// </summary>
        /// <param name="material">Идентификатор материала</param>
        /// <param name="upload">Количество переданного материала</param>
        /// <param name="download">Количество скачанного материала</param>
        public void UpdateInfo(int material, long upload, long download)
        {
            try
            {
                //NetworkStream nStream = new NetworkStream(socket);

                //nStream.WriteByte((byte)CS_Protocol.UpdateInfo);
                byte[] com = new byte[1]; com[0] = (byte)CS_Protocol.UpdateInfo;
                socket.Send(com, 1, SocketFlags.None);
                byte[] temp = Encript.Encode((long)material);// получаем кодированный идентификатор
                //nStream.Write(temp,0,256); // отправляем серверу
                socket.Send(temp, 256, SocketFlags.None);
                temp = Encript.Encode(upload); // получаем кодированную статистику отдачи
                //nStream.Write(temp, 0, 256); // отправляем серверу
                socket.Send(temp, 256, SocketFlags.None);
                temp = Encript.Encode(download); // получаем кодированную статистику скачивания
                //nStream.Write(temp, 0, 256); // отправляем серверу
                socket.Send(temp, 256, SocketFlags.None);
                //nStream.Close();

            }
            catch (IOException)
            {
                throw new Exceptions.UpdateInfoException();
               

            }
        }

        /// <summary>
        /// Вызывается при плучении ответа о завершении обновления статистики
        /// </summary>
        public void UpdateInfoFeedback()
        {
            try
            {
                //NetworkStream nStream = new NetworkStream(socket);
                byte[] t = new byte[1]; 
                //int result = nStream.ReadByte();
                socket.Receive(t, 1, SocketFlags.None);
                if (t[0] == 0) return;
                else if (t[0] == 1) throw new Exceptions.UpdateInfoExceptionFeedBack();
                else if (t[0] == 2) throw new Exceptions.UpdateInfoException();
                else throw new Exceptions.NotDocumentedException();
                
            }
            catch (IOException)
            {
                throw new Exceptions.UpdateInfoException();
            }
        }

       

        /// <summary>
        /// Закрывает сессию обмена данными
        /// </summary>
        public void EndTrasaction()
        {
            byte command = (byte)CS_Protocol.EndTrasaction; // получаем код команды
            byte [] t = new byte[1];
            t[0] = command;

            socket.Send(t, 1, SocketFlags.None);
            //NetworkStream nStream = new NetworkStream(socket); // сетевой поток
            //nStream.WriteByte(command); // записываем команду завершения сессии
            //nStream.Close(); // закрываем сетевой поток
        }
        
       
    }
}
