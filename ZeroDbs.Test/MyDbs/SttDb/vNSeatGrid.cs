using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 座位网格视图(新2020版本定义)
    /// </summary>
    [Serializable]
    public partial class vNSeatGrid
    {
        #region --标准字段--
        private string _GridCode = "";
        /// <summary>
        /// GridCode
        /// </summary>
        public string GridCode
        {
            get { return _GridCode; }
            set { _GridCode = value; }
        }
        private int _X;
        /// <summary>
        /// X
        /// </summary>
        public int X
        {
            get { return _X; }
            set { _X = value; }
        }
        private int _Y;
        /// <summary>
        /// Y
        /// </summary>
        public int Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        private int _W;
        /// <summary>
        /// W
        /// </summary>
        public int W
        {
            get { return _W; }
            set { _W = value; }
        }
        private int _H;
        /// <summary>
        /// H
        /// </summary>
        public int H
        {
            get { return _H; }
            set { _H = value; }
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
        private string _Remark = "";
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
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
        private int? _SeatID;
        /// <summary>
        /// SeatID
        /// </summary>
        public int? SeatID
        {
            get { return _SeatID; }
            set { _SeatID = value; }
        }
        #endregion

    }
}