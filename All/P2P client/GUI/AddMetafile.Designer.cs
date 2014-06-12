namespace P2P_client.GUI
{
    partial class AddMetafile
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
            this.browse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Path_string = new System.Windows.Forms.TextBox();
            this.Part_Size = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.create = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.Folder_Set = new System.Windows.Forms.FolderBrowserDialog();
            this.track_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(396, 31);
            this.browse.Margin = new System.Windows.Forms.Padding(4);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(100, 28);
            this.browse.TabIndex = 0;
            this.browse.Text = "Обзор";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Выберите папку";
            // 
            // Path_string
            // 
            this.Path_string.Location = new System.Drawing.Point(19, 31);
            this.Path_string.Margin = new System.Windows.Forms.Padding(4);
            this.Path_string.Name = "Path_string";
            this.Path_string.Size = new System.Drawing.Size(368, 22);
            this.Path_string.TabIndex = 2;
            // 
            // Part_Size
            // 
            this.Part_Size.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Part_Size.FormattingEnabled = true;
            this.Part_Size.Items.AddRange(new object[] {
            "512 Кб",
            "1 Мб",
            "2 Мб",
            "4 Мб"});
            this.Part_Size.Location = new System.Drawing.Point(183, 123);
            this.Part_Size.Margin = new System.Windows.Forms.Padding(4);
            this.Part_Size.Name = "Part_Size";
            this.Part_Size.Size = new System.Drawing.Size(128, 24);
            this.Part_Size.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 126);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Выбрать размер части";
            // 
            // create
            // 
            this.create.Location = new System.Drawing.Point(396, 66);
            this.create.Margin = new System.Windows.Forms.Padding(4);
            this.create.Name = "create";
            this.create.Size = new System.Drawing.Size(100, 28);
            this.create.TabIndex = 5;
            this.create.Text = "Создать";
            this.create.UseVisualStyleBackColor = true;
            this.create.Click += new System.EventHandler(this.create_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(396, 102);
            this.cancel.Margin = new System.Windows.Forms.Padding(4);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(100, 28);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Отмена";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // Folder_Set
            // 
            this.Folder_Set.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // track_name
            // 
            this.track_name.Location = new System.Drawing.Point(18, 86);
            this.track_name.Name = "track_name";
            this.track_name.Size = new System.Drawing.Size(368, 22);
            this.track_name.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 66);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Название раздачи";
            // 
            // AddMetafile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(501, 160);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.track_name);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.create);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Part_Size);
            this.Controls.Add(this.Path_string);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddMetafile";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обзор";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddMetafile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Path_string;
        private System.Windows.Forms.ComboBox Part_Size;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button create;
        private System.Windows.Forms.Button cancel;
        public System.Windows.Forms.FolderBrowserDialog Folder_Set;
        private System.Windows.Forms.TextBox track_name;
        private System.Windows.Forms.Label label3;

    }
}