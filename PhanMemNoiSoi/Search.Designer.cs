namespace PhanMemNoiSoi
{
    partial class Search
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Search));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbNam = new System.Windows.Forms.RadioButton();
            this.rbThang = new System.Windows.Forms.RadioButton();
            this.rbNgay = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnReCheck = new System.Windows.Forms.Button();
            this.btnDeleteSick = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnTatCa = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.dtNgayKetThuc = new System.Windows.Forms.DateTimePicker();
            this.dtNgayBatDau = new System.Windows.Forms.DateTimePicker();
            this.txtTenSearch = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtBirthday = new System.Windows.Forms.TextBox();
            this.txtCreatetime = new System.Windows.Forms.TextBox();
            this.txtMarred = new System.Windows.Forms.TextBox();
            this.txtJob = new System.Windows.Forms.TextBox();
            this.lbimgCount = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.txtIdCode = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnNextImg = new System.Windows.Forms.Button();
            this.btnPrevImg = new System.Windows.Forms.Button();
            this.pbPatient = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.gbResult = new System.Windows.Forms.GroupBox();
            this.dgvPatient = new System.Windows.Forms.DataGridView();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPatient)).BeginInit();
            this.gbResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatient)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNam);
            this.groupBox1.Controls.Add(this.rbThang);
            this.groupBox1.Controls.Add(this.rbNgay);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(128, 169);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tìm nhanh";
            // 
            // rbNam
            // 
            this.rbNam.AutoSize = true;
            this.rbNam.Location = new System.Drawing.Point(17, 126);
            this.rbNam.Name = "rbNam";
            this.rbNam.Size = new System.Drawing.Size(58, 22);
            this.rbNam.TabIndex = 2;
            this.rbNam.TabStop = true;
            this.rbNam.Text = "Năm";
            this.rbNam.UseVisualStyleBackColor = true;
            this.rbNam.CheckedChanged += new System.EventHandler(this.rbNam_CheckedChanged);
            // 
            // rbThang
            // 
            this.rbThang.AutoSize = true;
            this.rbThang.Location = new System.Drawing.Point(17, 77);
            this.rbThang.Name = "rbThang";
            this.rbThang.Size = new System.Drawing.Size(67, 22);
            this.rbThang.TabIndex = 1;
            this.rbThang.TabStop = true;
            this.rbThang.Text = "Tháng";
            this.rbThang.UseVisualStyleBackColor = true;
            this.rbThang.CheckedChanged += new System.EventHandler(this.rbThang_CheckedChanged);
            // 
            // rbNgay
            // 
            this.rbNgay.AutoSize = true;
            this.rbNgay.Checked = true;
            this.rbNgay.Location = new System.Drawing.Point(17, 32);
            this.rbNgay.Name = "rbNgay";
            this.rbNgay.Size = new System.Drawing.Size(60, 22);
            this.rbNgay.TabIndex = 0;
            this.rbNgay.TabStop = true;
            this.rbNgay.Text = "Ngày";
            this.rbNgay.UseVisualStyleBackColor = true;
            this.rbNgay.CheckedChanged += new System.EventHandler(this.rbNgay_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnReCheck);
            this.groupBox2.Controls.Add(this.btnDeleteSick);
            this.groupBox2.Controls.Add(this.btnXoa);
            this.groupBox2.Controls.Add(this.btnTatCa);
            this.groupBox2.Controls.Add(this.btnTimKiem);
            this.groupBox2.Controls.Add(this.dtNgayKetThuc);
            this.groupBox2.Controls.Add(this.dtNgayBatDau);
            this.groupBox2.Controls.Add(this.txtPhone);
            this.groupBox2.Controls.Add(this.txtTenSearch);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(146, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(677, 169);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bộ lọc";
            // 
            // btnReCheck
            // 
            this.btnReCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReCheck.Image = global::PhanMemNoiSoi.Properties.Resources.refresh_24;
            this.btnReCheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReCheck.Location = new System.Drawing.Point(400, 122);
            this.btnReCheck.Name = "btnReCheck";
            this.btnReCheck.Size = new System.Drawing.Size(98, 35);
            this.btnReCheck.TabIndex = 16;
            this.btnReCheck.Text = "     Khám lại";
            this.btnReCheck.UseVisualStyleBackColor = true;
            this.btnReCheck.Click += new System.EventHandler(this.btnReCheck_Click);
            // 
            // btnDeleteSick
            // 
            this.btnDeleteSick.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSick.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteSick.Image")));
            this.btnDeleteSick.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSick.Location = new System.Drawing.Point(504, 121);
            this.btnDeleteSick.Name = "btnDeleteSick";
            this.btnDeleteSick.Size = new System.Drawing.Size(140, 35);
            this.btnDeleteSick.TabIndex = 13;
            this.btnDeleteSick.Text = "     Xóa bệnh nhân";
            this.btnDeleteSick.UseVisualStyleBackColor = true;
            this.btnDeleteSick.Click += new System.EventHandler(this.btnDeleteSick_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.Image = global::PhanMemNoiSoi.Properties.Resources.trach_24;
            this.btnXoa.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXoa.Location = new System.Drawing.Point(240, 122);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(98, 35);
            this.btnXoa.TabIndex = 15;
            this.btnXoa.Text = " Tạo lại";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnTatCa
            // 
            this.btnTatCa.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTatCa.Image = ((System.Drawing.Image)(resources.GetObject("btnTatCa.Image")));
            this.btnTatCa.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTatCa.Location = new System.Drawing.Point(127, 122);
            this.btnTatCa.Name = "btnTatCa";
            this.btnTatCa.Size = new System.Drawing.Size(98, 35);
            this.btnTatCa.TabIndex = 14;
            this.btnTatCa.Text = "Tất cả";
            this.btnTatCa.UseVisualStyleBackColor = true;
            this.btnTatCa.Click += new System.EventHandler(this.btnTatCa_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.Image = ((System.Drawing.Image)(resources.GetObject("btnTimKiem.Image")));
            this.btnTimKiem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTimKiem.Location = new System.Drawing.Point(9, 120);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(98, 35);
            this.btnTimKiem.TabIndex = 13;
            this.btnTimKiem.Text = "    Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // dtNgayKetThuc
            // 
            this.dtNgayKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtNgayKetThuc.Location = new System.Drawing.Point(469, 74);
            this.dtNgayKetThuc.Name = "dtNgayKetThuc";
            this.dtNgayKetThuc.ShowCheckBox = true;
            this.dtNgayKetThuc.Size = new System.Drawing.Size(175, 24);
            this.dtNgayKetThuc.TabIndex = 11;
            // 
            // dtNgayBatDau
            // 
            this.dtNgayBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtNgayBatDau.Location = new System.Drawing.Point(469, 35);
            this.dtNgayBatDau.Name = "dtNgayBatDau";
            this.dtNgayBatDau.ShowCheckBox = true;
            this.dtNgayBatDau.Size = new System.Drawing.Size(175, 24);
            this.dtNgayBatDau.TabIndex = 10;
            // 
            // txtTenSearch
            // 
            this.txtTenSearch.Location = new System.Drawing.Point(105, 31);
            this.txtTenSearch.MaxLength = 69;
            this.txtTenSearch.Name = "txtTenSearch";
            this.txtTenSearch.Size = new System.Drawing.Size(176, 24);
            this.txtTenSearch.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(370, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 18);
            this.label13.TabIndex = 7;
            this.label13.Text = "Ngày kết thúc";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(369, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 18);
            this.label12.TabIndex = 6;
            this.label12.Text = "Ngày bắt đầu";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 18);
            this.label10.TabIndex = 1;
            this.label10.Text = "Tên";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtBirthday);
            this.groupBox4.Controls.Add(this.txtCreatetime);
            this.groupBox4.Controls.Add(this.txtMarred);
            this.groupBox4.Controls.Add(this.txtJob);
            this.groupBox4.Controls.Add(this.lbimgCount);
            this.groupBox4.Controls.Add(this.txtNote);
            this.groupBox4.Controls.Add(this.txtAge);
            this.groupBox4.Controls.Add(this.txtTelephone);
            this.groupBox4.Controls.Add(this.txtIdCode);
            this.groupBox4.Controls.Add(this.txtName);
            this.groupBox4.Controls.Add(this.btnNextImg);
            this.groupBox4.Controls.Add(this.btnPrevImg);
            this.groupBox4.Controls.Add(this.pbPatient);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(829, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(383, 570);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Chi tiết bệnh nhân";
            // 
            // txtBirthday
            // 
            this.txtBirthday.Enabled = false;
            this.txtBirthday.Location = new System.Drawing.Point(277, 28);
            this.txtBirthday.Name = "txtBirthday";
            this.txtBirthday.Size = new System.Drawing.Size(97, 24);
            this.txtBirthday.TabIndex = 29;
            // 
            // txtCreatetime
            // 
            this.txtCreatetime.Enabled = false;
            this.txtCreatetime.Location = new System.Drawing.Point(276, 118);
            this.txtCreatetime.Name = "txtCreatetime";
            this.txtCreatetime.Size = new System.Drawing.Size(97, 24);
            this.txtCreatetime.TabIndex = 28;
            // 
            // txtMarred
            // 
            this.txtMarred.Enabled = false;
            this.txtMarred.Location = new System.Drawing.Point(84, 71);
            this.txtMarred.Name = "txtMarred";
            this.txtMarred.Size = new System.Drawing.Size(97, 24);
            this.txtMarred.TabIndex = 27;
            // 
            // txtJob
            // 
            this.txtJob.Enabled = false;
            this.txtJob.Location = new System.Drawing.Point(280, 162);
            this.txtJob.Name = "txtJob";
            this.txtJob.Size = new System.Drawing.Size(97, 24);
            this.txtJob.TabIndex = 26;
            // 
            // lbimgCount
            // 
            this.lbimgCount.AutoSize = true;
            this.lbimgCount.Location = new System.Drawing.Point(213, 536);
            this.lbimgCount.Name = "lbimgCount";
            this.lbimgCount.Size = new System.Drawing.Size(36, 18);
            this.lbimgCount.TabIndex = 25;
            this.lbimgCount.Text = "0 / 0";
            // 
            // txtNote
            // 
            this.txtNote.Enabled = false;
            this.txtNote.Location = new System.Drawing.Point(84, 199);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(290, 62);
            this.txtNote.TabIndex = 20;
            // 
            // txtAge
            // 
            this.txtAge.Enabled = false;
            this.txtAge.Location = new System.Drawing.Point(277, 68);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(97, 24);
            this.txtAge.TabIndex = 17;
            // 
            // txtTelephone
            // 
            this.txtTelephone.Enabled = false;
            this.txtTelephone.Location = new System.Drawing.Point(83, 159);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(97, 24);
            this.txtTelephone.TabIndex = 15;
            // 
            // txtIdCode
            // 
            this.txtIdCode.Enabled = false;
            this.txtIdCode.Location = new System.Drawing.Point(84, 116);
            this.txtIdCode.Name = "txtIdCode";
            this.txtIdCode.Size = new System.Drawing.Size(97, 24);
            this.txtIdCode.TabIndex = 14;
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(83, 28);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(97, 24);
            this.txtName.TabIndex = 12;
            // 
            // btnNextImg
            // 
            this.btnNextImg.Image = ((System.Drawing.Image)(resources.GetObject("btnNextImg.Image")));
            this.btnNextImg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNextImg.Location = new System.Drawing.Point(283, 528);
            this.btnNextImg.Name = "btnNextImg";
            this.btnNextImg.Size = new System.Drawing.Size(98, 35);
            this.btnNextImg.TabIndex = 11;
            this.btnNextImg.Text = "  Sau";
            this.btnNextImg.UseVisualStyleBackColor = true;
            this.btnNextImg.Click += new System.EventHandler(this.btnNextImg_Click);
            // 
            // btnPrevImg
            // 
            this.btnPrevImg.Image = ((System.Drawing.Image)(resources.GetObject("btnPrevImg.Image")));
            this.btnPrevImg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrevImg.Location = new System.Drawing.Point(83, 528);
            this.btnPrevImg.Name = "btnPrevImg";
            this.btnPrevImg.Size = new System.Drawing.Size(98, 35);
            this.btnPrevImg.TabIndex = 10;
            this.btnPrevImg.Text = "   Trước";
            this.btnPrevImg.UseVisualStyleBackColor = true;
            this.btnPrevImg.Click += new System.EventHandler(this.btnPrevImg_Click);
            // 
            // pbPatient
            // 
            this.pbPatient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPatient.Location = new System.Drawing.Point(83, 279);
            this.pbPatient.Name = "pbPatient";
            this.pbPatient.Size = new System.Drawing.Size(291, 223);
            this.pbPatient.TabIndex = 9;
            this.pbPatient.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 202);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 18);
            this.label9.TabIndex = 8;
            this.label9.Text = "Ghi chú";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(187, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 18);
            this.label8.TabIndex = 7;
            this.label8.Text = "Nghề Nghiệp";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 165);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 18);
            this.label7.TabIndex = 6;
            this.label7.Text = "Số ĐT";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(187, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 18);
            this.label6.TabIndex = 5;
            this.label6.Text = "Ngày khám";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "CMTND";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(187, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tuổi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Hôn nhân";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ngày sinh";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(1121, 598);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(98, 35);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.button4_Click);
            // 
            // gbResult
            // 
            this.gbResult.Controls.Add(this.dgvPatient);
            this.gbResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbResult.Location = new System.Drawing.Point(12, 187);
            this.gbResult.Name = "gbResult";
            this.gbResult.Size = new System.Drawing.Size(811, 438);
            this.gbResult.TabIndex = 2;
            this.gbResult.TabStop = false;
            this.gbResult.Text = "Danh sách bệnh nhân";
            // 
            // dgvPatient
            // 
            this.dgvPatient.AllowUserToAddRows = false;
            this.dgvPatient.AllowUserToDeleteRows = false;
            this.dgvPatient.AllowUserToResizeRows = false;
            this.dgvPatient.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvPatient.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvPatient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPatient.Location = new System.Drawing.Point(3, 20);
            this.dgvPatient.MultiSelect = false;
            this.dgvPatient.Name = "dgvPatient";
            this.dgvPatient.ReadOnly = true;
            this.dgvPatient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPatient.Size = new System.Drawing.Size(805, 415);
            this.dgvPatient.TabIndex = 0;
            this.dgvPatient.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgPatient_CellClick);
            this.dgvPatient.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgPatient_DataBindingComplete);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(105, 65);
            this.txtPhone.MaxLength = 49;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(176, 24);
            this.txtPhone.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 18);
            this.label11.TabIndex = 5;
            this.label11.Text = "Số ĐT";
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 637);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gbResult);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Search";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search";
            this.UserIsAllowed += new System.EventHandler(this.Search_UserIsAllowed);
            this.UserIsDenied += new System.EventHandler(this.Search_UserIsDenied);
            this.Load += new System.EventHandler(this.Search_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPatient)).EndInit();
            this.gbResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pbPatient;
        private System.Windows.Forms.Button btnNextImg;
        private System.Windows.Forms.Button btnPrevImg;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTenSearch;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.TextBox txtIdCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.DateTimePicker dtNgayBatDau;
        private System.Windows.Forms.DateTimePicker dtNgayKetThuc;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnTatCa;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.GroupBox gbResult;
        private System.Windows.Forms.DataGridView dgvPatient;
        private System.Windows.Forms.RadioButton rbNgay;
        private System.Windows.Forms.RadioButton rbNam;
        private System.Windows.Forms.RadioButton rbThang;
        private System.Windows.Forms.Label lbimgCount;
        private System.Windows.Forms.TextBox txtJob;
        private System.Windows.Forms.TextBox txtBirthday;
        private System.Windows.Forms.TextBox txtCreatetime;
        private System.Windows.Forms.TextBox txtMarred;
        private System.Windows.Forms.Button btnDeleteSick;
        private System.Windows.Forms.Button btnReCheck;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label11;
    }
}