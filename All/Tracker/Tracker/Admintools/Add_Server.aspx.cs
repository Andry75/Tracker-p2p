using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;


namespace Tracker.Admintools
{
    public partial class Add_Server : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(this.TextBox1.Text);
            short port = Convert.ToInt16(this.TextBox2.Text);
            Conector con = new Conector();
            con.insert("INSERT INTO Servers_Tracker (IP_Server, Port_Server) VALUES     ("+(int)ip.Address+","+port+")");
            con.con_close();
        }
    }
}