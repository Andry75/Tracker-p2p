using System;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ASP.NET_Forum.Pages
{
    public partial class NewTopic : System.Web.UI.Page
    {
        OleDbCommand com;
        string login;
        int id_forum;

        protected void Page_Load(object sender, EventArgs e)
        {
            com = new OleDbCommand();
            com.Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"C:\\forum.mdb\"");
            com.Connection.Open();
            try
            {
                login = Request.Cookies["login"].Value;
                string pass = Request.Cookies["pass"].Value;
                if ((login == null || pass == null) || (!Authentication.CheckLogin(com, login, pass)))
                {
                    Response.Redirect("login.aspx?redirect=" + Request.RawUrl);
                }
                else
                {
                    page.InnerHtml = "Приветствуем Вас, " + login + ".<br />";
                    page.InnerHtml+= "<a href=\"login.aspx?action=exit\">Выход</a><br />";
                    try
                    {
                        id_forum = Convert.ToInt32(Request.QueryString["forumId"]);
                        com.CommandText = "select name from subforum where id = " + id_forum;
                        string s = com.ExecuteScalar().ToString();
                    }
                    catch (NullReferenceException)
                    {
                        err.InnerHtml = "Неверный идентификатор форума.";
                        return;
                    }
                }

            }
            catch (NullReferenceException)
            {
                Response.Redirect("login.aspx?redirect="+Request.RawUrl);
            }
        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            if (tbTopicName.Text.Length == 0)
            {
                err.InnerHtml = "Введите название темы";
                return;
            }
            if (tbMessText.Text.Length < 10)
            {
                err.InnerHtml = "Введите, по крайней мере, 10 символов сообщения.";
                return;
            }
            com.CommandText="select id from [user] where name ='"+ login+"';";
            int id_user = (int)com.ExecuteScalar();

            com.CommandText = @"insert into topic(subforum,name,mess,id_user)
                values(" + id_forum + ",'" + tbTopicName.Text + "',0," + id_user + ");";
            com.ExecuteNonQuery();

            com.CommandText="select id from topic where name ='"+tbTopicName.Text+"';";

            string id = com.ExecuteScalar().ToString();

            com.CommandText ="insert into post ([text],id_topic,id_user)"+
                " values('"+tbMessText.Text+"',"+id+","+id_user+");";
            com.ExecuteNonQuery();

            Response.Redirect("ShowTopic.aspx?topicId=" + id);
        }
    }
}
