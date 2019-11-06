namespace PhanMemNoiSoi
{
    partial class gbCheckRecord
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gbCheckRecord));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnCheckRecord = new System.Windows.Forms.Panel();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.image = new System.Windows.Forms.DataGridViewImageColumn();
            this.note = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lbNumImgChecked = new System.Windows.Forms.Label();
            this.btnCancle = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.pnCheckRecord);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(825, 612);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Danh mục khám bệnh";
            // 
            // pnCheckRecord
            // 
            this.pnCheckRecord.AutoScroll = true;
            this.pnCheckRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnCheckRecord.Location = new System.Drawing.Point(3, 22);
            this.pnCheckRecord.Name = "pnCheckRecord";
            this.pnCheckRecord.Size = new System.Drawing.Size(819, 587);
            this.pnCheckRecord.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox3.Controls.Add(this.dgvMain);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(843, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(409, 612);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Danh sách hình ảnh";
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.BackgroundColor = System.Drawing.Color.White;
            this.dgvMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.select,
            this.image,
            this.note});
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.Location = new System.Drawing.Point(3, 22);
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dgvMain.Size = new System.Drawing.Size(403, 587);
            this.dgvMain.TabIndex = 0;
            // 
            // select
            // 
            this.select.HeaderText = "X";
            this.select.Name = "select";
            this.select.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.select.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.select.Width = 30;
            // 
            // image
            // 
            this.image.HeaderText = "Ảnh";
            this.image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.image.Name = "image";
            this.image.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.image.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.image.Width = 200;
            // 
            // note
            // 
            this.note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.note.DefaultCellStyle = dataGridViewCellStyle1;
            this.note.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.note.HeaderText = "Ghi chú";
            this.note.Name = "note";
            this.note.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.note.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lbNumImgChecked
            // 
            this.lbNumImgChecked.AutoSize = true;
            this.lbNumImgChecked.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNumImgChecked.ForeColor = System.Drawing.Color.Blue;
            this.lbNumImgChecked.Location = new System.Drawing.Point(868, 630);
            this.lbNumImgChecked.Name = "lbNumImgChecked";
            this.lbNumImgChecked.Size = new System.Drawing.Size(51, 20);
            this.lbNumImgChecked.TabIndex = 8;
            this.lbNumImgChecked.Text = "label1";
            // 
            // btnCancle
            // 
            this.btnCancle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancle.Image = global::PhanMemNoiSoi.Properties.Resources.delete_24x24;
            this.btnCancle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancle.Location = new System.Drawing.Point(1151, 623);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(98, 35);
            this.btnCancle.TabIndex = 14;
            this.btnCancle.Text = "    Thoát";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = global::PhanMemNoiSoi.Properties.Resources.user_24;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(8, 623);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 35);
            this.button1.TabIndex = 5;
            this.button1.Text = "   Xem báo cáo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // gbCheckRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(1257, 666);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.lbNumImgChecked);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "gbCheckRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Báo cáo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.gbCheckRecord_FormClosed);
            this.Load += new System.EventHandler(this.Report_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.Panel pnCheckRecord;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbNumImgChecked;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.DataGridViewCheckBoxColumn select;
        private System.Windows.Forms.DataGridViewImageColumn image;
        private System.Windows.Forms.DataGridViewComboBoxColumn note;
    }
}