using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;


namespace p2p_Server
{
    class connect_info
    {
        public Socket Soket_;
        public Thread th;
    }
}
