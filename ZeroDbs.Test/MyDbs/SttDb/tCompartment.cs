using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 车厢
    /// </summary>
    [Serializable]
    public partial class tCompartment
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
        private int _Code;
        /// <summary>
        /// 车厢编号
        /// </summary>
        public int Code
        {
            get { return _Code; }
            set { _Code = value; }
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
        private int _TotalSeats;
        /// <summary>
        /// 座位总数
        /// </summary>
        public int TotalSeats
        {
            get { return _TotalSeats; }
            set { _TotalSeats = value; }
        }
        private string _Remark;
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
        #endregion

    }
}