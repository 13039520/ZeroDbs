using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 销售统计(老版本遗留，新2020版本未使用)
    /// </summary>
    [Serializable]
    public partial class vSaleStatistics
    {
        #region --标准字段--
        private string _Operator = "";
        /// <summary>
        /// Operator
        /// </summary>
        public string Operator
        {
            get { return _Operator; }
            set { _Operator = value; }
        }
        private string _SeatType = "";
        /// <summary>
        /// SeatType
        /// </summary>
        public string SeatType
        {
            get { return _SeatType; }
            set { _SeatType = value; }
        }
        private string _EndStation = "";
        /// <summary>
        /// EndStation
        /// </summary>
        public string EndStation
        {
            get { return _EndStation; }
            set { _EndStation = value; }
        }
        private string _PreferentialItem = "";
        /// <summary>
        /// PreferentialItem
        /// </summary>
        public string PreferentialItem
        {
            get { return _PreferentialItem; }
            set { _PreferentialItem = value; }
        }
        private int? _TicketCount;
        /// <summary>
        /// TicketCount
        /// </summary>
        public int? TicketCount
        {
            get { return _TicketCount; }
            set { _TicketCount = value; }
        }
        private int _UnitPrice;
        /// <summary>
        /// UnitPrice
        /// </summary>
        public int UnitPrice
        {
            get { return _UnitPrice; }
            set { _UnitPrice = value; }
        }
        private int? _Money;
        /// <summary>
        /// Money
        /// </summary>
        public int? Money
        {
            get { return _Money; }
            set { _Money = value; }
        }
        #endregion

    }
}