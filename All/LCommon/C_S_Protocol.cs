using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2P_client
{
    public enum CS_Protocol : byte
    {
        /// <summary>
        /// Регистрация на сервере [ID_client:int][port:short]
        /// </summary>
        Regiser = 0,
        /// <summary>
        /// Ответ на запрос о регистрации [Response:byte]
        /// </summary>
        RegisterFeedback = 1,
        /// <summary>
        /// Послать номер раздачи, ждать пиров [ID_material:int]
        /// </summary>
        GetClients = 2,
        /// <summary>
        /// Послать номер раздачи и массив клиентов[ID_material:int][client_number:int][(clients:int[4]+port:short)[]]
        /// </summary>
        ReceiveClients = 3,
        /// <summary>
        /// Послать статы [ID_material:int][Uploaded:ulong][Downloaded:ulong]
        /// </summary>
        UpdateInfo = 4,
        /// <summary>
        /// Подтверждение запроса [Id_material:int][updated:byte]
        /// </summary>
        UpdateInfoFeedback = 5,
        /// <summary>
        /// Завершение сессии запросов
        /// </summary>
        EndTrasaction = 6,
        /// <summary>
        /// Подтверждение окончания сессии
        /// </summary>
        EndTrasactionAccepted = 7


    }

}