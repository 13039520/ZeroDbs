// Version 1.0.0
// Date : 2025-11-19

namespace MyDbs.MySql001
{
    using System;
    
    
    /// <summary>
    /// 用户表(管理用途)
    /// </summary>
    [Serializable()]
    public class User
    {
        
        #region -- Members --
        private string _Account;

        private string _Password;

        private System.DateTime _CreateTime;
        
        private string _Email;
        
        private int _Gender;
        
        private long _ID;
        #endregion

        #region -- Properties --
        /// <summary>
        /// Account
        /// </summary>
        public string Account
        {
            get
            {
                return this._Account;
            }
            set
            {
                this._Account = value;
            }
        }

        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        /// <summary>
        /// CreateTime
        /// </summary>
        public System.DateTime CreateTime
        {
            get
            {
                return this._CreateTime;
            }
            set
            {
                this._CreateTime = value;
            }
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = value;
            }
        }
        
        /// <summary>
        /// Gender
        /// </summary>
        public int Gender
        {
            get
            {
                return this._Gender;
            }
            set
            {
                this._Gender = value;
            }
        }
        
        /// <summary>
        /// [Identity][PrimaryKey]
        /// </summary>
        public long ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
            }
        }
        #endregion
    }
}
