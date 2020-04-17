using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SendAddress
    /// </summary>
    [Serializable]
    public partial class tSendAddress
    {
        #region --标准字段--
        private Guid _AddrID;
        /// <summary>
        /// [主键]AddrID
        /// </summary>
        public Guid AddrID
        {
            get { return _AddrID; }
            set { _AddrID = value; }
        }
        private Guid _AddrOrganizationID;
        /// <summary>
        /// AddrOrganizationID
        /// </summary>
        public Guid AddrOrganizationID
        {
            get { return _AddrOrganizationID; }
            set { _AddrOrganizationID = value; }
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
        private string _AddrAreaProvinceID = "";
        /// <summary>
        /// AddrAreaProvinceID
        /// </summary>
        public string AddrAreaProvinceID
        {
            get { return _AddrAreaProvinceID; }
            set { _AddrAreaProvinceID = value; }
        }
        private string _AddrAreaID = "";
        /// <summary>
        /// AddrAreaID
        /// </summary>
        public string AddrAreaID
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
        #endregion

    }
}