using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// TABLE:SeatGrid
    /// </summary>
    [Serializable]
    public partial class tSeatGrid
    {
        #region --标准字段--
        private string _GridCode = "";
        /// <summary>
        /// [PrimaryKey]网格编号
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
        /// 车厢编号
        /// </summary>
        public int CompartmentCode
        {
            get { return _CompartmentCode; }
            set { _CompartmentCode = value; }
        }
        private int _SeatCode;
        /// <summary>
        /// 座位编号(非座位网格映射为0值)
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
        #endregion

    }
}