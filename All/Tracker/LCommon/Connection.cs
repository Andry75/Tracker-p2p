using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2P_client
{
    public enum ConDirection
    {
        Download = 0,
        Upload = 1
    }
    [Serializable]
    public struct Connection
    {
        [NonSerialized()] public System.Net.Sockets.Socket Socket;
        [NonSerialized()] public System.Threading.Thread Thread;
    }
}
