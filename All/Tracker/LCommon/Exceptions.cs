using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2P_client
{
    
    namespace Exceptions
    {
        /// <summary>
        /// Идентифицирует исключительную ситуацию при попытке регистрации.
        /// Возникает в случае ошибки ввода вывода при попытке отправить запрос о регистрации серверу
        /// </summary>
        public class RegisterException : Exception
        {
            public RegisterException() { } 
        }
        /// <summary>
        /// Идентифицирует исключительную ситуацию при провале регистрации
        /// Возникает при ошибке регистрации, произошедшей по вине сервера
        /// </summary>
        class RegistrationFeedBackException : Exception
        {
            public RegistrationFeedBackException() { }
        }

        /// <summary>
        /// Идентифицирует исключительную ситуацию, возникающую при передаче серверу неверных данных
        /// </summary>
        class RegistrationFailed : Exception
        {
            public RegistrationFailed() { }
        }
        /// <summary>
        /// Происходит при запросе клиентов, если произошел какой-либо сбой ввода-вывода
        /// </summary>
        class ClientsException : Exception
        {
            public ClientsException() { }
        }
        /// <summary>
        /// Происходит при указании неверного номера раздачи
        /// </summary>
        class ReceiveClients : Exception
        {
            public ReceiveClients() { }
        }

        /// <summary>
        /// Происходит при получении от сервера команд, или данных, не задокументированных в текущей версии протокола
        /// </summary>
        class NotDocumentedException : Exception
        {
            public NotDocumentedException () { }
        }

        /// <summary>
        /// Возникает при ошибке ввода-вывода в результате обновления статистики
        /// </summary>
        class UpdateInfoException : Exception
        {
            public UpdateInfoException() { }
        }
        /// <summary>
        /// Возникает при некорректных данных, отправленых серверу
        /// </summary>
        class UpdateInfoExceptionFeedBack : Exception
        {
            public UpdateInfoExceptionFeedBack() { }
        }
        /// <summary>
        /// Возникает при отсутсвии материала у клиента.
        /// Необходимо удалить его из очереди.
        /// </summary>
        class IsNotMaterial : Exception
        {
            public IsNotMaterial() { }
        }
        /// <summary>
        /// Возникает при передаче адреса несуществующего материала
        /// </summary>
        class IsNotMetafile : Exception
        {
            public IsNotMetafile() { }
        }

        /// <summary>
        /// Возникает при передаче адреса несуществующего материала
        /// </summary>
        class WrongMetafileFormat : Exception
        {
            public WrongMetafileFormat() { }
        }
        /// <summary>
        /// Возникает, если запрошенная часть не обнаружена у подключенного клиента
        /// </summary>
        class IsNotPart : Exception
        {
            public IsNotPart() { }
        }

        /// <summary>
        /// Возникает, если не удалось установить подключение к серверу
        /// </summary>
        class ServerConnection : Exception
        {
            public ServerConnection() { }
        }
    }
}
