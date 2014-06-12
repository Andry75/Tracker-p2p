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
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(pass);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            
           


            com.CommandText = "insert into [user](name,pass) values('"+login+"','"+sb.ToString()+"');";
            com.ExecuteNonQuery();
            Response.Redirect("login.aspx");
        }

      
    }
}
