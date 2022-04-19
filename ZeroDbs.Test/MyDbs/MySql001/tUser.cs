using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.MySql001
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public partial class tUser
    {
        
        private int _ID;
        /// <summary>
        /// [Identity][PrimaryKey]ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _Name = "";
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Email = "";
        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _Password = "";
        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private DateTime _CreateTime = DateTime.Now;
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

    }
}