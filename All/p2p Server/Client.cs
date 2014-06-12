using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace p2p_Server
{
    class Client
    {
        /// <summary>
        /// Поле для хранения адреса клиента
        /// </summary>
        public IPAddress ip;
        /// <summary>
        /// Поле для хранения открытого порта клиента
        /// </summary>
        public short port;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="IP">IP клиента</param>
        /// <param name="Port">Порт для входящих подключений</param>
        public Client(IPAddress IP, short Port)
        {
            ip = IP;
            port = Port;
        }

        public Client()
        {
            // TODO: Complete member initialization
        }

        /// <summary>
        /// Преобразует данные объекта в массив байтов
        /// </summary>
        /// <returns>Массив из 6 байт, представляющих IP-адрес и порт</returns>
        public byte[] Serialize()
        {
            byte[] Ip_Port = new byte[6]; // создаем массив
            
            string Ip = ip.ToString()+".0."; // получаем строку, содержащую IP адрес

            // получаем из строки отдельные байты
            for (int i = 0; i < 4; i++)
            {
                int pointPos = Ip.IndexOf('.'); // ищем точку
                byte b = Convert.ToByte(Ip.Substring(0,pointPos)); // отделяем байт
               
                Ip = Ip.Substring(pointPos+1); // отсекаем байт с запятой
                Ip_Port[i] = b; // заносим очередной байт в массив
            }
            byte[] Port = BitConverter.GetBytes(port); // получаем бинарное представление номера
            Ip_Port[4] = Port[0]; // заносим его
            Ip_Port[5] = Port[1]; // в массив

            return Ip_Port; // возвращаем номер порта 
        }
        /// <summary>
        /// Преобразуем массив байтов в IP -адрес и номер открытого порта
        /// </summary>
        /// <param name="Ip_Port">Массив, содержащий адрес и номер</param>
        public void Deserialize(byte[] Ip_Port)
        {
            if(Ip_Port.Length != 6) // если длинна массива не 6
                throw new ArgumentOutOfRangeException(); // нам пытаются впарить какую-то хрень

            ip = new IPAddress(Ip_Port);
            port = BitConverter.ToInt16(Ip_Port, 4);
        }

       
     
    }
}
