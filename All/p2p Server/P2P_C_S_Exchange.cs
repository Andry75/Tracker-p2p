using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using bc = System.BitConverter;
using System.Data.OleDb;
using System.Net;
using System.Data.SqlClient;

namespace p2p_Server
{
    class P2P_C_S_Exchange
    {
        private Socket socket;
        private NetworkStream nc;
        private int id_cl;
        public P2P_C_S_Exchange(ref Socket socket_)
        {
             socket = socket_;
             nc = new NetworkStream(socket);
        }
      public void Start()
        {
            bool Session = true;

            
            while (Session)
            {
                try
                {
                    byte[] comand = new byte[1];
                    try
                    {
                        socket.Receive(comand);



                        switch ((CS_Protocol)comand[0])
                        {
                            case CS_Protocol.Regiser:
                                Registration();
                                break;
                            case CS_Protocol.GetClients:
                                GetClients();
                                break;
                            case CS_Protocol.EndTrasaction:
                                 byte[] b = new byte[1];
                                 b[0] = (byte)CS_Protocol.EndTrasactionAccepted;
                                  socket.Send(b, 1, SocketFlags.None);
                                //nc.WriteByte((byte)CS_Protocol.EndTrasactionAccepted);
                                Session = false;
                                break;
                            case CS_Protocol.UpdateInfo:
                                Update_info();
                                break;



                            default:
                                Session = false;
                                break;

                        }
                    }
                    catch (SocketException) { return; }
                }
                
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e.ToString());
                    return;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.ToString());
                    return;
                }
            }
        }

      private void Update_info()
      {
          byte[] id_mat_encript = new byte[256];
          byte[] upload_encript = new byte[256];
          byte[] download_encript = new byte[256];
          socket.Receive(id_mat_encript);
          socket.Receive(upload_encript);
          socket.Receive(download_encript);
          //nc.Read(id_mat_encript, 0, 256);
          //nc.Read(upload_encript, 0, 256);
          //nc.Read(download_encript, 0, 256);
          long id_mat, upload, download;
          id_mat = Encript.Decoding(id_mat_encript);
          upload = Encript.Decoding(upload_encript);
          download = Encript.Decoding(download_encript);
          Conector con = new Conector();
          SqlDataReader is_available_mat = null;
          Console.WriteLine("Information were Encripted");
          is_available_mat = con.select("SELECT Id_material FROM     materials WHERE  (Id_material = " + id_mat + ")");
          if (is_available_mat.Read())
          {
              byte[] b = new byte[1];
              b[0] = (byte)CS_Protocol.UpdateInfoFeedback;
              socket.Send(b, 1, SocketFlags.None);
              //nc.WriteByte((byte)CS_Protocol.UpdateInfoFeedback);
              b[0] = (byte)Results_of_operations.Update_Info_Feedback_OK;
              socket.Send(b, 1, SocketFlags.None);
              //nc.WriteByte((byte)Results_of_operations.Update_Info_Feedback_OK);
              SqlDataReader down_up = con.select("SELECT Upload, Download FROM     Server_info WHERE  (ID_Client = " + id_cl + ")");
              if (down_up.Read())
              {
                  long temp_down = down_up.GetInt32(1);
                  long temp_up = down_up.GetInt32(0);
                  download += temp_down;
                  upload+=temp_up;
              }
              con.update("UPDATE Server_info SET  Download = "+download+", Upload =  "+upload+" WHERE  (Server_info.ID_Client = "+id_cl+")");
              con.con_close();
              Console.WriteLine("Information were update");
              //nc.WriteByte((byte)CS_Protocol.UpdateInfoFeedback);
              //nc.WriteByte((byte)Results_of_operations.Update_Info_Feedback_OK);
          }
          else
          {
              nc.WriteByte((byte)CS_Protocol.UpdateInfoFeedback);
              nc.WriteByte((byte)Results_of_operations.Update_Info_Feedback_Bad_informatio);

          }
      }

      private void GetClients()
      {
          byte[] id_mat = new byte[4];
          socket.Receive(id_mat);
          
          Conector con = new Conector();
          SqlDataReader is_available_mat = null;
          is_available_mat = con.select("SELECT Id_material FROM     materials WHERE  (Id_material = "+bc.ToInt32(id_mat,0)+")");
          if (is_available_mat.Read())
          {
              SqlDataReader is_available_mat_online = null;
              is_available_mat_online = con.select("SELECT ID_material_online FROM     material_online WHERE  (ID_material_online = "+ bc.ToInt32(id_mat, 0) +") AND (Id_client = "+ id_cl +")"
                  );
              if (is_available_mat_online.Read())
              {

              }
              else
              {
                  con.insert("INSERT INTO material_online (ID_material_online, Id_client) VALUES (" + bc.ToInt32(id_mat, 0) + ", " + id_cl + ")");
              }
              //nc.WriteByte((byte)CS_Protocol.ReceiveClients);
              byte[] b = new byte[1];
              b[0] = (byte)CS_Protocol.ReceiveClients;
              socket.Send(b, 1, SocketFlags.None);
              //nc.Write(id_mat, 0, 4);
              socket.Send(id_mat, 4, SocketFlags.None);
              ReceiveClients(bc.ToInt32(id_mat, 0));
              con.con_close();
              Console.WriteLine("Clients was got");
          }
          else
          {
              byte[] b = new byte[1];
              b[0] = 0;
              socket.Send(b, 1, SocketFlags.None);
          }
          

      }

      private void ReceiveClients(int id_mat)
      {
          int count;
          
          Conector con = new Conector();
          SqlDataReader select_count = null;
          select_count = con.select("SELECT count(*) FROM     material_online WHERE  (ID_material_online = " + id_mat + ") and (material_online.Id_client <>" + id_cl + ")");
          if (select_count.Read())
          {
              count = Convert.ToInt32(select_count[0].ToString());
              //nc.Write(bc.GetBytes(count), 0, 4);
              socket.Send(bc.GetBytes(count), 4, SocketFlags.None);

              select_count = con.select("SELECT Server_info.IP_client, Server_info.Port FROM   Server_info,  material_online   WHERE   (material_online.Id_client = Server_info.ID_Client and material_online.ID_material_online = " + id_mat + " and material_online.Id_client <>"+id_cl+" )");
              while (select_count.Read())
              {
                  int i = select_count.GetInt32(0);
                  byte[] b = bc.GetBytes(i);
                  //nc.Write(b, 0, 4);
                  socket.Send(b, 4, SocketFlags.None);
                  int tempport = select_count.GetInt32(1);
                  short port = Convert.ToInt16(tempport);
                  byte[] tarray = bc.GetBytes(port);
                  //nc.Write(tarray, 0, 2);
                  socket.Send(tarray, 2, SocketFlags.None);
                  //socket.Receive(null, 2, SocketFlags.None);
              }
              Console.WriteLine("Clients are goting");
          }
          
      }
      private void  Registration ()
        {
            try
            {

                byte[] reg = new byte[6];
                //nc.Read(reg, 0, 6);
                socket.Receive(reg);
                int Id_client = bc.ToInt32(reg, 0);
                short port = bc.ToInt16(reg, 4);
                if (Id_client == 0 || port == 0)
                {

                    //nc.WriteByte(2);
                    byte[] b = new byte[1];
                    b[0] = 2;
                    socket.Send(b, 1, SocketFlags.None);
                }
                else
                {
                    byte IsReg = RegistrationDB(Id_client, port);
                    if (IsReg == 0)
                    {
                        byte[] b = new byte[1];
                        b[0] = (byte)CS_Protocol.RegisterFeedback;
                        socket.Send(b, 1, SocketFlags.None);
                        //nc.WriteByte((byte)CS_Protocol.RegisterFeedback);
                        b[0] = (byte)Results_of_operations.Rgistration_Feedback_OK;
                        socket.Send(b, 1, SocketFlags.None);
                        //nc.WriteByte((byte)Results_of_operations.Rgistration_Feedback_OK);
                        id_cl = Id_client;
                    }
                    else
                    {
                        if (IsReg == 1)
                        {
                            byte[] b = new byte[1];
                            //nc.WriteByte((byte)CS_Protocol.RegisterFeedback);
                            b[0] = (byte)CS_Protocol.RegisterFeedback;
                            socket.Send(b, 1, SocketFlags.None);
                            //nc.WriteByte((byte)Results_of_operations.Rgistration_Feedback_Client_not_found);
                            b[0] = (byte)Results_of_operations.Rgistration_Feedback_Client_not_found;
                            socket.Send(b, 1, SocketFlags.None);
                        }
                        else
                        {
                            nc.WriteByte((byte)CS_Protocol.RegisterFeedback);
                            nc.WriteByte((byte)Results_of_operations.Rgistration_Feedback_Server_Error);
                        }
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
        }

      public byte RegistrationDB(int Id_client, short port)
        {
            Conector con = new Conector();
            SqlDataReader is_available_client = null;
            is_available_client = con.select("SELECT ID_Client FROM     Server_info WHERE  (ID_Client = "+Id_client+")");
            if (is_available_client.Read())
            {
                IPEndPoint clientep;
                
                clientep = (IPEndPoint)socket.RemoteEndPoint;
                Client clc = new Client(clientep.Address,port);

                int ip = bc.ToInt32(clc.Serialize(),0);

                string strConnect = "Connected with " +  clientep.Address+":"+port ;
                Console.WriteLine(strConnect);
                Console.WriteLine("Registration is successful");
                con.update("UPDATE Server_info SET IsOnline =" + 1 + ", Date_time_last_update ='" + DateTime.Today.Ticks + "', Port = " + port + ", IP_client = "+ip+" WHERE  (Server_info.ID_Client = " + Id_client + ")");
                con.con_close();
                return (byte)Results_of_operations.Rgistration_Feedback_OK;
                
            }

            else
            {
                con.con_close();
                return (byte)Results_of_operations.Rgistration_Feedback_Client_not_found;
            }

            
            
        }
    }
}
