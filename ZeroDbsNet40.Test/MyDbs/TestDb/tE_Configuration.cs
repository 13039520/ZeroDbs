using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:E_Configuration
    /// </summary>
    [Serializable]
    public partial class tE_Configuration
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
        private string _AnalysisProject = "";
        /// <summary>
        /// AnalysisProject
        /// </summary>
        public string AnalysisProject
        {
            get { return _AnalysisProject; }
            set { _AnalysisProject = value; }
        }
        private string _EnergyType;
        /// <summary>
        /// EnergyType
        /// </summary>
        public string EnergyType
        {
            get { return _EnergyType; }
            set { _EnergyType = value; }
        }
        private string _ChartType;
        /// <summary>
        /// ChartType
        /// </summary>
        public string ChartType
        {
            get { return _ChartType; }
            set { _ChartType = value; }
        }
        private int? _IsTotal;
        /// <summary>
        /// IsTotal
        /// </summary>
        public int? IsTotal
        {
            get { return _IsTotal; }
            set { _IsTotal = value; }
        }
        private int? _IsShowDetails;
        /// <summary>
        /// IsShowDetails
        /// </summary>
        public int? IsShowDetails
        {
            get { return _IsShowDetails; }
            set { _IsShowDetails = value; }
        }
        private int? _IsShowTable;
        /// <summary>
        /// IsShowTable
        /// </summary>
        public int? IsShowTable
        {
            get { return _IsShowTable; }
            set { _IsShowTable = value; }
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
        private string _Remark;
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
        private int? _CfgStatus;
        /// <summary>
        /// CfgStatus
        /// </summary>
        public int? CfgStatus
        {
            get { return _CfgStatus; }
            set { _CfgStatus = value; }
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

    }
}