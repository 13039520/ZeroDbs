using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:E_RawValues
    /// </summary>
    [Serializable]
    public partial class tE_RawValues
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
        private int _TagID;
        /// <summary>
        /// TagID
        /// </summary>
        public int TagID
        {
            get { return _TagID; }
            set { _TagID = value; }
        }
        private double _TagValue;
        /// <summary>
        /// TagValue
        /// </summary>
        public double TagValue
        {
            get { return _TagValue; }
            set { _TagValue = value; }
        }
        private DateTime _TagTime;
        /// <summary>
        /// TagTime
        /// </summary>
        public DateTime TagTime
        {
            get { return _TagTime; }
            set { _TagTime = value; }
        }
        private int _CreateID;
        /// <summary>
        /// CreateID
        /// </summary>
        public int CreateID
        {
            get { return _CreateID; }
            set { _CreateID = value; }
        }
        private string _Creator = "";
        /// <summary>
        /// Creator
        /// </summary>
        public string Creator
        {
            get { return _Creator; }
            set { _Creator = value; }
        }
        private DateTime _CreateDate;
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }
        private int _ModifyID;
        /// <summary>
        /// ModifyID
        /// </summary>
        public int ModifyID
        {
            get { return _ModifyID; }
            set { _ModifyID = value; }
        }
        private string _Modifier = "";
        /// <summary>
        /// Modifier
        /// </summary>
        public string Modifier
        {
            get { return _Modifier; }
            set { _Modifier = value; }
        }
        private DateTime _ModifyDate;
        /// <summary>
        /// ModifyDate
        /// </summary>
        public DateTime ModifyDate
        {
            get { return _ModifyDate; }
            set { _ModifyDate = value; }
        }

    }
}