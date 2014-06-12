using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using P2P_client;
using System.Net;

namespace Meta_File_Editor
{
    public partial class Form1 : Form
    {
        string open_file = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Server_IP.Text!=null||Server_Port.Text!=null||ID_Client.Text!=null||ID_Track.Text!=null)
            {


                IPAddress Server_ip = IPAddress.Parse(Server_IP.Text);
                short Sepver_port = Convert.ToInt16(Server_Port.Text);
                byte[] ipport = new byte[6];

                byte[] ipbytes = BitConverter.GetBytes((int)Server_ip.Address);
                for (int i = 0; i < 4; i++)
                {
                    ipport[i] = ipbytes[i];
                }
                byte[] portbytes = BitConverter.GetBytes(Sepver_port);
                ipport[4] = portbytes[0]; ipport[5] = portbytes[1];


                FileStream fs = new FileStream(open_file, FileMode.Open, FileAccess.Read);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Meta_File mf = (Meta_File)bf.Deserialize(fs);

                mf.IP_Port_Server = new Client();
                mf.IP_Port_Server.Deserialize(ipport);

                mf.ID_Client = Convert.ToInt32(ID_Client.Text) ;
                mf.ID_matirial = Convert.ToInt32(ID_Track.Text);

                if (saveFileDialog1.ShowDialog()==System.Windows.Forms.DialogResult.OK)
                {
                    fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);

                bf.Serialize(fs, mf);
                fs.Close();
                } 
               
                
            }
            else
                MessageBox.Show("Не все поля заполнены");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Server_IP.Enabled = false;
            Server_Port.Enabled = false;
            ID_Client.Enabled = false;
            ID_Track.Enabled = false;
            openFileDialog1.Filter = "Meta-File (*.mf)|*.mf";
            saveFileDialog1.Filter = "Meta-File (*.mf)|*.mf";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK== openFileDialog1.ShowDialog())
            {
                open_file = openFileDialog1.FileName;
            }
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Server_IP.Enabled = true;
            Server_Port.Enabled = true;
            ID_Client.Enabled = true;
            ID_Track.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
