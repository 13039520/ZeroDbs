using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_ProductSendAddress
    /// </summary>
    [Serializable]
    public partial class tProductSendAddress
    {
        #region --标准字段--
        private Guid _ProPointID;
        /// <summary>
        /// [主键]ProPointID
        /// </summary>
        public Guid ProPointID
        {
            get { return _ProPointID; }
            set { _ProPointID = value; }
        }
        private Guid _ProPointSendAddrID;
        /// <summary>
        /// ProPointSendAddrID
        /// </summary>
        public Guid ProPointSendAddrID
        {
            get { return _ProPointSendAddrID; }
            set { _ProPointSendAddrID = value; }
        }
        private Guid _ProPointProductID;
        /// <summary>
        /// ProPointProductID
        /// </summary>
        public Guid ProPointProductID
        {
            get { return _ProPointProductID; }
            set { _ProPointProductID = value; }
        }
        private DateTime _ProPointCreateTime;
        /// <summary>
        /// ProPointCreateTime
        /// </summary>
        public DateTime ProPointCreateTime
        {
            get { return _ProPointCreateTime; }
            set { _ProPointCreateTime = value; }
        }
        private int _ProPointSort;
        /// <summary>
        /// ProPointSort
        /// </summary>
        public int ProPointSort
        {
            get { return _ProPointSort; }
            set { _ProPointSort = value; }
        }
        private bool _ProPointDeleteStatus;
        /// <summary>
        /// ProPointDeleteStatus
        /// </summary>
        public bool ProPointDeleteStatus
        {
            get { return _ProPointDeleteStatus; }
            set { _ProPointDeleteStatus = value; }
        }
        #endregion

    }
}