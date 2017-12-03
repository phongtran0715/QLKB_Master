using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace PhanMemNoiSoi
{
    class Patient
    {
        string num;
        string name;
        int age;
        string sex;
        string note;
        string job;
        string addr;
        string idCode;
        string causeCheck;
        string telephone;
        string insuranceId;
        DateTime birthday;
        DateTime createTime;

        public Patient()
        {
            initData();
        }

        public Patient(string num, string name, int age, string telephone, string idCode,
                        string sex, DateTime birthday, DateTime createTime, string note,
                        string job, string causeCheck, string insuranceId)
        {
            this.num = num;
            this.name = name;
            this.age = age;
            this.telephone = telephone;
            this.idCode = idCode;
            this.sex = sex;
            this.birthday = birthday;
            this.createTime = createTime;
            this.note = note;
            this.job = job;
            this.causeCheck = causeCheck;
            this.insuranceId = insuranceId;
        }
        private void initData()
        {
            num = "";
            name = "";
            age = 0;
            telephone = "";
            idCode = "";
            sex = "";
            note = "";
            job = "";
            causeCheck = "";
            insuranceId = "";
            birthday = DateTime.MinValue;
            createTime = DateTime.MinValue;
        }

        public Patient getPatientByNum(string num)
        {
            Patient patient = new Patient();
            string query = "SELECT * FROM SickData WHERE SickNum = @num;";
            SqlCommand mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
            mySQL.Parameters.Add("@num", SqlDbType.NChar).Value = num;

            SqlDataReader rdrPatient = mySQL.ExecuteReader();
            while (rdrPatient.Read())
            {
                patient.NumProperty = num;
                patient.NameProperty = rdrPatient["SickName"].ToString().Trim();
                patient.AgeProperty = int.Parse(rdrPatient["Age"].ToString());
                patient.AddrProperty = rdrPatient["Address"].ToString().Trim();
                patient.JobProperty = rdrPatient["Occupation"].ToString().Trim();
                patient.TelephoneProperty = rdrPatient["Telephone"].ToString().Trim();
                patient.InsuranceIdProperty = rdrPatient["InsuranceId"].ToString().Trim();
                patient.SexProperty = rdrPatient["Sex"].ToString().Trim();
                patient.CauseCheckProperty = rdrPatient["CauseCheck"].ToString().Trim();
                patient.CreateTimeProperty = DateTime.Parse(rdrPatient["Createtime"].ToString());
                patient.NoteProperty = rdrPatient["SickHistoryNote"].ToString().Trim();
            }
            return patient;
        }

        public string getFolderPathByNum(string num)
        {
            Patient patient = getPatientByNum(num);
            string path = Properties.Settings.Default.imageFolder;
            path += @"\" +patient.createTime.Year.ToString() + patient.createTime.Month.ToString();
            path += @"\" + patient.createTime.Day.ToString();
            path += @"\" + num;
            return path;
        }

        public string getFolderPathByPatient(Patient patient)
        {
            string path = Properties.Settings.Default.imageFolder;
            path += @"\" + patient.createTime.Year.ToString() + patient.createTime.Month.ToString();
            path += @"\" + patient.createTime.Day.ToString();
            path += @"\" + patient.NumProperty + @"\";
            return path;
        }

        public string NumProperty
        {
            get { return num; }
            set { num = value; }
        }

        public string NameProperty
        {
            get { return name; }
            set { name = value; }
        }

        public int AgeProperty
        {
            get { return age; }
            set { age = value; }
        }

        public string TelephoneProperty
        {
            get { return telephone; }
            set { telephone = value; }
        }

        public string IdCodeProperty
        {
            get { return idCode; }
            set { idCode = value; }
        }

        public string SexProperty
        {
            get { return sex; }
            set { sex = value; }
        }

        public System.DateTime BirthdayProperty
        {
            get { return birthday; }
            set { birthday = value; }
        }

        public System.DateTime CreateTimeProperty
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public string NoteProperty
        {
            get { return note; }
            set { note = value; }
        }

        public string JobProperty
        {
            get { return job; }
            set { job = value; }
        }

        public string CauseCheckProperty
        {
            get { return causeCheck; }
            set { causeCheck = value; }
        }
        public string AddrProperty
        {
            get { return addr; }
            set { addr = value; }
        }

        public string InsuranceIdProperty
        {
            get { return insuranceId; }
            set { insuranceId = value; }
        }
    }
}
