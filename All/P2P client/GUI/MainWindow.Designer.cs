namespace P2P_client.GUI
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.split = new System.Windows.Forms.SplitContainer();
            this.materials = new System.Windows.Forms.DataGridView();
            this.Real_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metaFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.раздачуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.опцииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьВыделеннуюРаздачуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoTabsContainer = new System.Windows.Forms.TabControl();
            this.info = new System.Windows.Forms.TabPage();
            this.lbFilesNum = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbPartSize = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbSize = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbMaterialName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.connectionsInfo = new System.Windows.Forms.DataGridView();
            this.Fl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.files = new System.Windows.Forms.DataGridView();
            this.c = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Open_MF = new System.Windows.Forms.OpenFileDialog();
            this.save_to = new System.Windows.Forms.FolderBrowserDialog();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.materials)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.infoTabsContainer.SuspendLayout();
            this.info.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.connectionsInfo)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.files)).BeginInit();
            this.SuspendLayout();
            // 
            // split
            // 
            this.split.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(0, 0);
            this.split.Margin = new System.Windows.Forms.Padding(4);
            this.split.Name = "split";
            this.split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.materials);
            this.split.Panel1.Controls.Add(this.menuStrip1);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.infoTabsContainer);
            this.split.Size = new System.Drawing.Size(1195, 640);
            this.split.SplitterDistance = 463;
            this.split.SplitterWidth = 5;
            this.split.TabIndex = 0;
            // 
            // materials
            // 
            this.materials.AllowUserToAddRows = false;
            this.materials.AllowUserToDeleteRows = false;
            this.materials.AllowUserToOrderColumns = true;
            this.materials.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.materials.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.materials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.materials.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Real_Id,
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column3,
            this.Column5,
            this.Column6});
            this.materials.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.materials.Location = new System.Drawing.Point(15, 33);
            this.materials.Margin = new System.Windows.Forms.Padding(4);
            this.materials.MultiSelect = false;
            this.materials.Name = "materials";
            this.materials.RowTemplate.Height = 24;
            this.materials.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.materials.Size = new System.Drawing.Size(1164, 413);
            this.materials.TabIndex = 0;
            // 
            // Real_Id
            // 
            this.Real_Id.HeaderText = "Real_Id";
            this.Real_Id.Name = "Real_Id";
            this.Real_Id.Visible = false;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Connected clients";
            this.Column2.Name = "Column2";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Download Speed";
            this.Column4.Name = "Column4";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Upload Speed";
            this.Column3.Name = "Column3";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Downloaded";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Uploaded";
            this.Column6.Name = "Column6";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.опцииToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1193, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьToolStripMenuItem,
            this.создатьToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // добавитьToolStripMenuItem
            // 
            this.добавитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.metaFileToolStripMenuItem});
            this.добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
            this.добавитьToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.добавитьToolStripMenuItem.Text = "Добавить";
            // 
            // metaFileToolStripMenuItem
            // 
            this.metaFileToolStripMenuItem.Name = "metaFileToolStripMenuItem";
            this.metaFileToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.metaFileToolStripMenuItem.Text = "MetaFile";
            this.metaFileToolStripMenuItem.Click += new System.EventHandler(this.metaFileToolStripMenuItem_Click);
            // 
            // создатьToolStripMenuItem
            // 
            this.создатьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.раздачуToolStripMenuItem});
            this.создатьToolStripMenuItem.Name = "создатьToolStripMenuItem";
            this.создатьToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.создатьToolStripMenuItem.Text = "Создать";
            // 
            // раздачуToolStripMenuItem
            // 
            this.раздачуToolStripMenuItem.Name = "раздачуToolStripMenuItem";
            this.раздачуToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
            this.раздачуToolStripMenuItem.Text = "Раздачу";
            this.раздачуToolStripMenuItem.Click += new System.EventHandler(this.раздачуToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // опцииToolStripMenuItem
            // 
            this.опцииToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem,
            this.удалитьВыделеннуюРаздачуToolStripMenuItem});
            this.опцииToolStripMenuItem.Name = "опцииToolStripMenuItem";
            this.опцииToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.опцииToolStripMenuItem.Text = "Опции";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(285, 24);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // удалитьВыделеннуюРаздачуToolStripMenuItem
            // 
            this.удалитьВыделеннуюРаздачуToolStripMenuItem.Name = "удалитьВыделеннуюРаздачуToolStripMenuItem";
            this.удалитьВыделеннуюРаздачуToolStripMenuItem.Size = new System.Drawing.Size(285, 24);
            this.удалитьВыделеннуюРаздачуToolStripMenuItem.Text = "Удалить выделенную раздачу";
            this.удалитьВыделеннуюРаздачуToolStripMenuItem.Click += new System.EventHandler(this.удалитьВыделеннуюРаздачуToolStripMenuItem_Click);
            // 
            // infoTabsContainer
            // 
            this.infoTabsContainer.Controls.Add(this.info);
            this.infoTabsContainer.Controls.Add(this.tabPage2);
            this.infoTabsContainer.Controls.Add(this.tabPage1);
            this.infoTabsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoTabsContainer.HotTrack = true;
            this.infoTabsContainer.Location = new System.Drawing.Point(0, 0);
            this.infoTabsContainer.Margin = new System.Windows.Forms.Padding(4);
            this.infoTabsContainer.Name = "infoTabsContainer";
            this.infoTabsContainer.SelectedIndex = 0;
            this.infoTabsContainer.Size = new System.Drawing.Size(1193, 170);
            this.infoTabsContainer.TabIndex = 0;
            // 
            // info
            // 
            this.info.Controls.Add(this.lbFilesNum);
            this.info.Controls.Add(this.label5);
            this.info.Controls.Add(this.lbPartSize);
            this.info.Controls.Add(this.label4);
            this.info.Controls.Add(this.lbSize);
            this.info.Controls.Add(this.label2);
            this.info.Controls.Add(this.lbMaterialName);
            this.info.Controls.Add(this.label1);
            this.info.Location = new System.Drawing.Point(4, 25);
            this.info.Margin = new System.Windows.Forms.Padding(4);
            this.info.Name = "info";
            this.info.Padding = new System.Windows.Forms.Padding(4);
            this.info.Size = new System.Drawing.Size(1185, 141);
            this.info.TabIndex = 0;
            this.info.Text = "Информация";
            this.info.UseVisualStyleBackColor = true;
            // 
            // lbFilesNum
            // 
            this.lbFilesNum.AutoSize = true;
            this.lbFilesNum.Location = new System.Drawing.Point(1139, 64);
            this.lbFilesNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFilesNum.Name = "lbFilesNum";
            this.lbFilesNum.Size = new System.Drawing.Size(28, 17);
            this.lbFilesNum.TabIndex = 7;
            this.lbFilesNum.Text = "n/a";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(971, 64);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Количество файлов";
            // 
            // lbPartSize
            // 
            this.lbPartSize.AutoSize = true;
            this.lbPartSize.Location = new System.Drawing.Point(188, 64);
            this.lbPartSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPartSize.Name = "lbPartSize";
            this.lbPartSize.Size = new System.Drawing.Size(28, 17);
            this.lbPartSize.TabIndex = 5;
            this.lbPartSize.Text = "n/a";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(494, 64);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Размер раздачи";
            // 
            // lbSize
            // 
            this.lbSize.AutoSize = true;
            this.lbSize.Location = new System.Drawing.Point(662, 64);
            this.lbSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSize.Name = "lbSize";
            this.lbSize.Size = new System.Drawing.Size(28, 17);
            this.lbSize.TabIndex = 3;
            this.lbSize.Text = "n/a";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Размер части";
            // 
            // lbMaterialName
            // 
            this.lbMaterialName.AutoSize = true;
            this.lbMaterialName.Location = new System.Drawing.Point(188, 32);
            this.lbMaterialName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbMaterialName.Name = "lbMaterialName";
            this.lbMaterialName.Size = new System.Drawing.Size(28, 17);
            this.lbMaterialName.TabIndex = 1;
            this.lbMaterialName.Text = "n/a";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.connectionsInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1185, 141);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Соединения";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // connectionsInfo
            // 
            this.connectionsInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.connectionsInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.connectionsInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.connectionsInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Fl,
            this.Column9});
            this.connectionsInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectionsInfo.Location = new System.Drawing.Point(4, 4);
            this.connectionsInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connectionsInfo.Name = "connectionsInfo";



            this.connectionsInfo.RowTemplate.Height = 24;
            this.connectionsInfo.Size = new System.Drawing.Size(1177, 133);
            this.connectionsInfo.TabIndex = 0;
            // 
            // Fl
            // 
            this.Fl.HeaderText = "Адрес";
            this.Fl.Name = "Fl";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Порт";
            this.Column9.Name = "Column9";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.files);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1185, 141);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Файлы";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // files
            // 
            this.files.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.files.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.files.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.c,
            this.Column7,
            this.Column8});
            this.files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.files.Location = new System.Drawing.Point(0, 0);
            this.files.Margin = new System.Windows.Forms.Padding(4);
            this.files.Name = "files";
            this.files.RowTemplate.Height = 24;
            this.files.Size = new System.Drawing.Size(1185, 141);
            this.files.TabIndex = 0;
            // 
            // c
            // 
            this.c.HeaderText = "Название";
            this.c.Name = "c";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Размер";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Завершено";
            this.Column8.Name = "Column8";
            // 
            // Open_MF
            // 
            this.Open_MF.Filter = "Файл разачи (*.mf)|*.mf";
            this.Open_MF.FilterIndex = 2;
            this.Open_MF.FileOk += new System.ComponentModel.CancelEventHandler(this.Open_MF_FileOk);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 640);
            this.Controls.Add(this.split);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainWindow";
            this.Text = "Клиент одноранговой файлообменной сети";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel1.PerformLayout();
            this.split.Panel2.ResumeLayout(false);
            this.split.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.materials)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.infoTabsContainer.ResumeLayout(false);
            this.info.ResumeLayout(false);
            this.info.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.connectionsInfo)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.files)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.DataGridView materials;
        private System.Windows.Forms.TabControl infoTabsContainer;
        private System.Windows.Forms.TabPage info;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lbMaterialName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbPartSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView files;
        private System.Windows.Forms.DataGridViewTextBoxColumn c;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metaFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem раздачуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        public System.Windows.Forms.OpenFileDialog Open_MF;
        public System.Windows.Forms.FolderBrowserDialog save_to;
        private System.Windows.Forms.DataGridViewTextBoxColumn Real_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridView connectionsInfo;
        private System.Windows.Forms.ToolStripMenuItem опцииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.ToolStripMenuItem удалитьВыделеннуюРаздачуToolStripMenuItem;
        private System.Windows.Forms.Label lbFilesNum;
        private System.Windows.Forms.Label label5;
    }
}