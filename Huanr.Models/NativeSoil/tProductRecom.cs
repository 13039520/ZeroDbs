using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_ProductRecom
    /// </summary>
    [Serializable]
    public partial class tProductRecom
    {
        #region --标准字段--
        private Guid _RecomID;
        /// <summary>
        /// [主键]RecomID
        /// </summary>
        public Guid RecomID
        {
            get { return _RecomID; }
            set { _RecomID = value; }
        }
        private Guid _RecomProductID;
        /// <summary>
        /// RecomProductID
        /// </summary>
        public Guid RecomProductID
        {
            get { return _RecomProductID; }
            set { _RecomProductID = value; }
        }
        private string _RecomProductName = "";
        /// <summary>
        /// RecomProductName
        /// </summary>
        public string RecomProductName
        {
            get { return _RecomProductName; }
            set { _RecomProductName = value; }
        }
        private Guid _RecomProductID2;
        /// <summary>
        /// RecomProductID2
        /// </summary>
        public Guid RecomProductID2
        {
            get { return _RecomProductID2; }
            set { _RecomProductID2 = value; }
        }
        private DateTime _RecomCreateTime;
        /// <summary>
        /// RecomCreateTime
        /// </summary>
        public DateTime RecomCreateTime
        {
            get { return _RecomCreateTime; }
            set { _RecomCreateTime = value; }
        }
        private int _RecomSort;
        /// <summary>
        /// RecomSort
        /// </summary>
        public int RecomSort
        {
            get { return _RecomSort; }
            set { _RecomSort = value; }
        }
        private bool _RecomDeleteStatus;
        /// <summary>
        /// RecomDeleteStatus
        /// </summary>
        public bool RecomDeleteStatus
        {
            get { return _RecomDeleteStatus; }
            set { _RecomDeleteStatus = value; }
        }
        #endregion

    }
}