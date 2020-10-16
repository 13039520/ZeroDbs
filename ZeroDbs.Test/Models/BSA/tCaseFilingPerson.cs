using System;
using System.Collections.Generic;
using System.Text;
namespace Models.BSA
{
    /// <summary>
    /// 案子相关报案人记录
    /// </summary>
    [Serializable]
    public partial class tCaseFilingPerson
    {
        #region --标准字段--
        private long _CaseID;
        /// <summary>
        /// 隶属案子
        /// </summary>
        public long CaseID
        {
            get { return _CaseID; }
            set { _CaseID = value; }
        }
        private long _ID;
        /// <summary>
        /// [主键][自增]报案人编号
        /// </summary>
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _FilingPersonName = "";
        /// <summary>
        /// 报案人姓名
        /// </summary>
        public string FilingPersonName
        {
            get { return _FilingPersonName; }
            set { _FilingPersonName = value; }
        }
        private string _FilingPersonIDNumber = "";
        /// <summary>
        /// 报案人证件号
        /// </summary>
        public string FilingPersonIDNumber
        {
            get { return _FilingPersonIDNumber; }
            set { _FilingPersonIDNumber = value; }
        }
        private decimal _FilingPersonLossMoney;
        /// <summary>
        /// 报案损失金额
        /// </summary>
        public decimal FilingPersonLossMoney
        {
            get { return _FilingPersonLossMoney; }
            set { _FilingPersonLossMoney = value; }
        }
        private decimal _FilingPersonGetMoney;
        /// <summary>
        /// 报案返息金额
        /// </summary>
        public decimal FilingPersonGetMoney
        {
            get { return _FilingPersonGetMoney; }
            set { _FilingPersonGetMoney = value; }
        }
        private string _FromFileName = "";
        /// <summary>
        /// 来自文件
        /// </summary>
        public string FromFileName
        {
            get { return _FromFileName; }
            set { _FromFileName = value; }
        }
        #endregion

    }
}