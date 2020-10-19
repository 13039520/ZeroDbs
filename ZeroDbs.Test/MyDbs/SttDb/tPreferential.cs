using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 优惠
    /// </summary>
    [Serializable]
    public partial class tPreferential
    {
        #region --标准字段--
        private int _ID;
        /// <summary>
        /// [PrimaryKey]ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _PreferentialItem = "";
        /// <summary>
        /// 优惠项目
        /// </summary>
        public string PreferentialItem
        {
            get { return _PreferentialItem; }
            set { _PreferentialItem = value; }
        }
        private int _PreferentialMoney;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public int PreferentialMoney
        {
            get { return _PreferentialMoney; }
            set { _PreferentialMoney = value; }
        }
        private int _PreferentialRate;
        /// <summary>
        /// 优惠比例
        /// </summary>
        public int PreferentialRate
        {
            get { return _PreferentialRate; }
            set { _PreferentialRate = value; }
        }
        private int _TicketPricePrintCtrl = 0;
        /// <summary>
        /// 票价打印控制(0不打印1仅票价2仅优惠项目3票价+优惠)
        /// </summary>
        public int TicketPricePrintCtrl
        {
            get { return _TicketPricePrintCtrl; }
            set { _TicketPricePrintCtrl = value; }
        }
        private string _Remark = "";
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
        #endregion

    }
}