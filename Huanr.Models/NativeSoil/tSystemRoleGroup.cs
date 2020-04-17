using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SystemRoleGroup
    /// </summary>
    [Serializable]
    public partial class tSystemRoleGroup
    {
        #region --标准字段--
        private string _GroupName = "";
        /// <summary>
        /// [主键]GroupName
        /// </summary>
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
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
        private DateTime _GroupCreateTime = DateTime.Now;
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