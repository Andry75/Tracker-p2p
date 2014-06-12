using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2P_client
{

    /// <summary>
    /// InDirect Transactions
    /// Протокол непрямых транзакций
    /// </summary>
    /// 
    public enum IDT_Protocol : byte
    {
        /// <summary>
        /// Команда подключения по IDT протоколу
        /// </summary>
        Connect = 20,

        /// <summary>
        /// Код подтверждения подключения
        /// </summary>
        Connected = 21,

        /// <summary>
        /// Уведомление об ошибке подключения
        /// </summary>
        NotConnected = 22,

        /// <summary>
        /// Запрос на проверку доступности хоста
        /// </summary>
        IsLink = 23,   // 4 байта ip + 2 байта порт

        /// <summary>
        /// Уведомление о доступности хоста
        /// Клиент сумел установить маршрут и успешно подключился напрямую, либо через посредников
        /// </summary>
        LinkAccesible = 24,

        /// <summary>
        /// Хост недоступен
        /// Клиент на хосте обошел весь список подключенных к нему клиентов, но так и не смог установить маршрут для связи
        /// </summary>
        LinkUnaccesible = 25,


        /// <summary>
        /// Запрос на наличие материала
        /// </summary>
        IsMaterial = 26, // 4 байта - ид материала

        /// <summary>
        /// наличие материала подтверждено
        /// </summary>
        MaterialConfirmed = 27, // 

        /// <summary>
        /// Материал не найден
        /// </summary>
        MaterialNotFound = 28,

        /// <summary>
        /// Запрос на выдачу части
        /// </summary>
        GetPart = 29, // 4 байта - номер файла; 4 бата - номер части, 4 байта - размер части
        //Подразумевается, что промежуточные клиенты не имеют данных о раздаче и не знают размер части

        /// <summary>
        /// Команда получения части
        /// </summary>
        SendPart = 30,// 1 байт - подтверждение получения

        /// <summary>
        /// Команда для обрыва транзакции
        /// </summary>
        TerminateTransaction = 31,

    }
}
