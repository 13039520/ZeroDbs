using System;
using System.Collections.Generic;
using System.Text;
namespace Models.BSA
{
    /// <summary>
    /// 案子相关银行账单关键字段分组
    /// </summary>
    [Serializable]
    public partial class tCaseBankStatementGroup
    {
        #region --标准字段--
        private long _CaseID;
        /// <summary>
        /// CaseID
        /// </summary>
        public long CaseID
        {
            get { return _CaseID; }
            set { _CaseID = value; }
        }
        private string _GroupName = "";
        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }
        private string _GroupSubValue = "";
        /// <summary>
        /// 组子项值
        /// </summary>
        public string GroupSubValue
        {
            get { return _GroupSubValue; }
            set { _GroupSubValue = value; }
        }
        #endregion

    }
}