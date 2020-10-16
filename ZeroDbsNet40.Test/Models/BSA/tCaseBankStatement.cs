using System;
using System.Collections.Generic;
using System.Text;
namespace Models.BSA
{
    /// <summary>
    /// 案子相关银行流水账单记录
    /// </summary>
    [Serializable]
    public partial class tCaseBankStatement
    {
        #region --标准字段--
        private long _ID;
        /// <summary>
        /// [主键][自增]账单编号
        /// </summary>
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private long _CaseID;
        /// <summary>
        /// 归属案子编号
        /// </summary>
        public long CaseID
        {
            get { return _CaseID; }
            set { _CaseID = value; }
        }
        private DateTime _BSDealDateTime;
        /// <summary>
        /// 账单交易日期时间
        /// </summary>
        public DateTime BSDealDateTime
        {
            get { return _BSDealDateTime; }
            set { _BSDealDateTime = value; }
        }
        private string _BSClientName = "";
        /// <summary>
        /// 账单客户名称
        /// </summary>
        public string BSClientName
        {
            get { return _BSClientName; }
            set { _BSClientName = value; }
        }
        private string _BSDealAccount = "";
        /// <summary>
        /// 账单交易帐(卡)号
        /// </summary>
        public string BSDealAccount
        {
            get { return _BSDealAccount; }
            set { _BSDealAccount = value; }
        }
        private string _BSDealDirection = "";
        /// <summary>
        /// 账单交易方向
        /// </summary>
        public string BSDealDirection
        {
            get { return _BSDealDirection; }
            set { _BSDealDirection = value; }
        }
        private string _BSCurrencyType = "";
        /// <summary>
        /// 账单交易币种
        /// </summary>
        public string BSCurrencyType
        {
            get { return _BSCurrencyType; }
            set { _BSCurrencyType = value; }
        }
        private decimal _BSDealMoney;
        /// <summary>
        /// 账单交易金额
        /// </summary>
        public decimal BSDealMoney
        {
            get { return _BSDealMoney; }
            set { _BSDealMoney = value; }
        }
        private string _BSDealSummary = "";
        /// <summary>
        /// 交易摘要
        /// </summary>
        public string BSDealSummary
        {
            get { return _BSDealSummary; }
            set { _BSDealSummary = value; }
        }
        private string _BSDealTextSummary = "";
        /// <summary>
        /// 交易文本摘要
        /// </summary>
        public string BSDealTextSummary
        {
            get { return _BSDealTextSummary; }
            set { _BSDealTextSummary = value; }
        }
        private decimal _BSDealAccountBalance;
        /// <summary>
        /// 交易账(卡)号余额
        /// </summary>
        public decimal BSDealAccountBalance
        {
            get { return _BSDealAccountBalance; }
            set { _BSDealAccountBalance = value; }
        }
        private string _BSPartnerAccount = "";
        /// <summary>
        /// 对手帐号
        /// </summary>
        public string BSPartnerAccount
        {
            get { return _BSPartnerAccount; }
            set { _BSPartnerAccount = value; }
        }
        private string _BSPartnerName = "";
        /// <summary>
        /// 对手名称
        /// </summary>
        public string BSPartnerName
        {
            get { return _BSPartnerName; }
            set { _BSPartnerName = value; }
        }
        private string _BSPartnerBank = "";
        /// <summary>
        /// 对手开户行
        /// </summary>
        public string BSPartnerBank
        {
            get { return _BSPartnerBank; }
            set { _BSPartnerBank = value; }
        }
        private DateTime _BSImportTime = DateTime.Now;
        /// <summary>
        /// 导入时间
        /// </summary>
        public DateTime BSImportTime
        {
            get { return _BSImportTime; }
            set { _BSImportTime = value; }
        }
        private string _FromFileName = "";
        /// <summary>
        /// 来自文件的文件名
        /// </summary>
        public string FromFileName
        {
            get { return _FromFileName; }
            set { _FromFileName = value; }
        }
        #endregion

    }
}