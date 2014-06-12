using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace P2P_client
{
    [Serializable]
    public class Client
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
        public  Client() { }
        /// <summary>
        /// Преобразует данные объекта в массив байт
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
                byte b = Convert.ToByte(Ip.Substring(pointPos)); // отделяем байт
                Ip = Ip.Substring(pointPos+1); // отсекаем байт с запятой
                Ip_Port[i] = b; // заносим очередной байт в массив
            }
            byte[] Port = BitConverter.GetBytes(port); // получаем бинарное представление номера
            Ip_Port[4] = Port[0]; // заносим его
            Ip_Port[5] = Port[1]; // в массив

            return Ip_Port; // возвращаем номер порта 
        }
        /// <summary>
        /// Преобразуем массив байт в IP -адрес и номер открытого порта
        /// </summary>
        /// <param name="Ip_Port">Массив, содержащий адрес и номер</param>
        public void Deserialize(byte[] Ip_Port)
        {
            if(Ip_Port.Length != 6) // если длинна массива не 6
                throw new ArgumentOutOfRangeException(); // нам пытаются впарить какую-то хрень

            byte[] adr = new byte[4];
            for (int i = 0; i < 4; i++) adr[i] = Ip_Port[i];
            ip = new IPAddress(adr);
            port = BitConverter.ToInt16(Ip_Port, 4);
        }
       public static bool  operator ==(Client a, Client b)
        {
            if (a.ip == b.ip && a.port == b.port)
                return true;
            return false;
        }
       public static bool operator !=(Client a, Client b)
       {
           if (a.ip != b.ip || a.port != b.port)
               return true;
           return false;
       }
     
    }
}
