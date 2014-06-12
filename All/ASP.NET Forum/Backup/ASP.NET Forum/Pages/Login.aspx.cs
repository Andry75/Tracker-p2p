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
using System.Text;
using System.Security.Cryptography;

namespace ASP.NET_Forum.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        OleDbCommand com;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["action"] == "exit")
                {
                HttpCookie log = new HttpCookie("login");
                log.Expires = DateTime.Now.AddDays(-1);
                
                HttpCookie pas = new HttpCookie("pass");
                pas.Expires = DateTime.Now.AddDays(-1);

                Response.Cookies.Add(log);
                Response.Cookies.Add(pas);
                Response.Redirect("main.aspx");
                }
                
            }
            catch (NullReferenceException) { }
            com = new OleDbCommand();
            com.Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"C:\\forum.mdb\"");
            com.Connection.Open();
            try
            {
                string login = Request.Cookies["login"].Value;
                string pass = Request.Cookies["pass"].Value;
                if (login != null && pass != null)
                    Response.Redirect("main.aspx");
            }
            catch (NullReferenceException) { }
        }

       
       

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            err.InnerHtml="";
            string login = tbLogin.Text;
            string pass = tbPass.Text;
            if (login == "")
            {
                err.InnerHtml = "Введите логин";
            }
            if(pass == "" || login == "")
            {
                if (err.InnerHtml == "")
                    err.InnerHtml = "Введите пароль";
                else err.InnerHtml += " и пароль";
                return;
            }

            MD5 coder = MD5.Create();

            string hash = Encoding.ASCII.GetString(coder.ComputeHash(Encoding.ASCII.GetBytes(pass)));

            com.CommandText = "select pass from [user] where (name = '" + login + "')";

            string p="";
            try { p = com.ExecuteScalar().ToString(); }
            catch (NullReferenceException) { err.InnerHtml = "Нет такого пользователя"; return; }


            if (p != hash)
            {
                err.InnerHtml = "Пароль не верен";
                return;
            }
            else
            {
                DateTime save = DateTime.Now.AddDays(1);

                if (cbRem.Checked)
                    save = DateTime.MaxValue;


                Response.Cookies.Clear();


                HttpCookie log = new HttpCookie("login");
                log.Expires = save;
                log.Value = login;
                bool r = Request.Browser.Cookies;

                HttpCookie pas = new HttpCookie("pass");
                pas.Expires = save;
                pas.Value = hash;

                Response.Cookies.Add(log);
                Response.Cookies.Add(pas);

              
            }
            if (Request.QueryString.Count > 0)
                Response.Redirect(Request.QueryString["redirect"]);
            Response.Redirect("main.aspx");
        }
    }
}
