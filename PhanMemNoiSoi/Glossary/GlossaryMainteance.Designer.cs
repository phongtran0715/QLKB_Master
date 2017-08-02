namespace PhanMemNoiSoi
{
    partial class GlossaryMainteance
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlossaryMainteance));
            this.btnThoat = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnAddInfoReport = new System.Windows.Forms.Button();
            this.btnDeleteInfoReport = new System.Windows.Forms.Button();
            this.dgvInfo = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnEditCheckContent = new System.Windows.Forms.Button();
            this.btnDeleteCheckContent = new System.Windows.Forms.Button();
            this.btnAddCheckContent = new System.Windows.Forms.Button();
            this.btnEditCheck = new System.Windows.Forms.Button();
            this.btnDeleteCheck = new System.Windows.Forms.Button();
            this.btnAddCheck = new System.Windows.Forms.Button();
            this.dgCheckContentDetail = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCheck = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCheckContentDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheck)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThoat.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThoat.Image = ((System.Drawing.Image)(resources.GetObject("btnThoat.Image")));
            this.btnThoat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThoat.Location = new System.Drawing.Point(498, 598);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(99, 35);
            this.btnThoat.TabIndex = 13;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click_1);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuItem1.Text = "Rename";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnMoveDown);
            this.tabPage5.Controls.Add(this.btnMoveUp);
            this.tabPage5.Controls.Add(this.btnAddInfoReport);
            this.tabPage5.Controls.Add(this.btnDeleteInfoReport);
            this.tabPage5.Controls.Add(this.dgvInfo);
            this.tabPage5.Location = new System.Drawing.Point(4, 27);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(577, 549);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Thông tin in trên phiếu khám";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDown.Image")));
            this.btnMoveDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoveDown.Location = new System.Drawing.Point(323, 356);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(143, 35);
            this.btnMoveDown.TabIndex = 12;
            this.btnMoveDown.Text = "      Chuyển xuống";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUp.Image")));
            this.btnMoveUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoveUp.Location = new System.Drawing.Point(183, 356);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(134, 35);
            this.btnMoveUp.TabIndex = 11;
            this.btnMoveUp.Text = "      Chuyển lên";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnAddInfoReport
            // 
            this.btnAddInfoReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddInfoReport.Image = ((System.Drawing.Image)(resources.GetObject("btnAddInfoReport.Image")));
            this.btnAddInfoReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddInfoReport.Location = new System.Drawing.Point(78, 356);
            this.btnAddInfoReport.Name = "btnAddInfoReport";
            this.btnAddInfoReport.Size = new System.Drawing.Size(99, 35);
            this.btnAddInfoReport.TabIndex = 10;
            this.btnAddInfoReport.Text = "  Thêm";
            this.btnAddInfoReport.UseVisualStyleBackColor = true;
            this.btnAddInfoReport.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnDeleteInfoReport
            // 
            this.btnDeleteInfoReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteInfoReport.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteInfoReport.Image")));
            this.btnDeleteInfoReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteInfoReport.Location = new System.Drawing.Point(472, 356);
            this.btnDeleteInfoReport.Name = "btnDeleteInfoReport";
            this.btnDeleteInfoReport.Size = new System.Drawing.Size(99, 35);
            this.btnDeleteInfoReport.TabIndex = 9;
            this.btnDeleteInfoReport.Text = "Xóa ";
            this.btnDeleteInfoReport.UseVisualStyleBackColor = true;
            this.btnDeleteInfoReport.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgvInfo
            // 
            this.dgvInfo.AllowUserToAddRows = false;
            this.dgvInfo.AllowUserToDeleteRows = false;
            this.dgvInfo.AllowUserToResizeRows = false;
            this.dgvInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInfo.Location = new System.Drawing.Point(21, 19);
            this.dgvInfo.MultiSelect = false;
            this.dgvInfo.Name = "dgvInfo";
            this.dgvInfo.ReadOnly = true;
            this.dgvInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInfo.Size = new System.Drawing.Size(550, 318);
            this.dgvInfo.TabIndex = 2;
            this.dgvInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInfo_CellClick);
            this.dgvInfo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvInfo_DataBindingComplete);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnEditCheckContent);
            this.tabPage1.Controls.Add(this.btnDeleteCheckContent);
            this.tabPage1.Controls.Add(this.btnAddCheckContent);
            this.tabPage1.Controls.Add(this.btnEditCheck);
            this.tabPage1.Controls.Add(this.btnDeleteCheck);
            this.tabPage1.Controls.Add(this.btnAddCheck);
            this.tabPage1.Controls.Add(this.dgCheckContentDetail);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.dgvCheck);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(577, 549);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Danh mục kết luận";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnEditCheckContent
            // 
            this.btnEditCheckContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditCheckContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditCheckContent.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCheckContent.Image")));
            this.btnEditCheckContent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditCheckContent.Location = new System.Drawing.Point(466, 501);
            this.btnEditCheckContent.Name = "btnEditCheckContent";
            this.btnEditCheckContent.Size = new System.Drawing.Size(99, 35);
            this.btnEditCheckContent.TabIndex = 13;
            this.btnEditCheckContent.Text = "Sửa";
            this.btnEditCheckContent.UseVisualStyleBackColor = true;
            this.btnEditCheckContent.Click += new System.EventHandler(this.btnSua_ND_Click);
            // 
            // btnDeleteCheckContent
            // 
            this.btnDeleteCheckContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteCheckContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteCheckContent.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteCheckContent.Image")));
            this.btnDeleteCheckContent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteCheckContent.Location = new System.Drawing.Point(350, 501);
            this.btnDeleteCheckContent.Name = "btnDeleteCheckContent";
            this.btnDeleteCheckContent.Size = new System.Drawing.Size(99, 35);
            this.btnDeleteCheckContent.TabIndex = 12;
            this.btnDeleteCheckContent.Text = "Xóa ";
            this.btnDeleteCheckContent.UseVisualStyleBackColor = true;
            this.btnDeleteCheckContent.Click += new System.EventHandler(this.btnXoa_ND_Click);
            // 
            // btnAddCheckContent
            // 
            this.btnAddCheckContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCheckContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCheckContent.Image = ((System.Drawing.Image)(resources.GetObject("btnAddCheckContent.Image")));
            this.btnAddCheckContent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddCheckContent.Location = new System.Drawing.Point(234, 501);
            this.btnAddCheckContent.Name = "btnAddCheckContent";
            this.btnAddCheckContent.Size = new System.Drawing.Size(99, 35);
            this.btnAddCheckContent.TabIndex = 11;
            this.btnAddCheckContent.Text = "  Thêm";
            this.btnAddCheckContent.UseVisualStyleBackColor = true;
            this.btnAddCheckContent.Click += new System.EventHandler(this.btnThem_ND_Click);
            // 
            // btnEditCheck
            // 
            this.btnEditCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditCheck.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCheck.Image")));
            this.btnEditCheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditCheck.Location = new System.Drawing.Point(466, 207);
            this.btnEditCheck.Name = "btnEditCheck";
            this.btnEditCheck.Size = new System.Drawing.Size(99, 35);
            this.btnEditCheck.TabIndex = 8;
            this.btnEditCheck.Text = "Sửa";
            this.btnEditCheck.UseVisualStyleBackColor = true;
            this.btnEditCheck.Click += new System.EventHandler(this.btnSua_DM_Click);
            // 
            // btnDeleteCheck
            // 
            this.btnDeleteCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteCheck.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteCheck.Image")));
            this.btnDeleteCheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteCheck.Location = new System.Drawing.Point(350, 207);
            this.btnDeleteCheck.Name = "btnDeleteCheck";
            this.btnDeleteCheck.Size = new System.Drawing.Size(99, 35);
            this.btnDeleteCheck.TabIndex = 7;
            this.btnDeleteCheck.Text = "Xóa ";
            this.btnDeleteCheck.UseVisualStyleBackColor = true;
            this.btnDeleteCheck.Click += new System.EventHandler(this.btnXoa_DM_Click);
            // 
            // btnAddCheck
            // 
            this.btnAddCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCheck.Image = ((System.Drawing.Image)(resources.GetObject("btnAddCheck.Image")));
            this.btnAddCheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddCheck.Location = new System.Drawing.Point(234, 207);
            this.btnAddCheck.Name = "btnAddCheck";
            this.btnAddCheck.Size = new System.Drawing.Size(99, 35);
            this.btnAddCheck.TabIndex = 6;
            this.btnAddCheck.Text = "  Thêm";
            this.btnAddCheck.UseVisualStyleBackColor = true;
            this.btnAddCheck.Click += new System.EventHandler(this.btnThem_DM_Click);
            // 
            // dgCheckContentDetail
            // 
            this.dgCheckContentDetail.AllowUserToAddRows = false;
            this.dgCheckContentDetail.AllowUserToDeleteRows = false;
            this.dgCheckContentDetail.AllowUserToResizeRows = false;
            this.dgCheckContentDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgCheckContentDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgCheckContentDetail.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgCheckContentDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCheckContentDetail.Location = new System.Drawing.Point(17, 277);
            this.dgCheckContentDetail.Name = "dgCheckContentDetail";
            this.dgCheckContentDetail.ReadOnly = true;
            this.dgCheckContentDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgCheckContentDetail.Size = new System.Drawing.Size(550, 200);
            this.dgCheckContentDetail.TabIndex = 3;
            this.dgCheckContentDetail.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCheckContentDetail_CellDoubleClick);
            this.dgCheckContentDetail.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgNoiDung_DataBindingComplete);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 244);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nội dung";
            // 
            // dgvCheck
            // 
            this.dgvCheck.AllowUserToAddRows = false;
            this.dgvCheck.AllowUserToDeleteRows = false;
            this.dgvCheck.AllowUserToResizeRows = false;
            this.dgvCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCheck.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCheck.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvCheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCheck.Location = new System.Drawing.Point(17, 39);
            this.dgvCheck.MultiSelect = false;
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.ReadOnly = true;
            this.dgvCheck.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCheck.Size = new System.Drawing.Size(550, 150);
            this.dgvCheck.TabIndex = 1;
            this.dgvCheck.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheck_CellDoubleClick);
            this.dgvCheck.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgDanhMuc_DataBindingComplete);
            this.dgvCheck.SelectionChanged += new System.EventHandler(this.dgDanhMuc_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Danh mục";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(585, 580);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            this.tabControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseClick);
            // 
            // GlossaryMainteance
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(602, 641);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GlossaryMainteance";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý danh mục";
            this.UserIsAllowed += new System.EventHandler(this.GlossaryMainteance_UserIsAllowed);
            this.UserIsDenied += new System.EventHandler(this.GlossaryMainteance_UserIsDenied);
            this.Load += new System.EventHandler(this.GlossaryMainteance_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCheckContentDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheck)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnEditCheckContent;
        private System.Windows.Forms.Button btnDeleteCheckContent;
        private System.Windows.Forms.Button btnAddCheckContent;
        private System.Windows.Forms.Button btnEditCheck;
        private System.Windows.Forms.Button btnDeleteCheck;
        private System.Windows.Forms.Button btnAddCheck;
        private System.Windows.Forms.DataGridView dgCheckContentDetail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.DataGridView dgvInfo;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnAddInfoReport;
        private System.Windows.Forms.Button btnDeleteInfoReport;
    }
}