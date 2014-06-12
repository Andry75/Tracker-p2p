using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2P_client
{
    public enum C_C_Protocol : byte
    {
        /// <summary>
        /// Подключение к клиенту
        /// </summary>
        Connect=0,
        /// <summary>
        /// Результат подключения [Accepted:byte]
        /// </summary>
        Connected=1,
        /// <summary>
        /// Запрос на наличие материала [Material:int]
        /// </summary>
        IsMaterial=2,
        /// <summary>
        /// Ответ на запрос о наличии материала [Material:int] [IsMaterial:byte]
        /// </summary>
        MaterialResponse=3,
        /// <summary>
        /// Запрос на часть материла [Material:int] [File:int][Part:int]
        /// </summary>
        GetPart=4,
        /// <summary>
        /// Передача части материала[признак наличия части:byte] [Material:int] [File:int] [Part:int][PartData:byte[]]
        /// </summary>
        SendPart=5,
        /// <summary>
        /// Завершить подкелючение
        /// </summary>
        EndConnection=6,
        NotConnected =7,
        InNotmaterial = 8

    }
}
