using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_ProductAttr
    /// </summary>
    [Serializable]
    public partial class tProductAttr
    {
        #region --标准字段--
        private Guid _AttrID;
        /// <summary>
        /// [主键]AttrID
        /// </summary>
        public Guid AttrID
        {
            get { return _AttrID; }
            set { _AttrID = value; }
        }
        private Guid _AttrProductID;
        /// <summary>
        /// AttrProductID
        /// </summary>
        public Guid AttrProductID
        {
            get { return _AttrProductID; }
            set { _AttrProductID = value; }
        }
        private string _AttrKey = "";
        /// <summary>
        /// AttrKey
        /// </summary>
        public string AttrKey
        {
            get { return _AttrKey; }
            set { _AttrKey = value; }
        }
        private string _AttrValue = "";
        /// <summary>
        /// AttrValue
        /// </summary>
        public string AttrValue
        {
            get { return _AttrValue; }
            set { _AttrValue = value; }
        }
        private DateTime _AttrCreateTime;
        /// <summary>
        /// AttrCreateTime
        /// </summary>
        public DateTime AttrCreateTime
        {
            get { return _AttrCreateTime; }
            set { _AttrCreateTime = value; }
        }
        private int _AttrSort;
        /// <summary>
        /// AttrSort
        /// </summary>
        public int AttrSort
        {
            get { return _AttrSort; }
            set { _AttrSort = value; }
        }
        private bool _AttrDeleteStatus;
        /// <summary>
        /// AttrDeleteStatus
        /// </summary>
        public bool AttrDeleteStatus
        {
            get { return _AttrDeleteStatus; }
            set { _AttrDeleteStatus = value; }
        }
        #endregion

    }
}