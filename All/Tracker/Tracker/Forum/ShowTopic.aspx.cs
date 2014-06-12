using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;

namespace Tracker.Forum
{
    public partial class ShowTopic : System.Web.UI.Page
    {
         string forum_id = "-0";
         string subforum_id = "-0";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Guid user = new Guid("533cd376-7dba-47a8-a509-1ea25609c9f8");
            //MembershipUser myObject = Membership.GetUser(user);
            //string UserID = myObject.ProviderUserKey.ToString();
            forum_id = Request.QueryString["forum"];
            subforum_id = Request.QueryString["subforum"];
            if (forum_id != null && subforum_id != null)
            {

                Conector con = new Conector();
            TreeView treeForum;
            
            treeForum = this.TreeView1;

            int i = 0;
            
            SqlDataReader topics = con.select("SELECT     id, name, mess, id_user FROM         topic WHERE     (subforum = "+subforum_id+")");
           
            while (topics.Read())
            {
                TreeNode t = new TreeNode(topics[1].ToString());
                t.NavigateUrl = "~/Forum/Topic.aspx?topic=" + topics[0].ToString();
                treeForum.Nodes.Add(t);
                //dr["Название"] = "<a href = '"+name.NavigateUrl.ToString()+"'>"+name.Text+"</a>";
               
                SqlDataReader LastMessage = con.select("SELECT     TOP (1) text FROM         post WHERE     (id_topic = " + topics[0].ToString() + ") ORDER BY id DESC");
                LastMessage.Read();
                TreeNode t1 =new TreeNode(LastMessage[0].ToString());
                
               treeForum.Nodes[i].ChildNodes.Add(t1);
                Guid user_id = new Guid(topics[3].ToString());
                MembershipUser myObject = Membership.GetUser(user_id);
                TreeNode t2 = new TreeNode( myObject.UserName.ToString());
             
                treeForum.Nodes[i].ChildNodes.Add(t2);
                i++;
            }
           
            }
            else
                Response.Redirect("~/Default.aspx");
            //Page.DataBind();
        }
      
    }
}