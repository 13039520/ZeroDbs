using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 未知用途视图(老版本遗留)
    /// </summary>
    [Serializable]
    public partial class v17_5_16
    {
        #region --标准字段--
        private int _ID;
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _TrainType = "";
        /// <summary>
        /// TrainType
        /// </summary>
        public string TrainType
        {
            get { return _TrainType; }
            set { _TrainType = value; }
        }
        private DateTime _DepartureTime;
        /// <summary>
        /// DepartureTime
        /// </summary>
        public DateTime DepartureTime
        {
            get { return _DepartureTime; }
            set { _DepartureTime = value; }
        }
        private int? _TicketRemaining1;
        /// <summary>
        /// TicketRemaining1
        /// </summary>
        public int? TicketRemaining1
        {
            get { return _TicketRemaining1; }
            set { _TicketRemaining1 = value; }
        }
        private int? _TicketRemaining2;
        /// <summary>
        /// TicketRemaining2
        /// </summary>
        public int? TicketRemaining2
        {
            get { return _TicketRemaining2; }
            set { _TicketRemaining2 = value; }
        }
        private int? _TicketRemaining3;
        /// <summary>
        /// TicketRemaining3
        /// </summary>
        public int? TicketRemaining3
        {
            get { return _TicketRemaining3; }
            set { _TicketRemaining3 = value; }
        }
        private int? _TicketRemaining4;
        /// <summary>
        /// TicketRemaining4
        /// </summary>
        public int? TicketRemaining4
        {
            get { return _TicketRemaining4; }
            set { _TicketRemaining4 = value; }
        }
        private int? _TicketRemaining5;
        /// <summary>
        /// TicketRemaining5
        /// </summary>
        public int? TicketRemaining5
        {
            get { return _TicketRemaining5; }
            set { _TicketRemaining5 = value; }
        }
        private int? _TicketRemaining6;
        /// <summary>
        /// TicketRemaining6
        /// </summary>
        public int? TicketRemaining6
        {
            get { return _TicketRemaining6; }
            set { _TicketRemaining6 = value; }
        }
        private int? _YingSales;
        /// <summary>
        /// YingSales
        /// </summary>
        public int? YingSales
        {
            get { return _YingSales; }
            set { _YingSales = value; }
        }
        private int? _YingTicketRemaining;
        /// <summary>
        /// YingTicketRemaining
        /// </summary>
        public int? YingTicketRemaining
        {
            get { return _YingTicketRemaining; }
            set { _YingTicketRemaining = value; }
        }
        private int? _RuanSales;
        /// <summary>
        /// RuanSales
        /// </summary>
        public int? RuanSales
        {
            get { return _RuanSales; }
            set { _RuanSales = value; }
        }
        private int? _RuanTicketRemaining;
        /// <summary>
        /// RuanTicketRemaining
        /// </summary>
        public int? RuanTicketRemaining
        {
            get { return _RuanTicketRemaining; }
            set { _RuanTicketRemaining = value; }
        }
        private int? _TotalSales;
        /// <summary>
        /// TotalSales
        /// </summary>
        public int? TotalSales
        {
            get { return _TotalSales; }
            set { _TotalSales = value; }
        }
        private int? _TotalTicketRemaining;
        /// <summary>
        /// TotalTicketRemaining
        /// </summary>
        public int? TotalTicketRemaining
        {
            get { return _TotalTicketRemaining; }
            set { _TotalTicketRemaining = value; }
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
        #endregion

    }
}