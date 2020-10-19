using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 销售
    /// </summary>
    [Serializable]
    public partial class tSales
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
        /// 车次ID
        /// </summary>
        public int TrainID
        {
            get { return _TrainID; }
            set { _TrainID = value; }
        }
        private int _SeatID;
        /// <summary>
        /// 座位ID
        /// </summary>
        public int SeatID
        {
            get { return _SeatID; }
            set { _SeatID = value; }
        }
        private string _Operator = "";
        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator
        {
            get { return _Operator; }
            set { _Operator = value; }
        }
        private DateTime _CreateTime;
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        private string _StartStation = "";
        /// <summary>
        /// 起点站
        /// </summary>
        public string StartStation
        {
            get { return _StartStation; }
            set { _StartStation = value; }
        }
        private string _EndStation = "";
        /// <summary>
        /// 终点站
        /// </summary>
        public string EndStation
        {
            get { return _EndStation; }
            set { _EndStation = value; }
        }
        private string _SaleType = "";
        /// <summary>
        /// 销售类别
        /// </summary>
        public string SaleType
        {
            get { return _SaleType; }
            set { _SaleType = value; }
        }
        private int _Money;
        /// <summary>
        /// 金额
        /// </summary>
        public int Money
        {
            get { return _Money; }
            set { _Money = value; }
        }
        private int _SaleTypeCount;
        /// <summary>
        /// 销售类型计次(第N次销售或退票或补打)
        /// </summary>
        public int SaleTypeCount
        {
            get { return _SaleTypeCount; }
            set { _SaleTypeCount = value; }
        }
        #endregion

    }
}