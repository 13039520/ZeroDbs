using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 票价
    /// </summary>
    [Serializable]
    public partial class tTicketPrice
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
        /// 票价编号
        /// </summary>
        public int Code
        {
            get { return _Code; }
            set { _Code = value; }
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
        private int _YingPrice;
        /// <summary>
        /// 硬座价
        /// </summary>
        public int YingPrice
        {
            get { return _YingPrice; }
            set { _YingPrice = value; }
        }
        private int _RuanPrice;
        /// <summary>
        /// 软座价
        /// </summary>
        public int RuanPrice
        {
            get { return _RuanPrice; }
            set { _RuanPrice = value; }
        }
        private int _ZhanPrice;
        /// <summary>
        /// 站票价
        /// </summary>
        public int ZhanPrice
        {
            get { return _ZhanPrice; }
            set { _ZhanPrice = value; }
        }
        private int _YingBusySeasonPrice;
        /// <summary>
        /// 硬座价_旺季
        /// </summary>
        public int YingBusySeasonPrice
        {
            get { return _YingBusySeasonPrice; }
            set { _YingBusySeasonPrice = value; }
        }
        private int _RuanBusySeasonPrice;
        /// <summary>
        /// 软座价_旺季
        /// </summary>
        public int RuanBusySeasonPrice
        {
            get { return _RuanBusySeasonPrice; }
            set { _RuanBusySeasonPrice = value; }
        }
        private int _ZhanBusySeasonPrice;
        /// <summary>
        /// 站票价_旺季
        /// </summary>
        public int ZhanBusySeasonPrice
        {
            get { return _ZhanBusySeasonPrice; }
            set { _ZhanBusySeasonPrice = value; }
        }
        #endregion

    }
}