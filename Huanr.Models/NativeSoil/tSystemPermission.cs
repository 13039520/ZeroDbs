using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SystemPermission
    /// </summary>
    [Serializable]
    public partial class tSystemPermission
    {
        #region --标准字段--
        private string _GroupName = "";
        /// <summary>
        /// GroupName
        /// </summary>
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }
        private string _PermissionName = "";
        /// <summary>
        /// [主键]PermissionName
        /// </summary>
        public string PermissionName
        {
            get { return _PermissionName; }
            set { _PermissionName = value; }
        }
        private string _PermissionRemark = "";
        /// <summary>
        /// PermissionRemark
        /// </summary>
        public string PermissionRemark
        {
            get { return _PermissionRemark; }
            set { _PermissionRemark = value; }
        }
        private int _PermissionSort;
        /// <summary>
        /// PermissionSort
        /// </summary>
        public int PermissionSort
        {
            get { return _PermissionSort; }
            set { _PermissionSort = value; }
        }
        private DateTime _PermissionCreateTime = DateTime.Now;
        /// <summary>
        /// PermissionCreateTime
        /// </summary>
        public DateTime PermissionCreateTime
        {
            get { return _PermissionCreateTime; }
            set { _PermissionCreateTime = value; }
        }
        private bool _PermissionDeleteStatus;
        /// <summary>
        /// PermissionDeleteStatus
        /// </summary>
        public bool PermissionDeleteStatus
        {
            get { return _PermissionDeleteStatus; }
            set { _PermissionDeleteStatus = value; }
        }
        #endregion

    }
}