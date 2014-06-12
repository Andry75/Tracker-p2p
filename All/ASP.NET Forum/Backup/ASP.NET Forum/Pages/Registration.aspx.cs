using System;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;

namespace ASP.NET_Forum.Pages
{
    public partial class Registration : System.Web.UI.Page
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
                if (login != null && pass != null)
                    Response.Redirect("main.aspx");
            }
            catch (NullReferenceException) { }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string login;
            string pass;
                       

            login = tbLogin.Text;
            pass = tbPass.Text;

            if (login == "" || pass == "")
            {
                err.InnerHtml = "Ведите регистрационные данные";
                return;
            }

            com.CommandText = "select count(*) from [user] where name = '" + login + "';";
            int n = (int)com.ExecuteScalar();
            if (n > 0)
            {
                err.InnerHtml = "Данный логин занят";
                return;
            }

            string hash = Encoding.ASCII.GetString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(pass)));

            hash.Replace("\'", "\'\'");


            com.CommandText = "insert into [user](name,pass) values('"+login+"','"+hash+"');";
            com.ExecuteNonQuery();
            Response.Redirect("login.aspx");
        }

      
    }
}
