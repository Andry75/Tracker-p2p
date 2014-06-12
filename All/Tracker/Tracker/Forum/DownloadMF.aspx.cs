using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.IO;
using P2P_client;

namespace Tracker.Forum
{
    public partial class DownloadMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Response.Clear();
             string topic = Request.QueryString["topic"];
            if (topic!=null)
        	    {   string filename;

                    int Server_user_id;
                    int Server_IP;
                    short Server_PORT;
                    int Track=Convert.ToInt32(topic);
                    Conector con = new Conector();
                    MembershipUser m = Membership.GetUser();
                    SqlDataReader server_user_id_db = con.select("SELECT    Top (1)  Server_id_user FROM         IDs_Server_and_Forum WHERE     (Forum_id_user = '"+m.ProviderUserKey.ToString()+"')");
                    server_user_id_db.Read();
                    Server_user_id=Convert.ToInt32(server_user_id_db[0].ToString());
                    SqlDataReader Server_configs = con.select("SELECT     TOP (1) IP_Server, Port_Server FROM         Servers_Tracker");
                    Server_configs.Read();
                    Server_IP = Convert.ToInt32(Server_configs[0].ToString());
                    Server_PORT = Convert.ToInt16(Server_configs[1].ToString());
                    SqlDataReader name_track = con.select("SELECT  TOP(1)   name FROM         topic WHERE     (id = "+Track+")");
                    name_track.Read();
                    filename = name_track[0].ToString();
                    con.con_close();

                    byte[] ipport = new byte[6];

                    byte[] ipbytes = BitConverter.GetBytes(Server_IP);
                    for (int i = 0; i < 4; i++)
			        {
			            ipport [i] = ipbytes[i];
			        }
                    byte[] portbytes = BitConverter.GetBytes(Server_PORT);
                    ipport [4] = portbytes[0];   ipport [5] = portbytes[1];
                    

                    FileStream fs = new FileStream (Request.PhysicalApplicationPath+"/MetaFiles/"+Track+".mf",FileMode.Open,FileAccess.Read);
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter  bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    Meta_File mf = (Meta_File) bf.Deserialize(fs);
                    
                    mf.IP_Port_Server = new Client();
                    mf.IP_Port_Server.Deserialize(ipport);

                    mf.ID_Client = Server_user_id;
                    mf.ID_matirial = Track;


                filename = Request.PhysicalApplicationPath+"Temp\\" + mf.name+".mf";
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                
                bf.Serialize(fs,mf);
                fs.Close();
               
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + mf.name + ".mf");
               
                Response.WriteFile(filename);
                Response.Flush();
               //Response.Redirect("~/Temp/"+mf.name+".mf");
                System.IO.File.Delete(filename);
                }
            else
                Response.Redirect("~/Default.aspx");
;        }
    }
}