using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PhanMemNoiSoi
{
    class RolesList
    {
        private static RolesList instance = null;
        private static readonly object padlock = new object();

        public const string ADD_NEW_USER            = "0";
        public const string MODIFY_USER             = "1";
        public const string DELETE_USER             = "2";
        public const string CHANGE_PASSWORD         = "3";
        public const string SETUP_CONFIGURATION     = "4";
        public const string VIEW_REPORT             = "5";
        public const string DELETE_PATIENT          = "6";
        public const string CHANGE_GLOSSARY         = "7";
        public const string BACKUP_RESTORE_DATA     = "8";
        public const string VIEW_LOG_HISTORY        = "9";

        public List<string> roleList = new List<string>()
        {
            ADD_NEW_USER,
            MODIFY_USER,
            DELETE_USER,
            CHANGE_PASSWORD,
            SETUP_CONFIGURATION,
            VIEW_REPORT,
            DELETE_PATIENT,
            CHANGE_GLOSSARY,
            BACKUP_RESTORE_DATA,
            VIEW_LOG_HISTORY
        };

        public static RolesList Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RolesList();
                    }
                    return instance;
                }
            }
        }

        public string [] getRoleByUser(string userGroup, int userId)
        {
            if(userId == 1)
            {
                return roleList.ToArray();
            }
            List<string> listUserRole = new List<string>();
            string sqlCommand = "SELECT RoleId FROM UserGroupRole WHERE GroupId = @id;";
            SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
            mySQL.Parameters.Add("@id", SqlDbType.NChar).Value = userGroup;
            SqlDataReader rdrUser = mySQL.ExecuteReader();
            if (rdrUser.HasRows)
            {
                while (rdrUser.Read())
                {
                    string roleId = rdrUser["RoleId"].ToString().Trim();
                    listUserRole.Add(roleId);
                }
            }
            return listUserRole.ToArray();
        }
    }
}
