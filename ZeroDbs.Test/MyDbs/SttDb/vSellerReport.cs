using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 售票员报表视图(老版本遗留，新2020版中未使用)
    /// </summary>
    [Serializable]
    public partial class vSellerReport
    {
        #region --标准字段--
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
        private int _TrainID;
        /// <summary>
        /// TrainID
        /// </summary>
        public int TrainID
        {
            get { return _TrainID; }
            set { _TrainID = value; }
        }
        #endregion

    }
}