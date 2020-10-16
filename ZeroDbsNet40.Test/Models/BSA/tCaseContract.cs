using System;
using System.Collections.Generic;
using System.Text;
namespace Models.BSA
{
    /// <summary>
    /// 案子相关合同记录
    /// </summary>
    [Serializable]
    public partial class tCaseContract
    {
        #region --标准字段--
        private long _CaseID;
        /// <summary>
        /// 合同隶属案子ID
        /// </summary>
        public long CaseID
        {
            get { return _CaseID; }
            set { _CaseID = value; }
        }
        private long _ID;
        /// <summary>
        /// [主键][自增]合同ID(系统内唯一标识)
        /// </summary>
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _CaseContractCode = "";
        /// <summary>
        /// 合同编号
        /// </summary>
        public string CaseContractCode
        {
            get { return _CaseContractCode; }
            set { _CaseContractCode = value; }
        }
        private string _CaseContractTitle = "";
        /// <summary>
        /// 合同标题名称
        /// </summary>
        public string CaseContractTitle
        {
            get { return _CaseContractTitle; }
            set { _CaseContractTitle = value; }
        }
        private string _CaseContractPartyA = "";
        /// <summary>
        /// 合同甲方
        /// </summary>
        public string CaseContractPartyA
        {
            get { return _CaseContractPartyA; }
            set { _CaseContractPartyA = value; }
        }
        private string _CaseContractPartyAIDNumber = "";
        /// <summary>
        /// 合同甲方证件号
        /// </summary>
        public string CaseContractPartyAIDNumber
        {
            get { return _CaseContractPartyAIDNumber; }
            set { _CaseContractPartyAIDNumber = value; }
        }
        private string _CaseContractPartyB = "";
        /// <summary>
        /// 合同乙方
        /// </summary>
        public string CaseContractPartyB
        {
            get { return _CaseContractPartyB; }
            set { _CaseContractPartyB = value; }
        }
        private string _CaseContractPartyBIDNumber = "";
        /// <summary>
        /// 合同乙方证件号
        /// </summary>
        public string CaseContractPartyBIDNumber
        {
            get { return _CaseContractPartyBIDNumber; }
            set { _CaseContractPartyBIDNumber = value; }
        }
        private DateTime _CaseContractSigningTime;
        /// <summary>
        /// 合同签订时间
        /// </summary>
        public DateTime CaseContractSigningTime
        {
            get { return _CaseContractSigningTime; }
            set { _CaseContractSigningTime = value; }
        }
        private DateTime _CaseContractEndTime;
        /// <summary>
        /// 合同到期时间
        /// </summary>
        public DateTime CaseContractEndTime
        {
            get { return _CaseContractEndTime; }
            set { _CaseContractEndTime = value; }
        }
        private decimal _CaseContractMoney;
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal CaseContractMoney
        {
            get { return _CaseContractMoney; }
            set { _CaseContractMoney = value; }
        }
        private string _FromFileName = "";
        /// <summary>
        /// 来自文件名称
        /// </summary>
        public string FromFileName
        {
            get { return _FromFileName; }
            set { _FromFileName = value; }
        }
        #endregion

    }
}