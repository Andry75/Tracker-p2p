using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Security.Cryptography;

namespace ASP.NET_Forum
{
    public partial class MainPage : System.Web.UI.Page
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
                    page.InnerHtml+= "<a href=\"login.aspx?action=exit\">Выход</a><br />";
                }

            }
            catch (NullReferenceException)
            {
                page.InnerHtml += "<a href=\"login.aspx\">Вход</a>\n";
                page.InnerHtml += " <a href=\"registration.aspx\">Регистрация</a>\n";
            }
            finally
            {
                GetForums();
            }
        }
        private void GetForums()
        {
            page.InnerHtml += "<table class=\"fstr\">\n";


            com.CommandText = "select id,name,description from forum order by [position]";
            OleDbDataReader dReader = com.ExecuteReader();

            List<ForumItem> list = new List<ForumItem>();

            while (dReader.Read())
            {
                ForumItem fi = new ForumItem();
                fi.id = dReader.GetInt32(0);
                fi.name = dReader.GetString(1);
                fi.description = dReader.GetString(2);

                list.Add(fi);
            }

            for (int i = 0; i < list.Count; i++)
            {
                page.InnerHtml += "<tr class=\"fstr\">";
                page.InnerHtml += ForumObjectParser(list[i].name, list[i].id, list[i].description);
                page.InnerHtml += "</tr>";
            }


            page.InnerHtml += "</table>\n";

        }



        private string ForumObjectParser(string Text, int forumId,string description)
        {
            string temp =
             "\n<!-- Блок форума №" + forumId + ".-->" +
             "\n\t<td width=\"70%\" class=\"fstr\">" +
             "\n\t\t\t<a href=\"ForumDisplay.aspx?forumId=" + forumId + "&type=forum\">" + Text + "</a>" +
             "\n\t\t\t\t<div style=\"font-size:smaller; color:Gray\">" + description + "</div>" +
             "\n\t</td>" +
             "\n\t<td class=\"fstr\"></td>\n";
            return temp;
        }
    }
}
