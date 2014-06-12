using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.IO;

namespace Tracker.Forum
{
    public partial class Create_topic : System.Web.UI.Page
    {
        string selected_forum_id = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole("Users") || Roles.IsUserInRole("Admins") )
            {
                //this.DropDownList1.Items.Clear();
                Conector con = new Conector();
                SqlDataReader forum = con.select("SELECT     name  FROM         forum");
                while (forum.Read())
                {
                    this.DropDownList1.Items.Add(forum[0].ToString());
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole("Users") || Roles.IsUserInRole("Admins")||this.DropDownList2.SelectedValue!=null||this.TextBox1.Text!=null||this.TextBox2.Text!=null||this.FileUpload1.HasFile)
            {
                selected_forum_id = this.DropDownList1.SelectedValue;
                Conector con = new Conector();
                SqlDataReader id_subforum = con.select("SELECT     id FROM         subforum WHERE     (name = N'"+this.DropDownList2.SelectedValue+"')");
                id_subforum.Read();
                SqlDataReader id_topic_DB = con.select("SELECT     TOP (1) id FROM         topic ORDER BY id DESC");
                id_topic_DB.Read();
                int id_new_topic = Convert.ToInt32(id_topic_DB[0].ToString());
                id_new_topic++;
                SqlDataReader new_id_post = con.select("SELECT     TOP (1) id FROM         post ORDER BY id DESC");
                new_id_post.Read();
                int id_post_new = Convert.ToInt32(new_id_post[0].ToString());
                id_post_new++;
                MembershipUser m = Membership.GetUser();
                con.insert("INSERT      INTO            post(id, text, id_topic, id_user) VALUES     ("+id_post_new+", N'"+this.TextBox2.Text+"', "+id_new_topic+",'"+m.ProviderUserKey.ToString()+"')");
                con.insert("INSERT INTO topic  (id, subforum, name, mess, id_user) VALUES     ("+id_new_topic+", "+id_subforum[0].ToString()+", N'"+this.TextBox1.Text+"', "+id_post_new+",'"+m.ProviderUserKey.ToString()+"')");
                con.insert("INSERT INTO materials  (Id_material) VALUES     ("+id_post_new+")");

                Directory.SetCurrentDirectory(Request.PhysicalApplicationPath);
                this.FileUpload1.SaveAs(Request.PhysicalApplicationPath+"/MetaFiles/"+id_new_topic.ToString()+".mf");
                con.con_close();
                Response.Redirect("~/Forum/Topic.aspx?topic="+id_new_topic);

            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole("Users") || Roles.IsUserInRole("Admins"))
            {
                this.DropDownList2.Items.Clear();
                Conector con = new Conector();
                SqlDataReader forum_id = con.select("SELECT     id FROM         forum WHERE     (name = N'"+this.DropDownList1.SelectedItem.Text+"')");
                forum_id.Read();
                selected_forum_id = forum_id[0].ToString();
                SqlDataReader sub_forum = con.select("SELECT     name FROM         subforum WHERE     (forum = "+selected_forum_id+")");
                while (sub_forum.Read())
                {
                    this.DropDownList2.Items.Add(sub_forum[0].ToString());
                }
            }

        }
    }
}