using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.Sqlite001
{
    /// <summary>
    /// TABLE:User
    /// </summary>
    [Serializable]
    public partial class tUser
    {
        
        private long _ID;
        /// <summary>
        /// [Identity][PrimaryKey]INTEGER
        /// </summary>
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _Name = "";
        /// <summary>
        /// VARCHAR (50)
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Email = "";
        /// <summary>
        /// VARCHAR (100)
        /// </summary>
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _Password = "";
        /// <summary>
        /// VARCHAR (100)
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private DateTime _CreateTime = DateTime.Now;
        /// <summary>
        /// DATETIME
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

    }
}