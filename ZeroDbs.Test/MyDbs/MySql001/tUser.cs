// Version 1.0.0
// Date : 2024-05-31

namespace MyDbs.MySql001
{
    using System;
    
    
    /// <summary>
    /// 用户表
    /// </summary>
    [Serializable()]
    public class tUser
    {
        
        #region -- Members --
        private System.DateTime _CreateTime = DateTime.Now;
        
        private string _Email;
        
        private int _ID;
        
        private string _Name;
        
        private string _Password;
        #endregion
        
        #region -- Properties --
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
        /// [Identity][PrimaryKey]ID
        /// </summary>
        public int ID
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
        
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
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
        #endregion
    }
}
