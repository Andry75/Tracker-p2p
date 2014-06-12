using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace P2P_client
{
    [Serializable]
    public class Meta_File
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        int id;
        /// <summary>
        /// Общий размер раздачи
        /// </summary>
        long Size_Treck;
        /// <summary>
        /// Размер части
        /// </summary>
        int Size_Part;
        /// <summary>
        /// Адрес и порт сервера
        /// </summary>
        Client Server;
        /// <summary>
        /// Массив файлов
        /// </summary>
        File[] Files;
        /// <summary>
        /// Идентификатор раздачи
        /// </summary>
        int id_matirial;
        /// <summary>
        /// Название раздачи
        /// </summary>
        public string name;

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public int ID_Client
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Общий размер раздачи
        /// </summary>
        public long Size_treck
        {
            get { return Size_Treck; }
            set { Size_Treck = value; }
        }

        /// <summary>
        /// Размер части
        /// </summary>
        public int Size_part
        {
            get { return Size_Part; }
            set { Size_Part = value; }
        }

        /// <summary>
        /// Адрес и порт сервера
        /// </summary>
        public Client IP_Port_Server
        {
            get { return Server; }
            set { Server = value; }
        }

        /// <summary>
        /// Массив файлов
        /// </summary>
        public File[] files
        {
            get { return Files; }
            set { Files = value; }
        }

        /// <summary>
        /// Идентификатор раздачи
        /// </summary>
        public int ID_matirial
        {
            get { return id_matirial; }
            set { id_matirial = value; }
        }

    }
}