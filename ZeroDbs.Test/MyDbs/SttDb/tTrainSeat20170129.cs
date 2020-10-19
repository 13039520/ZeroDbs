using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// TABLE:TrainSeat20170129
    /// </summary>
    [Serializable]
    public partial class tTrainSeat20170129
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
        private int _SeatStateID;
        /// <summary>
        /// SeatStateID
        /// </summary>
        public int SeatStateID
        {
            get { return _SeatStateID; }
            set { _SeatStateID = value; }
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
        private int _TicketPrice;
        /// <summary>
        /// TicketPrice
        /// </summary>
        public int TicketPrice
        {
            get { return _TicketPrice; }
            set { _TicketPrice = value; }
        }
        private string _TicketSeller;
        /// <summary>
        /// TicketSeller
        /// </summary>
        public string TicketSeller
        {
            get { return _TicketSeller; }
            set { _TicketSeller = value; }
        }
        private string _PreferentialItem;
        /// <summary>
        /// PreferentialItem
        /// </summary>
        public string PreferentialItem
        {
            get { return _PreferentialItem; }
            set { _PreferentialItem = value; }
        }
        #endregion

    }
}