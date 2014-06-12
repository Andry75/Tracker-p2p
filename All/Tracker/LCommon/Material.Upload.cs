using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using bc = System.BitConverter;
namespace P2P_client
{
    public partial class Material
    {
        List<Connection> connections = new List<Connection>();

        public int ConnectionsNum
        {
            get { return connections.Count; }
        }
        public List<Connection> Connections
        {
            get { return connections; }
        }


        public void UploadThreadStart(ref Socket socket)
        {
            Connection con = new Connection();
            con.Socket = (Socket)socket;
            con.Thread = new System.Threading.Thread(Upload);
            con.Thread.IsBackground = true;
            con.Thread.Start(con);
           
            

        }
        void Upload(object connection)
        {
            Connection con = (Connection)connection;
            
            try
            {
                lock (connections)
                    connections.Add(con);

                byte[] buff = new byte[4];
            session:
                con.Socket.Receive(buff, 1, SocketFlags.None);
                if (buff[0] != (byte)C_C_Protocol.GetPart) { buff[0] = (byte)C_C_Protocol.EndConnection; con.Socket.Send(buff, 1, SocketFlags.None); return; }
                con.Socket.Receive(buff, 4, SocketFlags.None);
                int material = bc.ToInt32(buff, 0);
                con.Socket.Receive(buff, 4, SocketFlags.None);
                int file = bc.ToInt32(buff, 0);
                con.Socket.Receive(buff, 4, SocketFlags.None);
                int part = bc.ToInt32(buff, 0);

                buff[0] = (byte)C_C_Protocol.SendPart;
               con.Socket.Send(buff, 1, SocketFlags.None);

                if (part >= Files[file].Count_Parts_) 
                    { buff[0] = 0; con.Socket.Send(buff, 1, SocketFlags.None); goto session; }
                if (Files[file].IsPart(part))
                {
                    

                    buff[0] = 1;
                    con.Socket.Send(buff, 1, SocketFlags.None);
                    con.Socket.Send(bc.GetBytes(material), SocketFlags.None);
                    con.Socket.Send(bc.GetBytes(file), SocketFlags.None);
                    con.Socket.Send(bc.GetBytes(part), SocketFlags.None);
                    byte[] buffer = Files[file].Get_Part(part);
                    Stat_Upload_ += buffer.Length;


                    NetworkStream nStream = new NetworkStream(con.Socket);
                    DateTime start_sending = DateTime.Now;
                    nStream.Write(buffer, 0, buffer.Length);
                    DateTime end_sending = DateTime.Now;
                    speed_Upload = (double)buffer.Length / (double)(end_sending - start_sending).Seconds;
                    nStream.Close();
                    
                }
                else
                {
                    buff[0] = 0;
                    con.Socket.Send(buff, 1, SocketFlags.None);
                }
                goto session;
            }
            catch (Exception)
            {
                lock (connections)
                {
                    con.Socket.Close();
                    connections.Remove(con);
                    Config.Connections--;
                }
            }
        }
    }
}
