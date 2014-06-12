using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace p2p_Server
{
    
    class Program
    {
       static Configs x = new Configs();
       static Socket sock;
       static bool exit = false;
       static List<connect_info> con = new List<connect_info>();
       static Thread sockqth = new Thread(Socket_Accept);
       static Thread control_ = new Thread(Control);
        static void Main(string[] args)
        {        
            
           
           
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            sock.Bind(new IPEndPoint(IPAddress.Any, x.port));
            sock.Listen((int)SocketOptionName.MaxConnections);

            sockqth.IsBackground = true;
            control_.IsBackground = true;
            control_.Start();
           

            while (!exit)
            {
                
            }
           
        }
        static void Control()
        {
            bool IsOn = true;
            string com;
            while (IsOn)
            {
                com = Console.ReadLine();
                com = com.ToLower();
                switch (com)
                {
                    case "shoutdown":
                        IsOn = false;
                        exit = true;
                        com = null;
                        break;
                    case "server -start":
                        try
                        {
                            sockqth.Start();
                        }
                        catch (ThreadStateException e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;
                    case "server -stop":
                        try
                        {
                            sockqth.Abort();
                        }
                        catch (ThreadStateException e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;
                    case "socket -pause":
                                    try
                                    {
                                        sockqth.Suspend();
                                    }
                                    catch (ThreadStateException e)
                                    {
                                        Console.WriteLine(e.ToString());
                                    }
                        break;
                    case "socket -start":
                        try
                        {
                            sockqth.Resume();
                            
                        }
                        catch (ThreadStateException e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    break;
                    case "config":
                    conf_create();
                    break;
                    default:
                        break;
                }
            }
        }
        private static void Socket_Accept()
        {

            while (true)
            {


                connect_info swe = new connect_info();
                swe.Soket_ = sock.Accept();
                swe.th = new Thread(ProcConn);
                swe.th.IsBackground = true;
                swe.th.Start(swe);
                lock (con)
                {
                    con.Add(swe);
                }
            }
           
        }
        public static void ProcConn(object conobj) 
        {
            connect_info c = (connect_info) conobj;
            P2P_C_S_Exchange p2p = new P2P_C_S_Exchange(ref c.Soket_);
            
            p2p.Start();
            
            Console.WriteLine("out");
            lock (con) con.Remove(c);
        }
        static void conf_create()
        {
            Console.WriteLine("Enter IP address of Data Base Server:");
            string tmp=Console.ReadLine();
            Console.WriteLine("Enter Port of Server:");
            short port = Convert.ToInt16(Console.ReadLine());
            Configs d = new Configs(tmp, port);
            d.Write();
            
        }
    }
}
