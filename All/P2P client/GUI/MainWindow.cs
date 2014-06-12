using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using P2P_client;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace P2P_client.GUI
{

    public partial class MainWindow : Form
    {
        System.Windows.Forms.Timer up =new System.Windows.Forms.Timer();
        static List<Thread> ths = new List<Thread>();

        public MainWindow()
        {
            InitializeComponent();
            up.Interval = 1024;
            up.Tick += new EventHandler(up_Tick);
            up.Start();
        }

        void up_Tick(object sender, EventArgs e)
        {
            FillTable();
            infoFill();
        }




        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
                ProgramMaterials.MaterialsArray  = new List<Material>();
                if(!Directory.Exists("Materials\\")) return;
                string[] files = Directory.GetFiles("Materials\\");
               
                Material tempm;
                
                for (int i = 0; i < files.Length; i++)
                {
                     FileStream fs =new FileStream(files[i],FileMode.Open);
                BinaryFormatter bf =new BinaryFormatter();
                    tempm= (Material)bf.Deserialize(fs);
                        
                    
                    ProgramMaterials.MaterialsArray.Add(tempm);
                    FillTable();
                    
                } 
            }
            catch
            {
            }
           
        }

        private void FillTable()
        {
            materials.Rows.Clear();
            for (int i = 0; i < ProgramMaterials.MaterialsArray.Count; i++)
            {
                materials.Rows.Add();
                materials[0, i].Value = ProgramMaterials.MaterialsArray[i].IdMaterial;
                materials[1, i].Value = ProgramMaterials.MaterialsArray[i].Name;
                materials[2, i].Value = ProgramMaterials.MaterialsArray[i].ConnectionsNum;
                materials[3, i].Value = ConvertValue(ProgramMaterials.MaterialsArray[i].speed_Download)+"/c";
                materials[4, i].Value = ConvertValue(ProgramMaterials.MaterialsArray[i].speed_Upload)+"/c";
                materials[5, i].Value = ConvertValue(ProgramMaterials.MaterialsArray[i].Stat_Download);
                materials[6, i].Value = ConvertValue(ProgramMaterials.MaterialsArray[i].Stat_Upload);
            }
          
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            infoTabsContainer.SelectedIndex++;
            infoTabsContainer.SelectedIndex--;
        }

        private void infoFill()
        {
            if(materials.SelectedRows.Count ==0) return;

            int rowId = materials.SelectedRows[0].Index;
            int id = (int)materials[0, rowId].Value;
            foreach (Material m in ProgramMaterials.MaterialsArray)
            {
                if(id == m.IdMaterial)
                {
                    if (infoTabsContainer.SelectedIndex == 0)
                    {
                        lbMaterialName.Text = m.Name;
                        lbPartSize.Text = ConvertValue(m.Files[0].Part_size);
                        lbSize.Text = ConvertValue(m.size);
                        lbFilesNum.Text = m.Files.Length.ToString(); ;
                        break;
                    }

                    if (infoTabsContainer.SelectedIndex == 1)
                    {
                        int i =0;
                        foreach (Connection con in m.Connections)
                        {
                            
                            connectionsInfo.Rows.Add();
                            connectionsInfo[0, i].Value = ((System.Net.IPEndPoint)con.Socket.RemoteEndPoint).Address.ToString();
                            connectionsInfo[1, i].Value = ((System.Net.IPEndPoint)con.Socket.RemoteEndPoint).Port.ToString();
                        }
                    }

                    if (infoTabsContainer.SelectedIndex == 2)
                    {
                        files.Rows.Clear();
                        for (int i = 0; i < m.Files.Length; i++)
                        {
                            files.Rows.Add();
                            files[0, i].Value = m.Files[i].path;
                            files[1, i].Value = ConvertValue(m.Files[i].File_Size);

                            long size = 0;
                            
                            for(int t = 0;t<m.Files[i].Count_Parts_;t++)
                            {
                                if(m.Files[i].IsPart(t))
                                    size += m.Files[i].Part_size;
                            }

                            files[2, i].Value = 100 * size / m.Files[i].File_Size + " %";
                        }
                        break;
                    }
                }
            }

        }

        private void metaFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open_MF.ShowDialog();

        }

        private void раздачуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMetafile AMF = new AddMetafile();
            AMF.ShowDialog();

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Open_MF_FileOk(object sender, CancelEventArgs e)
        {
           
            Thread th = new Thread(OpenMaterial);
            save_to.ShowDialog();
            th.Start(sender);
            
        }

        private void OpenMaterial(object sender)
        {
            lock (save_to)
            {


                string dir = save_to.SelectedPath;
                string name = Open_MF.FileName;
                ((OpenFileDialog)sender).Dispose();
               

                Material Open_File=null;
                
                

                Open_File = new Material(dir, name);// Создаем материал по ссылки

                

                
                
                Open_File.Start();
                lock (ProgramMaterials.MaterialsArray)// Лочим доступ к массиву на время манипуляций с ним
                {
                    ProgramMaterials.MaterialsArray.Add(Open_File);// добовляем новую раздачу 
                }
                
            }
        }
        delegate void ShowFormDelegate();
        
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            

            /*******************************************************************/
            Directory.SetCurrentDirectory(Application.StartupPath);
            Directory.CreateDirectory("./Materials");
            foreach (Material m in ProgramMaterials.MaterialsArray)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream("Materials\\" + m.Name + "_" + m.IdMaterial, FileMode.Create);
                bf.Serialize(fs, m);
                fs.Close();
                //m.SaveMetaFile("matirials//" + m.Name + "_" + m.IdMaterial + ".cm");
            }
            Application.Exit();
            /*******************************************************************/
        }

        private string ConvertValue(double bytes)
        {

            char b='\0';
            string val="";
            //bytes
            if (bytes < 1024)
            {
                val = bytes.ToString();
                b = val[val.Length-1];
                val += " байт";
                goto answer;
            }
                //kbytes
            else if (bytes < Math.Pow(1024,2))
            {
                bytes /= 1024;
                bytes = Math.Round(bytes, 2);
                val = bytes.ToString();
                b = val[val.Length - 1];
                val += " килобайт";
                goto answer;
            }
            //mbytes
            else if (bytes < Math.Pow(1024,3))
            {
                bytes /= Math.Pow(1024, 2);
                bytes = Math.Round(bytes, 2);
                val = bytes.ToString();
                b = val[val.Length - 1];
                val += " мегабайт";
                goto answer;
            }
            //gbytes
            else if (bytes < Math.Pow(1024, 4))
            {
                bytes /= Math.Pow(1024, 3);
                bytes = Math.Round(bytes, 2);
                val = bytes.ToString();
                b = val[val.Length - 1];
                val += " гигабайт";
                goto answer;
            }
            //tbytes
            else if (bytes < Math.Pow(1024, 5))
            {
                bytes /= Math.Pow(1024, 4);
                bytes = Math.Round(bytes, 2);
                val = bytes.ToString();
                b = val[val.Length - 1];
                val += " терабайт";
                goto answer;
            }
            answer:
                    switch(b)
                    {
                        case '2':
                        case '3':
                        case '4':
                                    if (!val.Contains(".")) val += 'а';               
                                    break;
                        default:
                                    break;
                    }
            return val;
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options form = new Options();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            Config.Save();

        }

        private void удалитьВыделеннуюРаздачуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(materials.SelectedRows.Count !=1) return;
            int mat = (int)materials.SelectedRows[0].Cells[0].Value;

            foreach (Material m in ProgramMaterials.MaterialsArray)
            {
                if (mat == m.IdMaterial)
                {
                    ProgramMaterials.MaterialsArray.Remove(m);
                    System.IO.File.Delete("Materials\\" + m.Name + "_" + m.IdMaterial);
                    return;

                }
            }
        }

       
    }
}
