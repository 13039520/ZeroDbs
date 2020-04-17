using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_BaseCategory
    /// </summary>
    [Serializable]
    public partial class tBaseCategory
    {
        #region --标准字段--
        private string _BID = "";
        /// <summary>
        /// [主键]BID
        /// </summary>
        public string BID
        {
            get { return _BID; }
            set { _BID = value; }
        }
        private string _BParentID = "";
        /// <summary>
        /// BParentID
        /// </summary>
        public string BParentID
        {
            get { return _BParentID; }
            set { _BParentID = value; }
        }
        private string _BName = "";
        /// <summary>
        /// BName
        /// </summary>
        public string BName
        {
            get { return _BName; }
            set { _BName = value; }
        }
        private string _BAsname = "";
        /// <summary>
        /// BAsname
        /// </summary>
        public string BAsname
        {
            get { return _BAsname; }
            set { _BAsname = value; }
        }
        private string _BPath = "";
        /// <summary>
        /// BPath
        /// </summary>
        public string BPath
        {
            get { return _BPath; }
            set { _BPath = value; }
        }
        private DateTime _BCreateTime;
        /// <summary>
        /// BCreateTime
        /// </summary>
        public DateTime BCreateTime
        {
            get { return _BCreateTime; }
            set { _BCreateTime = value; }
        }
        private int _BSort;
        /// <summary>
        /// BSort
        /// </summary>
        public int BSort
        {
            get { return _BSort; }
            set { _BSort = value; }
        }
        private bool _BDeleteStatus;
        /// <summary>
        /// BDeleteStatus
        /// </summary>
        public bool BDeleteStatus
        {
            get { return _BDeleteStatus; }
            set { _BDeleteStatus = value; }
        }
        #endregion

    }
}