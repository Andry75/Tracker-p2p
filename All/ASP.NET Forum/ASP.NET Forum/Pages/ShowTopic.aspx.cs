using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
    public partial class ShowTopic : System.Web.UI.Page
    {
        OleDbCommand com;
        protected void Page_Load(object sender, EventArgs e)
        {
            com = new OleDbCommand();
            com.Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"C:\\forum.mdb\"");

            com.Connection.Open();
            try
            {
                string login = Request.Cookies["login"].Value;
                string pass = Request.Cookies["pass"].Value;
                if ((login == null || pass == null) || (!Authentication.CheckLogin(com, login, pass)))
                {
                    page.InnerHtml += "<a href=\"login.aspx\">Вход</a>\n";
                    page.InnerHtml += " <a href=\"registration.aspx\">Регистрация</a>\n";
                    Request.Cookies.Clear();
                }
                else
                {
                    page.InnerHtml = "Приветствуем Вас, " + login + ".<br />";
                    page.InnerHtml += "<a href=\"login.aspx?action=exit\">Выход</a><br />";

                }

            }
            catch (NullReferenceException)
            {
                page.InnerHtml += "<a href=\"login.aspx\">Вход</a>\n";
                page.InnerHtml += " <a href=\"registration.aspx\">Регистрация</a>\n";
                Request.Cookies.Clear();
            }
            string topicId;
            try
            {
                topicId = Request.QueryString["topicId"];
            }
            catch (NullReferenceException) { Response.Redirect("main.aspx"); return; }

            com.CommandText = "select [name],[text] from post,[user] where(id_user=user.id and id_topic=" +
                topicId + ") order by post.id";
            OleDbDataReader dReader = com.ExecuteReader();
            
            while (dReader.Read())
            {
                string user = dReader.GetString(0);
                string text = dReader.GetString(1);
                GetPosts(user,text);
            }
            
            
            page.InnerHtml += "<a href = \"newreply.aspx?topic=" + topicId + "\">Ответить</a><br>\n";
        }

        void GetPosts(string user, string text)
        {
            page.InnerHtml += "<table class=\"fstr\">\n";
            page.InnerHtml += "<tr class=\"fstr\"><td class=\"fstr\">" + user + "</td></tr>\n" +
                "<tr class=\"fstr\"><td class=\"fstr\"td>" + text +
                "</td></tr>\n";
            page.InnerHtml += "</table>\n";
            page.InnerHtml += "<br>\n";
        }
    }
}
