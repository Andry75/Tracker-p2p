using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;

namespace Tracker.Forum
{
    public partial class Topic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           string id_topic = Request.QueryString["topic"];
           string id_first_post =null;
           if (id_topic != null)
           {
               Conector con = new Conector();
               SqlDataReader First_post_DB = con.select("SELECT     TOP (1) text, id_user , id FROM         post WHERE     (id_topic = " + id_topic + ") ORDER BY id");
               if (First_post_DB.Read())
               {
                   ////////////////////////First Post///////////////////////////////
                   Label User_FP = new Label();
                   User_FP.CssClass = "bold";
                   Guid user_id = new Guid(First_post_DB[1].ToString());
                   MembershipUser myObject = Membership.GetUser(user_id);
                   User_FP.Text = myObject.UserName.ToString() + ":";
                   this.First_post.Controls.Add(User_FP);
                   Literal br = new Literal();
                   br.Text = "<br />";
                   this.First_post.Controls.Add(br);
                   Label Post_F = new Label();
                   Post_F.Text = First_post_DB[0].ToString();
                   Post_F.BorderColor = System.Drawing.Color.Black;
                   this.First_post.Controls.Add(Post_F);
                   id_first_post = First_post_DB[2].ToString();
                   ////////////////////////////////////////////////////////////////
               }
               //////////////////////////Else Posts////////////////////////////////
               Literal br1 = new Literal();
               br1.Text = "<br />";
               this.Posts.Controls.Add(br1);
               this.Posts.Controls.Add(br1);
               List<Label> Posts = new List<Label>();
               SqlDataReader Posts_db = con.select("SELECT     text, id_user FROM         post WHERE     (id_topic = " + id_topic + ") AND (id <> " + id_first_post + ") ORDER BY id");
               while (Posts_db.Read())
               {
                   Label login = new Label();
                   login.CssClass = "bold";
                   Guid user_id = new Guid(Posts_db[1].ToString());
                   MembershipUser myObject = Membership.GetUser(user_id);
                   login.Text = myObject.UserName.ToString() + ":";
                   Posts.Add(login);
                   Label Post = new Label();
                   Post.Text = Posts_db[0].ToString();
                   Post.BorderColor = System.Drawing.Color.Black;
                   Posts.Add(Post);
               }
               int j = 0;
               foreach (Label l in Posts)
               {
                   Literal br2 = new Literal();
                   br2.Text = "<br />";
                   this.Posts.Controls.Add(br2);
                   this.Posts.Controls.Add(l);


               }
               con.con_close();
               ////////////////////////////////////////////////////////////////////
               if (Roles.IsUserInRole("Users") || Roles.IsUserInRole("Admins") && id_topic == null)
               {
                   this.Button1.Enabled = true;
                   
                   this.TextBox1.Enabled = true;
                   this.Button2.Enabled = true;
               }
               else
               {
                   this.Button1.Enabled = false;
                   this.TextBox1.Enabled = false;
                   this.Button2.Enabled = false;
               }

           }
           else
           {
               Response.Redirect("~/Default.aspx");
           }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string id_topic = Request.QueryString["topic"];
            MembershipUser m = Membership.GetUser();
            if (Roles.IsUserInRole("Users")||Roles.IsUserInRole("Admins")&& id_topic==null)
            {
                //Response.Redirect("~/Forum/DownloadMF.aspx?topic="+id_topic);
                Response.Redirect("DownloadMF.aspx?topic=" + id_topic);
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string post = null;
            string id_topic = Request.QueryString["topic"];
            if (TextBox1.Text!=null&&id_topic!=null)
            {
                post = TextBox1.Text;
                 MembershipUser m = Membership.GetUser();
                Conector con = new Conector();
                con.insert("INSERT INTO post (text, id_user, id_topic, id) VALUES     ('"+post+"', '"+m.ProviderUserKey+"', "+id_topic+",(SELECT TOP (1) id FROM post ORDER BY id DESC) + 1)");
                con.con_close();
                TextBox1.Text = null;
                Response.Redirect("~/Forum/Topic.aspx?topic="+id_topic);
            }


        }
    }
}