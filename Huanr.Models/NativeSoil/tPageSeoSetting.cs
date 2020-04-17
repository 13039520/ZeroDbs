using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_PageSeoSetting
    /// </summary>
    [Serializable]
    public partial class tPageSeoSetting
    {
        #region --标准字段--
        private Guid _PageSeoGuid;
        /// <summary>
        /// [主键]PageSeoGuid
        /// </summary>
        public Guid PageSeoGuid
        {
            get { return _PageSeoGuid; }
            set { _PageSeoGuid = value; }
        }
        private string _PageSeoName = "";
        /// <summary>
        /// PageSeoName
        /// </summary>
        public string PageSeoName
        {
            get { return _PageSeoName; }
            set { _PageSeoName = value; }
        }
        private string _PageSeoPath = "";
        /// <summary>
        /// PageSeoPath
        /// </summary>
        public string PageSeoPath
        {
            get { return _PageSeoPath; }
            set { _PageSeoPath = value; }
        }
        private string _PageSeoTitle = "";
        /// <summary>
        /// PageSeoTitle
        /// </summary>
        public string PageSeoTitle
        {
            get { return _PageSeoTitle; }
            set { _PageSeoTitle = value; }
        }
        private string _PageSeoKeywords = "";
        /// <summary>
        /// PageSeoKeywords
        /// </summary>
        public string PageSeoKeywords
        {
            get { return _PageSeoKeywords; }
            set { _PageSeoKeywords = value; }
        }
        private string _PageSeoDescription = "";
        /// <summary>
        /// PageSeoDescription
        /// </summary>
        public string PageSeoDescription
        {
            get { return _PageSeoDescription; }
            set { _PageSeoDescription = value; }
        }
        private Guid _PageSeoCreateUserID;
        /// <summary>
        /// PageSeoCreateUserID
        /// </summary>
        public Guid PageSeoCreateUserID
        {
            get { return _PageSeoCreateUserID; }
            set { _PageSeoCreateUserID = value; }
        }
        private Guid _PageSeoModifyUserID;
        /// <summary>
        /// PageSeoModifyUserID
        /// </summary>
        public Guid PageSeoModifyUserID
        {
            get { return _PageSeoModifyUserID; }
            set { _PageSeoModifyUserID = value; }
        }
        private DateTime _PageSeoModifyTime;
        /// <summary>
        /// PageSeoModifyTime
        /// </summary>
        public DateTime PageSeoModifyTime
        {
            get { return _PageSeoModifyTime; }
            set { _PageSeoModifyTime = value; }
        }
        private DateTime _PageSeoCreateTime;
        /// <summary>
        /// PageSeoCreateTime
        /// </summary>
        public DateTime PageSeoCreateTime
        {
            get { return _PageSeoCreateTime; }
            set { _PageSeoCreateTime = value; }
        }
        private int _PageSeoSort;
        /// <summary>
        /// PageSeoSort
        /// </summary>
        public int PageSeoSort
        {
            get { return _PageSeoSort; }
            set { _PageSeoSort = value; }
        }
        private bool _PageSeoDeleteStatus;
        /// <summary>
        /// PageSeoDeleteStatus
        /// </summary>
        public bool PageSeoDeleteStatus
        {
            get { return _PageSeoDeleteStatus; }
            set { _PageSeoDeleteStatus = value; }
        }
        #endregion

    }
}