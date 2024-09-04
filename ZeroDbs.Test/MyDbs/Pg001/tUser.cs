// Version 1.0.0
// Date : 2025-11-19

namespace MyDbs.Pg001
{
    using System;
    
    
    /// <summary>
    /// 用户表
    /// </summary>
    [Serializable()]
    public class tUser
    {
        
        #region -- Members --
        private int _ID;
        
        private string _Name;
        
        private string _Password;
        
        private System.Nullable<System.DateTime> _CreateTime;
        #endregion
        
        #region -- Properties --
        /// <summary>
        /// [Identity][PrimaryKey]int4(integer)
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
        /// text(text)
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
        /// text(text)
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
        /// timestamptz(timestamp with time zone)
        /// </summary>
        public System.Nullable<System.DateTime> CreateTime
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
