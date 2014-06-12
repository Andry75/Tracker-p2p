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
    public partial class AddMetafile : Form
    {
        public AddMetafile()
        {
            InitializeComponent();
        }

        private void browse_Click(object sender, EventArgs e)
        {
            Folder_Set.ShowDialog();
            if (Folder_Set.SelectedPath != null)
            {
                Path_string.Text = Folder_Set.SelectedPath;
            }

        }

        private void AddMetafile_Load(object sender, EventArgs e)
        {
            Part_Size.SelectedIndex = 0;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void create_Click(object sender, EventArgs e)
        {
            if (track_name.TextLength < 6)
                MessageBox.Show("Вы не ввели имя раздачи! Минимальное количество символов 6", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (Path_string.TextLength < 3)
                    MessageBox.Show("Вы не выбрали каталог!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    string pash = Path_string.Text;
                    string name = track_name.Text;
                    int part_size;
                    switch (Part_Size.SelectedIndex)
                    {
                        case 0:
                            part_size = 512 * 1024;
                            break;
                        case 1:
                            part_size = 1024 * 1024;
                            break;
                        case 2:
                            part_size = 1024 * 1024 * 2;
                            break;
                        case 3:
                            part_size = 1024 * 1024 * 4;
                            break;
                        default:
                            part_size = 0;
                            break;
                    }
                    if (part_size == 0)
                    {
                        MessageBox.Show("Неправильно введен размер части!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Material Open_File = new Material(name, pash, part_size); ;// Создаем материал по ссылки

                        
                        Open_File.SaveMetaFile(pash + "\\" + name + ".mf");
                        Open_File.Start();
                        lock (ProgramMaterials.MaterialsArray)// Лочим доступ к массиву на время манипуляций с ним
                        {
                            ProgramMaterials.MaterialsArray.Add(Open_File);// добовляем новую раздачю   
                        }
                        Close();
                    }
                }
            }
        }

     
    }
}
