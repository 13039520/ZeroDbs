using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:E_ConfigurationDetails
    /// </summary>
    [Serializable]
    public partial class tE_ConfigurationDetails
    {
        
        private int _DetailID;
        /// <summary>
        /// [Identity][PrimaryKey]DetailID
        /// </summary>
        public int DetailID
        {
            get { return _DetailID; }
            set { _DetailID = value; }
        }
        private int _ID;
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _TagName = "";
        /// <summary>
        /// TagName
        /// </summary>
        public string TagName
        {
            get { return _TagName; }
            set { _TagName = value; }
        }
        private int? _OrderNO;
        /// <summary>
        /// OrderNO
        /// </summary>
        public int? OrderNO
        {
            get { return _OrderNO; }
            set { _OrderNO = value; }
        }
        private int? _CreateID;
        /// <summary>
        /// CreateID
        /// </summary>
        public int? CreateID
        {
            get { return _CreateID; }
            set { _CreateID = value; }
        }
        private string _Creator;
        /// <summary>
        /// Creator
        /// </summary>
        public string Creator
        {
            get { return _Creator; }
            set { _Creator = value; }
        }
        private DateTime? _CreateDate;
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime? CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }
        private int? _ModifyID;
        /// <summary>
        /// ModifyID
        /// </summary>
        public int? ModifyID
        {
            get { return _ModifyID; }
            set { _ModifyID = value; }
        }
        private string _Modifier;
        /// <summary>
        /// Modifier
        /// </summary>
        public string Modifier
        {
            get { return _Modifier; }
            set { _Modifier = value; }
        }
        private DateTime? _ModifyDate;
        /// <summary>
        /// ModifyDate
        /// </summary>
        public DateTime? ModifyDate
        {
            get { return _ModifyDate; }
            set { _ModifyDate = value; }
        }
        private int? _TagID;
        /// <summary>
        /// TagID
        /// </summary>
        public int? TagID
        {
            get { return _TagID; }
            set { _TagID = value; }
        }

    }
}