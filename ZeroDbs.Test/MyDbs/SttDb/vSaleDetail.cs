using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 销售明细(老版本遗留，新2020版中未使用)
    /// </summary>
    [Serializable]
    public partial class vSaleDetail
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
        private DateTime _DepartureTime;
        /// <summary>
        /// DepartureTime
        /// </summary>
        public DateTime DepartureTime
        {
            get { return _DepartureTime; }
            set { _DepartureTime = value; }
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
        private int _CompartmentCode;
        /// <summary>
        /// CompartmentCode
        /// </summary>
        public int CompartmentCode
        {
            get { return _CompartmentCode; }
            set { _CompartmentCode = value; }
        }
        private int _SeatCode;
        /// <summary>
        /// SeatCode
        /// </summary>
        public int SeatCode
        {
            get { return _SeatCode; }
            set { _SeatCode = value; }
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
        private string _PreferentialItem = "";
        /// <summary>
        /// PreferentialItem
        /// </summary>
        public string PreferentialItem
        {
            get { return _PreferentialItem; }
            set { _PreferentialItem = value; }
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
        private int _Money;
        /// <summary>
        /// Money
        /// </summary>
        public int Money
        {
            get { return _Money; }
            set { _Money = value; }
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
        #endregion

    }
}