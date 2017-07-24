using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class PatientDetail : Form
    {
        string num;
        public delegate void DoEvent(string username, string age, string address,
                                     string job, string telephone, string insuranceId,
                                     string sex, string causeCheck);
        public event DoEvent RefreshDgv;

        public PatientDetail(string num)
        {
            InitializeComponent();
            this.num = num;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //update patient to database
            string sex = "";
            if (rdMen.Checked)
                sex = "Nam";
            else
                sex = "Nữ";
            try
            {
                string query = "UPDATE SickData SET SickName = @SickName, Age = @Age, Address = @Address, Occupation = @Occupation, "
                                    + "Telephone = @Telephone, InsuranceId = @InsuranceId, Sex = @Sex, CauseCheck = @CauseCheck, "
                                    + " Createtime = @Createtime, SickHistoryNote = @SickHistoryNote"
                                    + " WHERE SickNum = '" + num + "';";
                SqlCommand command = new SqlCommand(query, DBConnection.Instance.sqlConn);
                command.Parameters.Add("@SickName", SqlDbType.NChar).Value = txtName.Text.Trim();
                command.Parameters.Add("@Age", SqlDbType.Int).Value = int.Parse(txtAge.Text.Trim());
                command.Parameters.Add("@Address", SqlDbType.NChar).Value = txtAddr.Text.Trim();
                command.Parameters.Add("@Occupation", SqlDbType.NChar).Value = txtJob.Text.Trim();
                command.Parameters.Add("@Telephone", SqlDbType.NChar).Value = txtTelephone.Text.Trim();
                command.Parameters.Add("@InsuranceId", SqlDbType.NChar).Value = txtInsuranceId.Text.Trim();
                command.Parameters.Add("@Sex", SqlDbType.NChar).Value = sex;
                command.Parameters.Add("@CauseCheck", SqlDbType.NChar).Value = txtCauseCheck.Text.Trim();
                command.Parameters.Add("@Createtime", SqlDbType.NChar).Value = dtCreateTime.Value;
                command.Parameters.Add("@SickHistoryNote", SqlDbType.NChar).Value = txtNote.Text.Trim();
                command.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Sửa dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau",
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }
            this.RefreshDgv(txtName.Text.Trim(), txtAge.Text.Trim(), txtAddr.Text.Trim(),
                            txtJob.Text.Trim(), txtTelephone.Text.Trim(), txtInsuranceId.Text.Trim(),
                            sex, txtCauseCheck.Text.Trim());
            this.Close();
        }

        private void fillPatientDetail(Patient patient)
        {
            txtPatientId.Text = patient.NumProperty;
            txtName.Text = patient.NameProperty.Trim();
            txtAge.Text = patient.AgeProperty.ToString();
            txtAddr.Text = patient.AddrProperty.Trim();
            txtJob.Text = patient.JobProperty;
            txtTelephone.Text = patient.TelephoneProperty.Trim();
            txtInsuranceId.Text = patient.InsuranceIdProperty.Trim();
            if (string.Equals(patient.SexProperty, "Nam"))
                rdMen.Checked = true;
            else
                rdWomen.Checked = true;
            txtCauseCheck.Text = patient.CauseCheckProperty;
            dtCreateTime.Value = patient.CreateTimeProperty;
            txtNote.Text = patient.NoteProperty.Trim();
        }

        private void PatientDetail_Load(object sender, EventArgs e)
        {
            Patient patient = new Patient().getPatientByNum(num);
            fillPatientDetail(patient);
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {
            if (txtAge.Text != null && txtAge.Text != "")
            {
                if (1 > int.Parse(txtAge.Text.Trim()) ||
               120 < int.Parse(txtAge.Text.Trim()))
                {
                    MessageBox.Show("Tuổi bệnh nhân phải nằm trong khoảng 1 đến 120", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtAge.Text = "";
                }
            }
        }
    }
}
