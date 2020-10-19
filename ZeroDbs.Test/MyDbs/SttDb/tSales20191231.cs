using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// TABLE:Sales20191231
    /// </summary>
    [Serializable]
    public partial class tSales20191231
    {
        #region --标准字段--
        private int _ID;
        /// <summary>
        /// [Identity][PrimaryKey]ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
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
        private int _SeatID;
        /// <summary>
        /// SeatID
        /// </summary>
        public int SeatID
        {
            get { return _SeatID; }
            set { _SeatID = value; }
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
        private string _StartStation = "";
        /// <summary>
        /// StartStation
        /// </summary>
        public string StartStation
        {
            get { return _StartStation; }
            set { _StartStation = value; }
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