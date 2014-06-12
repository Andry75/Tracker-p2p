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
using System.Xml.Linq;

namespace ASP.NET_Forum
{
    public partial class ForumDisplay : System.Web.UI.Page
    {
        OleDbCommand com;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            string forumId= Request.QueryString["forumId"];
            string type = Request.QueryString["type"];

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
            if (type == "subforum")
            {
                page.InnerHtml += "<br><a href=\"newtopic.aspx?forumId=" + forumId + "\">Новая тема</a><br>";
                page.InnerHtml += "<table class=\"fstr\">\n";
            }


            if (type == "forum")
                GetSubForums(forumId);
            else if (type == "subforum")
                GetTopics(forumId);
            else
                Response.Redirect("/Index.aspx");
            

        }

        

        private void GetSubForums(string forumId)
        {
            page.InnerHtml += "<table class=\"fstr\">\n";


            com.CommandText =
                "select id,name,description from subforum where(forum=" + forumId + ") order by [position];";

            OleDbDataReader dReader = com.ExecuteReader();
            List<ForumItem> list = new List<ForumItem>();

            while (dReader.Read())
            {
                ForumItem fi = new ForumItem();
                fi.id = dReader.GetInt32(0);
                fi.name = dReader.GetString(1);
                try
                {
                    fi.description = dReader.GetString(2);
                }
                catch (InvalidCastException) { fi.description = ""; };
                list.Add(fi);
                
            }
            for (int i = 0; i < list.Count; i++)
            {
                page.InnerHtml += "<tr class=\"fstr\">";
                page.InnerHtml += SubForumObjectParser(list[i].name, list[i].id, list[i].description);
                page.InnerHtml += "</tr>";
                
            }
            page.InnerHtml += "</table>\n";

        }
        private void GetTopics(string forumId)
        {



            com.CommandText =
                "select id,name,mess from topic where(subforum=" + forumId + ");";

            OleDbDataReader dReader = com.ExecuteReader();
            List<TopicItem> list = new List<TopicItem>();

            while (dReader.Read())
            {
                TopicItem ti = new TopicItem();
                ti.id = dReader.GetInt32(0);
                ti.name = dReader.GetString(1);
                ti.mess = dReader.GetInt32(2);
                list.Add(ti);
                
            }
            for (int i = 0; i < list.Count; i++)
            {
                page.InnerHtml += "<tr class=\"fstr\">";
                page.InnerHtml += TopicObjectParser(list[i].name, list[i].id, list[i].mess);
                page.InnerHtml += "</tr>";
            }
            if (list.Count == 0)
            {

                page.InnerHtml += "<tr><td><div align=\"center\">В данном разделе пока нет тем</div></td></tr>";
            }
            page.InnerHtml += "</table>\n";
        }

        private string TopicObjectParser(string Text, int id, int mess)
        {
            string temp =
             "\n<!-- Блок форума №" + id + ".-->" +
             "\n\t<td width=\"70%\" class=\"fstr\">" +
             "\n\t\t\t\t<a href=\"ShowTopic.aspx?topicId=" + id + "\">" + Text + "</a>" +
             "\n\t\t\t\t<div style=\"font-size:smaller; color:Gray\">Сообщений:" + mess + "</div>" +
             "\n\t\t</td>" +
             "\n\t<td class=\"fstr\"></td>\n";
            return temp;
        }

        private string SubForumObjectParser(string Text, int forumId, string description)
        {
            string temp =
             "\n<!-- Блок форума №" + forumId + ".-->" +
             "\n\t<td width=\"70%\" class=\"fstr\">" +
             "\n\t\t\t<a href=\"ForumDisplay.aspx?forumId=" + forumId + "&type=subforum\">" + Text + "</a>" +
             "\n\t\t\t<div style=\"font-size:smaller; color:Gray\">" + description + "</div>" +
             "\n\t</td>" +
             "\n\t\t<td class=\"fstr\"></td>\n";
            return temp;
        }
    }
}
