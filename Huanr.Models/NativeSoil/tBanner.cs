using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_Banner
    /// </summary>
    [Serializable]
    public partial class tBanner
    {
        #region --标准字段--
        private Guid _BannerID;
        /// <summary>
        /// [主键]BannerID
        /// </summary>
        public Guid BannerID
        {
            get { return _BannerID; }
            set { _BannerID = value; }
        }
        private int _BannerGroupCode;
        /// <summary>
        /// BannerGroupCode
        /// </summary>
        public int BannerGroupCode
        {
            get { return _BannerGroupCode; }
            set { _BannerGroupCode = value; }
        }
        private Guid _BannerCreateUserID;
        /// <summary>
        /// BannerCreateUserID
        /// </summary>
        public Guid BannerCreateUserID
        {
            get { return _BannerCreateUserID; }
            set { _BannerCreateUserID = value; }
        }
        private string _BannerName = "";
        /// <summary>
        /// BannerName
        /// </summary>
        public string BannerName
        {
            get { return _BannerName; }
            set { _BannerName = value; }
        }
        private string _BannerImagePath = "";
        /// <summary>
        /// BannerImagePath
        /// </summary>
        public string BannerImagePath
        {
            get { return _BannerImagePath; }
            set { _BannerImagePath = value; }
        }
        private string _BannerLinkUrl = "";
        /// <summary>
        /// BannerLinkUrl
        /// </summary>
        public string BannerLinkUrl
        {
            get { return _BannerLinkUrl; }
            set { _BannerLinkUrl = value; }
        }
        private string _BannerDescription = "";
        /// <summary>
        /// BannerDescription
        /// </summary>
        public string BannerDescription
        {
            get { return _BannerDescription; }
            set { _BannerDescription = value; }
        }
        private Guid _BannerModifyUserID;
        /// <summary>
        /// BannerModifyUserID
        /// </summary>
        public Guid BannerModifyUserID
        {
            get { return _BannerModifyUserID; }
            set { _BannerModifyUserID = value; }
        }
        private DateTime _BannerCreateTime;
        /// <summary>
        /// BannerCreateTime
        /// </summary>
        public DateTime BannerCreateTime
        {
            get { return _BannerCreateTime; }
            set { _BannerCreateTime = value; }
        }
        private int _BannerSort;
        /// <summary>
        /// BannerSort
        /// </summary>
        public int BannerSort
        {
            get { return _BannerSort; }
            set { _BannerSort = value; }
        }
        private bool _BannerDeleteStatus;
        /// <summary>
        /// BannerDeleteStatus
        /// </summary>
        public bool BannerDeleteStatus
        {
            get { return _BannerDeleteStatus; }
            set { _BannerDeleteStatus = value; }
        }
        #endregion

    }
}