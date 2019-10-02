namespace PhanMemNoiSoi
{
    partial class BackupAndRestore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupAndRestore));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbThang = new System.Windows.Forms.ComboBox();
            this.cbNamThang = new System.Windows.Forms.ComboBox();
            this.cbNam = new System.Windows.Forms.ComboBox();
            this.rbNam = new System.Windows.Forms.RadioButton();
            this.rbThang = new System.Windows.Forms.RadioButton();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dgvBackup = new System.Windows.Forms.DataGridView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackup)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbThang);
            this.groupBox1.Controls.Add(this.cbNamThang);
            this.groupBox1.Controls.Add(this.cbNam);
            this.groupBox1.Controls.Add(this.rbNam);
            this.groupBox1.Controls.Add(this.rbThang);
            this.groupBox1.Controls.Add(this.btnSaveData);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(594, 110);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sao lưu dữ liệu";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(256, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 23;
            this.label1.Text = "Năm";
            // 
            // cbThang
            // 
            this.cbThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThang.FormattingEnabled = true;
            this.cbThang.Location = new System.Drawing.Point(107, 25);
            this.cbThang.Name = "cbThang";
            this.cbThang.Size = new System.Drawing.Size(119, 28);
            this.cbThang.TabIndex = 2;
            // 
            // cbNamThang
            // 
            this.cbNamThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNamThang.FormattingEnabled = true;
            this.cbNamThang.Location = new System.Drawing.Point(314, 25);
            this.cbNamThang.Name = "cbNamThang";
            this.cbNamThang.Size = new System.Drawing.Size(119, 28);
            this.cbNamThang.TabIndex = 3;
            // 
            // cbNam
            // 
            this.cbNam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNam.FormattingEnabled = true;
            this.cbNam.Location = new System.Drawing.Point(107, 68);
            this.cbNam.Name = "cbNam";
            this.cbNam.Size = new System.Drawing.Size(119, 28);
            this.cbNam.TabIndex = 5;
            // 
            // rbNam
            // 
            this.rbNam.AutoSize = true;
            this.rbNam.Location = new System.Drawing.Point(14, 72);
            this.rbNam.Name = "rbNam";
            this.rbNam.Size = new System.Drawing.Size(60, 24);
            this.rbNam.TabIndex = 4;
            this.rbNam.TabStop = true;
            this.rbNam.Text = "Năm";
            this.rbNam.UseVisualStyleBackColor = true;
            this.rbNam.CheckedChanged += new System.EventHandler(this.rbNam_CheckedChanged);
            // 
            // rbThang
            // 
            this.rbThang.AutoSize = true;
            this.rbThang.Location = new System.Drawing.Point(14, 29);
            this.rbThang.Name = "rbThang";
            this.rbThang.Size = new System.Drawing.Size(72, 24);
            this.rbThang.TabIndex = 1;
            this.rbThang.TabStop = true;
            this.rbThang.Text = "Tháng";
            this.rbThang.UseVisualStyleBackColor = true;
            this.rbThang.CheckedChanged += new System.EventHandler(this.rbThang_CheckedChanged);
            // 
            // btnSaveData
            // 
            this.btnSaveData.Image = global::PhanMemNoiSoi.Properties.Resources.save_24;
            this.btnSaveData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveData.Location = new System.Drawing.Point(314, 67);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(122, 35);
            this.btnSaveData.TabIndex = 6;
            this.btnSaveData.Text = "Sao lưu";
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.dgvBackup);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(5, 121);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(594, 238);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Khôi phục dữ liệu";
            // 
            // button3
            // 
            this.button3.Image = global::PhanMemNoiSoi.Properties.Resources.trash_24x24;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(295, 190);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(122, 35);
            this.button3.TabIndex = 9;
            this.button3.Text = "   Xóa file";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Image = global::PhanMemNoiSoi.Properties.Resources.search_24;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(146, 190);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 35);
            this.button2.TabIndex = 8;
            this.button2.Text = "  Tìm file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = global::PhanMemNoiSoi.Properties.Resources.export_24x24;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(6, 190);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 35);
            this.button1.TabIndex = 7;
            this.button1.Text = "     Khôi phục ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgvBackup
            // 
            this.dgvBackup.AllowUserToAddRows = false;
            this.dgvBackup.AllowUserToDeleteRows = false;
            this.dgvBackup.AllowUserToResizeRows = false;
            this.dgvBackup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBackup.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvBackup.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvBackup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBackup.Location = new System.Drawing.Point(6, 25);
            this.dgvBackup.MultiSelect = false;
            this.dgvBackup.Name = "dgvBackup";
            this.dgvBackup.ReadOnly = true;
            this.dgvBackup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBackup.Size = new System.Drawing.Size(582, 150);
            this.dgvBackup.TabIndex = 6;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(11, 365);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(588, 23);
            this.progressBar1.TabIndex = 19;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Image = global::PhanMemNoiSoi.Properties.Resources.delete_24x24;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(501, 394);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(98, 35);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "    Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // BackupAndRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(604, 433);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackupAndRestore";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sao lưu và khôi phục dữ liệu";
            this.UserIsDenied += new System.EventHandler(this.BackupAndRestore_UserIsDenied);
            this.Load += new System.EventHandler(this.BackupAndRestore_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvBackup;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.RadioButton rbNam;
        private System.Windows.Forms.RadioButton rbThang;
        private System.Windows.Forms.ComboBox cbNam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbThang;
        private System.Windows.Forms.ComboBox cbNamThang;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button3;
    }
}