using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 车次座位
    /// </summary>
    [Serializable]
    public partial class tTrainSeat
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
        private int _SeatStateID = 0;
        /// <summary>
        /// 座位状态ID(0待售1已售)
        /// </summary>
        public int SeatStateID
        {
            get { return _SeatStateID; }
            set { _SeatStateID = value; }
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
        private int _TicketPrice = 0;
        /// <summary>
        /// 票价
        /// </summary>
        public int TicketPrice
        {
            get { return _TicketPrice; }
            set { _TicketPrice = value; }
        }
        private string _TicketSeller;
        /// <summary>
        /// 售票员
        /// </summary>
        public string TicketSeller
        {
            get { return _TicketSeller; }
            set { _TicketSeller = value; }
        }
        private string _PreferentialItem;
        /// <summary>
        /// 优惠项目
        /// </summary>
        public string PreferentialItem
        {
            get { return _PreferentialItem; }
            set { _PreferentialItem = value; }
        }
        #endregion

    }
}