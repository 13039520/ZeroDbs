using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 座位
    /// </summary>
    [Serializable]
    public partial class tSeat
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
        private int _CompartmentCode;
        /// <summary>
        /// 车厢编号
        /// </summary>
        public int CompartmentCode
        {
            get { return _CompartmentCode; }
            set { _CompartmentCode = value; }
        }
        private int _SeatCode;
        /// <summary>
        /// 座位编号
        /// </summary>
        public int SeatCode
        {
            get { return _SeatCode; }
            set { _SeatCode = value; }
        }
        private string _SeatType = "";
        /// <summary>
        /// 座位类别
        /// </summary>
        public string SeatType
        {
            get { return _SeatType; }
            set { _SeatType = value; }
        }
        #endregion

    }
}