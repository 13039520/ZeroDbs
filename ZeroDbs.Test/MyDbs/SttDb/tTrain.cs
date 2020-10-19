﻿using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 车次
    /// </summary>
    [Serializable]
    public partial class tTrain
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
        private string _TrainType = "";
        /// <summary>
        /// 车次类别
        /// </summary>
        public string TrainType
        {
            get { return _TrainType; }
            set { _TrainType = value; }
        }
        private DateTime _DepartureTime;
        /// <summary>
        /// 发车时间
        /// </summary>
        public DateTime DepartureTime
        {
            get { return _DepartureTime; }
            set { _DepartureTime = value; }
        }
        private int? _TicketRemaining1 = 40;
        /// <summary>
        /// 余票1
        /// </summary>
        public int? TicketRemaining1
        {
            get { return _TicketRemaining1; }
            set { _TicketRemaining1 = value; }
        }
        private int? _TicketRemaining2 = 56;
        /// <summary>
        /// 余票2
        /// </summary>
        public int? TicketRemaining2
        {
            get { return _TicketRemaining2; }
            set { _TicketRemaining2 = value; }
        }
        private int? _TicketRemaining3 = 32;
        /// <summary>
        /// 余票3
        /// </summary>
        public int? TicketRemaining3
        {
            get { return _TicketRemaining3; }
            set { _TicketRemaining3 = value; }
        }
        private int? _TicketRemaining4 = 21;
        /// <summary>
        /// 余票4
        /// </summary>
        public int? TicketRemaining4
        {
            get { return _TicketRemaining4; }
            set { _TicketRemaining4 = value; }
        }
        private int? _TicketRemaining5 = 56;
        /// <summary>
        /// 余票5
        /// </summary>
        public int? TicketRemaining5
        {
            get { return _TicketRemaining5; }
            set { _TicketRemaining5 = value; }
        }
        private int? _TicketRemaining6 = 40;
        /// <summary>
        /// 余票6
        /// </summary>
        public int? TicketRemaining6
        {
            get { return _TicketRemaining6; }
            set { _TicketRemaining6 = value; }
        }
        private int? _YingSales = 0;
        /// <summary>
        /// 硬座销售
        /// </summary>
        public int? YingSales
        {
            get { return _YingSales; }
            set { _YingSales = value; }
        }
        private int? _YingTicketRemaining = 192;
        /// <summary>
        /// 硬座余票
        /// </summary>
        public int? YingTicketRemaining
        {
            get { return _YingTicketRemaining; }
            set { _YingTicketRemaining = value; }
        }
        private int? _RuanSales = 0;
        /// <summary>
        /// 软座销售
        /// </summary>
        public int? RuanSales
        {
            get { return _RuanSales; }
            set { _RuanSales = value; }
        }
        private int? _RuanTicketRemaining = 53;
        /// <summary>
        /// 软座余票
        /// </summary>
        public int? RuanTicketRemaining
        {
            get { return _RuanTicketRemaining; }
            set { _RuanTicketRemaining = value; }
        }
        private int? _ZhanSales = 0;
        /// <summary>
        /// 站票余票
        /// </summary>
        public int? ZhanSales
        {
            get { return _ZhanSales; }
            set { _ZhanSales = value; }
        }
        private int? _ZhanTicketRemaining = 0;
        /// <summary>
        /// 站票销售
        /// </summary>
        public int? ZhanTicketRemaining
        {
            get { return _ZhanTicketRemaining; }
            set { _ZhanTicketRemaining = value; }
        }
        private int? _TotalSales = 0;
        /// <summary>
        /// 销售合计
        /// </summary>
        public int? TotalSales
        {
            get { return _TotalSales; }
            set { _TotalSales = value; }
        }
        private int? _TotalTicketRemaining = 245;
        /// <summary>
        /// 余票合计
        /// </summary>
        public int? TotalTicketRemaining
        {
            get { return _TotalTicketRemaining; }
            set { _TotalTicketRemaining = value; }
        }
        private string _Remark;
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