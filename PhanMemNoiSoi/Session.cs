using System.Collections.Generic;

namespace PhanMemNoiSoi
{
    class Session
    {
        private string userName;
        private string password;
        private string uWorkGroup;
        private int userId;
        string [] userRole;
        private static Session instance = null;
        private bool isActiveLicense = false;
        private static readonly object padlock = new object();

        private Session()
        {
            this.UserName = "";
        }

        public static Session Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Session();
                    }
                    return instance;
                }
            }
        }

        public bool ActiveLicense
        {
            get
            {
                return isActiveLicense;
            }
            set
            {
                this.isActiveLicense = value;
            }
        }

        public string WorkGroup
        {
            get
            {
                return uWorkGroup;
            }

            set
            {
                uWorkGroup = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public int UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        public string[] UserRole
        {
            get
            {
                return userRole;
            }

            set
            {
                userRole = value;
            }
        }
    }
}
