// Version 1.0.0
// Date : 2025-11-19

namespace MyDbs.Sqlite001
{
    using System;
    
    
    /// <summary>
    /// TABLE:User
    /// </summary>
    [Serializable()]
    public class User
    {
        
        #region -- Members --
        private long _ID;
        
        private string _Name;
        
        private string _Email;
        
        private string _Password;
        
        private System.DateTime _CreateTime;
        #endregion
        
        #region -- Properties --
        /// <summary>
        /// [Identity][PrimaryKey]INTEGER
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
        
        /// <summary>
        /// VARCHAR (50)
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
        /// VARCHAR (100)
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
        /// VARCHAR (100)
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
        /// DATETIME
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
        #endregion
    }
}
