using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
namespace Tracker
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TreeView treeForum;
            Conector con = new Conector();
            /////////////////////////////////////Forum ID->Server ID////////////////////////////////////            
            MembershipUser m = Membership.GetUser();
            if (m!=null)
            {
                SqlDataReader is_allrady_Reg = con.select("SELECT   TOP (1)  Server_id_user FROM         IDs_Server_and_Forum WHERE     (Forum_id_user = '" + m.ProviderUserKey.ToString() + "')");
                if (!is_allrady_Reg.Read())
                {
                    int id_server;
                    SqlDataReader id_server_new = con.select("SELECT   TOP (1)  Forum_id_user FROM         IDs_Server_and_Forum ORDER BY Forum_id_user DESC");
                    if (id_server_new.Read())
                    {
                        id_server = Convert.ToInt32(id_server_new[0].ToString());
                    }
                    else
                        id_server = 0;
                    con.insert("INSERT INTO IDs_Server_and_Forum  (Server_id_user, Forum_id_user) VALUES     (" + id_server + ",'" + m.ProviderUserKey.ToString() + "')");
                    con.insert("INSERT INTO Server_info  (ID_Client) VALUES     (" + id_server + ")");
                }
            }
            
            ////////////////////////////////////////////////////////////////////////////////////////////
            SqlDataReader Forum;
            SqlDataReader SabForum;
           
            int i = 0;
            Forum = con.select("SELECT id, name FROM forum");
            treeForum = this.TreeView1;
            ContentPlaceHolder menu = (ContentPlaceHolder)Master.FindControl("menu");
            Menu men = (Menu)menu.FindControl("NavigationMenu");
            if (Roles.IsUserInRole("Users") || Roles.IsUserInRole("Admins"))
            {
                MenuItem it = new MenuItem("Создать раздачю");
                it.NavigateUrl= "~/Forum/Create_topic.aspx";
                men.Items.Add(it);
            }
            
            
            while (Forum.Read())
	        {

                
                TreeNode t =new TreeNode(Forum[1].ToString());
                t.NavigateUrl="~/Forum/ShowForum.aspx?forum="+Forum[0].ToString();
                treeForum.Nodes.Add(t);
                SabForum = con.select("SELECT     name, id FROM         subforum WHERE     (forum = "+Forum[0].ToString()+")");
			     while(SabForum.Read())
                 {
                     
                    TreeNode t1 = new TreeNode(SabForum[0].ToString());
                     t1.NavigateUrl="~/Forum/ShowSabForum.aspx?forum="+Forum[0].ToString()+"&subforum="+SabForum[1].ToString();
                    treeForum.Nodes[i].ChildNodes.Add(t1);
                    
			    }
                 i++;
			}

            con.con_close();
            
        }
    }
}
