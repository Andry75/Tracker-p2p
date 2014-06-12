using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace P2P_client.GUI
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            upd.Value = Config.UpdateTimeout / 60 / 1000;
            cons.Value = Config.ConnectionsMax;
            port.Text = Config.Port.ToString();
            cte.Checked = Config.CTE_Enaibled;
            idt.Checked = Config.IDT_Enaibled;
            radioButton1.Checked = Config.idtmode;
            radioButton2.Checked = !Config.idtmode;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            Config.UpdateTimeout = 60 * 1000 * (int)upd.Value;
            Config.ConnectionsMax = (int)cons.Value;
            Config.Port = Convert.ToInt16(port.Text);
            Config.CTE_Enaibled = cte.Checked;
            Config.IDT_Enaibled = idt.Checked;
            if (!Config.IDT_Enaibled) Config.idtmode = false;
            else
            {
                if (radioButton1.Checked) Config.idtmode = true;
                else Config.idtmode = false;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            groupBox1.Enabled = c.Checked;
            if (Config.idtmode) radioButton2.Checked = true;
            else radioButton1.Checked = true; 
        }

        private void port_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                short Port = Convert.ToInt16(port.Text);
            }
            catch (System.FormatException)
            { 
                MessageBox.Show("Неверный формат порта");
                e.Cancel = true;
                return; 
            }
        }
    }
}
