using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SystemMenuGroup
    /// </summary>
    [Serializable]
    public partial class tSystemMenuGroup
    {
        #region --标准字段--
        private Guid _GroupID;
        /// <summary>
        /// [主键]GroupID
        /// </summary>
        public Guid GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value; }
        }
        private string _GroupName = "";
        /// <summary>
        /// GroupName
        /// </summary>
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }
        private bool _GroupIsHidden;
        /// <summary>
        /// GroupIsHidden
        /// </summary>
        public bool GroupIsHidden
        {
            get { return _GroupIsHidden; }
            set { _GroupIsHidden = value; }
        }
        private string _GroupRemark = "";
        /// <summary>
        /// GroupRemark
        /// </summary>
        public string GroupRemark
        {
            get { return _GroupRemark; }
            set { _GroupRemark = value; }
        }
        private int _GroupSort;
        /// <summary>
        /// GroupSort
        /// </summary>
        public int GroupSort
        {
            get { return _GroupSort; }
            set { _GroupSort = value; }
        }
        private DateTime _GroupCreateTime;
        /// <summary>
        /// GroupCreateTime
        /// </summary>
        public DateTime GroupCreateTime
        {
            get { return _GroupCreateTime; }
            set { _GroupCreateTime = value; }
        }
        private bool _GroupDeleteStatus;
        /// <summary>
        /// GroupDeleteStatus
        /// </summary>
        public bool GroupDeleteStatus
        {
            get { return _GroupDeleteStatus; }
            set { _GroupDeleteStatus = value; }
        }
        #endregion

    }
}