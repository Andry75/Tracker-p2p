using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace p2p_Server
{
        enum CS_Protocol
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
            /// Послать номер раздачи, ждать пиров [ID_material:int][ID_client:int]
            /// </summary>
            GetClients = 2,
            /// <summary>
            /// Послать номер раздачи и массив клиентов[ID_material:int][client_number:int][(clients:int[4]+port:short)[]]
            /// </summary>
            ReceiveClients = 3,
            /// <summary>
            /// Послать статы [ID_material:int][Uploaded:ulong][Downloaded:ulong][ID_client:int]
            /// </summary>
            UpdateInfo = 4,
            /// <summary>
            /// Подтверждение запроса [Id_material:int][updated:byte]
            /// </summary>
            UpdateInfoFeedback = 5,
            EndTrasaction = 6,
            EndTrasactionAccepted = 7

        }
    }