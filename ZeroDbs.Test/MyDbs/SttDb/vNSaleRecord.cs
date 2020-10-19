using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 销售记录视图(以Sales表作为基础，仅列出最终被销售出去的座位销售记录类型操作)
    /// </summary>
    [Serializable]
    public partial class vNSaleRecord
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
        private int? _TrainID;
        /// <summary>
        /// TrainID
        /// </summary>
        public int? TrainID
        {
            get { return _TrainID; }
            set { _TrainID = value; }
        }
        private string _TrainType;
        /// <summary>
        /// TrainType
        /// </summary>
        public string TrainType
        {
            get { return _TrainType; }
            set { _TrainType = value; }
        }
        private string _StartStation = "";
        /// <summary>
        /// StartStation
        /// </summary>
        public string StartStation
        {
            get { return _StartStation; }
            set { _StartStation = value; }
        }
        private DateTime? _DepartureTime;
        /// <summary>
        /// DepartureTime
        /// </summary>
        public DateTime? DepartureTime
        {
            get { return _DepartureTime; }
            set { _DepartureTime = value; }
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
        private int? _SeatID;
        /// <summary>
        /// SeatID
        /// </summary>
        public int? SeatID
        {
            get { return _SeatID; }
            set { _SeatID = value; }
        }
        private int? _SeatCode;
        /// <summary>
        /// SeatCode
        /// </summary>
        public int? SeatCode
        {
            get { return _SeatCode; }
            set { _SeatCode = value; }
        }
        private string _SeatType;
        /// <summary>
        /// SeatType
        /// </summary>
        public string SeatType
        {
            get { return _SeatType; }
            set { _SeatType = value; }
        }
        private int? _CompartmentCode;
        /// <summary>
        /// CompartmentCode
        /// </summary>
        public int? CompartmentCode
        {
            get { return _CompartmentCode; }
            set { _CompartmentCode = value; }
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
        private string _Operator = "";
        /// <summary>
        /// Operator
        /// </summary>
        public string Operator
        {
            get { return _Operator; }
            set { _Operator = value; }
        }
        private DateTime _CreateTime;
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        private string _SaleType = "";
        /// <summary>
        /// SaleType
        /// </summary>
        public string SaleType
        {
            get { return _SaleType; }
            set { _SaleType = value; }
        }
        private int _Money;
        /// <summary>
        /// Money
        /// </summary>
        public int Money
        {
            get { return _Money; }
            set { _Money = value; }
        }
        private int _SaleTypeCount;
        /// <summary>
        /// SaleTypeCount
        /// </summary>
        public int SaleTypeCount
        {
            get { return _SaleTypeCount; }
            set { _SaleTypeCount = value; }
        }
        #endregion

    }
}