using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// VIEW:V_ProductSendAddress
    /// </summary>
    [Serializable]
    public partial class vProductSendAddress
    {
        #region --标准字段--
        private Guid _ProPointID;
        /// <summary>
        /// ProPointID
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
        private Guid _AddrID;
        /// <summary>
        /// AddrID
        /// </summary>
        public Guid AddrID
        {
            get { return _AddrID; }
            set { _AddrID = value; }
        }
        private Guid _AddrUserID;
        /// <summary>
        /// AddrUserID
        /// </summary>
        public Guid AddrUserID
        {
            get { return _AddrUserID; }
            set { _AddrUserID = value; }
        }
        private string _AddrName = "";
        /// <summary>
        /// AddrName
        /// </summary>
        public string AddrName
        {
            get { return _AddrName; }
            set { _AddrName = value; }
        }
        private long _AddrAreaProvinceID;
        /// <summary>
        /// AddrAreaProvinceID
        /// </summary>
        public long AddrAreaProvinceID
        {
            get { return _AddrAreaProvinceID; }
            set { _AddrAreaProvinceID = value; }
        }
        private long _AddrAreaID;
        /// <summary>
        /// AddrAreaID
        /// </summary>
        public long AddrAreaID
        {
            get { return _AddrAreaID; }
            set { _AddrAreaID = value; }
        }
        private string _AddrAreaPath = "";
        /// <summary>
        /// AddrAreaPath
        /// </summary>
        public string AddrAreaPath
        {
            get { return _AddrAreaPath; }
            set { _AddrAreaPath = value; }
        }
        private string _AddrDetail = "";
        /// <summary>
        /// AddrDetail
        /// </summary>
        public string AddrDetail
        {
            get { return _AddrDetail; }
            set { _AddrDetail = value; }
        }
        private string _AddrTelephone = "";
        /// <summary>
        /// AddrTelephone
        /// </summary>
        public string AddrTelephone
        {
            get { return _AddrTelephone; }
            set { _AddrTelephone = value; }
        }
        private string _AddrContactName = "";
        /// <summary>
        /// AddrContactName
        /// </summary>
        public string AddrContactName
        {
            get { return _AddrContactName; }
            set { _AddrContactName = value; }
        }
        private string _AddrZipCode = "";
        /// <summary>
        /// AddrZipCode
        /// </summary>
        public string AddrZipCode
        {
            get { return _AddrZipCode; }
            set { _AddrZipCode = value; }
        }
        private DateTime _AddrCreateTime;
        /// <summary>
        /// AddrCreateTime
        /// </summary>
        public DateTime AddrCreateTime
        {
            get { return _AddrCreateTime; }
            set { _AddrCreateTime = value; }
        }
        private int _AddrSort;
        /// <summary>
        /// AddrSort
        /// </summary>
        public int AddrSort
        {
            get { return _AddrSort; }
            set { _AddrSort = value; }
        }
        private bool _AddrDeleteStatus;
        /// <summary>
        /// AddrDeleteStatus
        /// </summary>
        public bool AddrDeleteStatus
        {
            get { return _AddrDeleteStatus; }
            set { _AddrDeleteStatus = value; }
        }
        private string _AreaConcatName;
        /// <summary>
        /// AreaConcatName
        /// </summary>
        public string AreaConcatName
        {
            get { return _AreaConcatName; }
            set { _AreaConcatName = value; }
        }
        #endregion

    }
}