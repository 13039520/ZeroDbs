using System;
using System.Collections.Generic;
using System.Text;
namespace Models.BSA
{
    /// <summary>
    /// 案子
    /// </summary>
    [Serializable]
    public partial class tCase
    {
        #region --标准字段--
        private long _ID;
        /// <summary>
        /// [主键][自增]ID
        /// </summary>
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _CaseName = "";
        /// <summary>
        /// CaseName
        /// </summary>
        public string CaseName
        {
            get { return _CaseName; }
            set { _CaseName = value; }
        }
        private string _CaseBaseFileDir = "";
        /// <summary>
        /// CaseBaseFileDir
        /// </summary>
        public string CaseBaseFileDir
        {
            get { return _CaseBaseFileDir; }
            set { _CaseBaseFileDir = value; }
        }
        private DateTime _CaseImportTime = DateTime.Now;
        /// <summary>
        /// CaseImportTime
        /// </summary>
        public DateTime CaseImportTime
        {
            get { return _CaseImportTime; }
            set { _CaseImportTime = value; }
        }
        #endregion

    }
}