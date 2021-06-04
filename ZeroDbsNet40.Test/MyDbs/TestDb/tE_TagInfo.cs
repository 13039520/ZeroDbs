using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:E_TagInfo
    /// </summary>
    [Serializable]
    public partial class tE_TagInfo
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
        private string _TagName = "";
        /// <summary>
        /// TagName
        /// </summary>
        public string TagName
        {
            get { return _TagName; }
            set { _TagName = value; }
        }
        private string _EnergyType;
        /// <summary>
        /// 能源类型
        /// </summary>
        public string EnergyType
        {
            get { return _EnergyType; }
            set { _EnergyType = value; }
        }
        private string _Rename;
        /// <summary>
        /// Rename
        /// </summary>
        public string Rename
        {
            get { return _Rename; }
            set { _Rename = value; }
        }
        private int? _TagStatus;
        /// <summary>
        /// TagStatus
        /// </summary>
        public int? TagStatus
        {
            get { return _TagStatus; }
            set { _TagStatus = value; }
        }
        private string _DbName;
        /// <summary>
        /// DbName
        /// </summary>
        public string DbName
        {
            get { return _DbName; }
            set { _DbName = value; }
        }
        private string _TableName;
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        private string _ColumnName;
        /// <summary>
        /// ColumnName
        /// </summary>
        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }
        private decimal? _Rate;
        /// <summary>
        /// Rate
        /// </summary>
        public decimal? Rate
        {
            get { return _Rate; }
            set { _Rate = value; }
        }
        private string _DataType;
        /// <summary>
        /// DataType
        /// </summary>
        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }
        private string _Remark;
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
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
        private string _RawSourceType;
        /// <summary>
        /// RawSourceType
        /// </summary>
        public string RawSourceType
        {
            get { return _RawSourceType; }
            set { _RawSourceType = value; }
        }
        private string _RawSourceUID;
        /// <summary>
        /// RawSourceUID
        /// </summary>
        public string RawSourceUID
        {
            get { return _RawSourceUID; }
            set { _RawSourceUID = value; }
        }
        private int? _WorkArea;
        /// <summary>
        /// WorkArea
        /// </summary>
        public int? WorkArea
        {
            get { return _WorkArea; }
            set { _WorkArea = value; }
        }

    }
}