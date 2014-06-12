using System;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ASP.NET_Forum.Pages
{
    public partial class NewReply : System.Web.UI.Page
    {
        OleDbCommand com;
        string login;
        int id_topic;
        protected void Page_Load(object sender, EventArgs e)
        {
            tbText.Visible = true;
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
                    page.InnerHtml += "<a href=\"login.aspx?action=exit\">Выход</a><br />";
                    try
                    {
                        id_topic = Convert.ToInt32(Request.QueryString["topic"]);
                        com.CommandText = "select name from topic where id = " + id_topic;
                        string s = com.ExecuteScalar().ToString();
                    }
                    catch (NullReferenceException)
                    {
                        err.InnerHtml = "Неверный идентификатор темы.";
                        tbText.Visible = false;
                        return;
                    }
                }

            }
            catch (NullReferenceException)
            {
                Response.Redirect("login.aspx?redirect=" + Request.RawUrl);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (tbText.Text.Length < 10)
            {
                err.InnerHtml = "Введите хотя бы 10 символов сообщения";
                return;
            }
            com.CommandText = "select id from [user] where name ='" + login + "';";
            string us_is = com.ExecuteScalar().ToString();

            com.CommandText = "insert into post([text],id_topic,id_user) values('" +
                tbText.Text + "'," + id_topic + "," + us_is + ");";
            com.ExecuteNonQuery();

            Response.Redirect("showtopic.aspx?topicId=" + id_topic);
        }
    }
}
