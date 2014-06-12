using System;
using System.Data.OleDb;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;

namespace ASP.NET_Forum
{
    public class Authentication
    {
        public static bool CheckLogin(OleDbCommand com, string Login, string PassHash)
        {
            string hash_pass="";
            com.CommandText = "select pass from [user] where name = '" + Login + "';";
            try
            {
                hash_pass = com.ExecuteScalar().ToString();
            }
            catch (Exception) { }

            if (PassHash != hash_pass)              
                return false;

  
            return true;
        }
    }
}
