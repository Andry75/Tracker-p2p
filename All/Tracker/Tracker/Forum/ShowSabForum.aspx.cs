using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace Tracker.Forum
{
    public partial class ShowSabForum : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string forum_id = "-0";

            forum_id = Request.QueryString["forum"];
            if (forum_id != null)
            {


                TreeView treeForum;
                Conector con = new Conector();
                treeForum = this.TreeView1;

                SqlDataReader SabForum;
                SabForum = con.select("SELECT     id, forum, name, description, position FROM         subforum WHERE     (forum = " + forum_id + ") ORDER BY position DESC");
                while (SabForum.Read())
                {
                    TreeNode t = new TreeNode(SabForum[2].ToString());
                    t.NavigateUrl = "~/Forum/ShowTopic.aspx?forum=" + SabForum[1].ToString() + "&subforum=" + SabForum[0].ToString();
                    treeForum.Nodes.Add(t);
                }
                con.con_close();
            }
            else
                Response.Redirect("~/Default.aspx");
        }
          
    } 
   
}