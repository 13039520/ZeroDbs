// Version 1.0.0
// Date : 2022-07-18

namespace MyDbs.SqlServer001
{
    using System;
    
    
    /// <summary>
    /// TABLE:User
    /// </summary>
    [Serializable()]
    public class tUser
    {
        
        #region -- Members --
        private int _ID;
        
        private string _Name;
        
        private string _Email;
        
        private string _Password;
        
        private System.DateTime _CreateTime = DateTime.Now;
        #endregion
        
        #region -- Properties --
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
        #endregion
    }
}
