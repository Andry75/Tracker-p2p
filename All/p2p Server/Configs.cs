using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using bc = System.BitConverter;
namespace p2p_Server
{
    
    class Configs
    {
        public string ip;
        public short port;
        public Configs(string IP, short Port)
        {
            IPAddress t = new IPAddress(new byte[4]);
            if (!IPAddress.TryParse(IP,out t))
                throw new ArgumentException("Wrong IP adress format.");
            ip = IP;
            port = Port;
        }
        public Configs()
        {
            
                StreamReader sr = new StreamReader("Configs/Server.conf");
                ip = sr.ReadLine();
                port = Convert.ToInt16(sr.ReadLine());
                sr.Close();
            
        }
        public void Write()
        {
            Directory.CreateDirectory("Config");
            StreamWriter sw = new StreamWriter("Configs/Server.conf");
            sw.WriteLine(ip);
            sw.WriteLine(port);
            sw.Close();
        }

    }

}
