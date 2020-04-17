using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SystemRole
    /// </summary>
    [Serializable]
    public partial class tSystemRole
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
        private string _RoleName = "";
        /// <summary>
        /// [主键]RoleName
        /// </summary>
        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }
        private string _RoleRemark = "";
        /// <summary>
        /// RoleRemark
        /// </summary>
        public string RoleRemark
        {
            get { return _RoleRemark; }
            set { _RoleRemark = value; }
        }
        private DateTime _RoleCreateTime;
        /// <summary>
        /// RoleCreateTime
        /// </summary>
        public DateTime RoleCreateTime
        {
            get { return _RoleCreateTime; }
            set { _RoleCreateTime = value; }
        }
        private int _RoleSort;
        /// <summary>
        /// RoleSort
        /// </summary>
        public int RoleSort
        {
            get { return _RoleSort; }
            set { _RoleSort = value; }
        }
        private bool _RoleDeleteStatus;
        /// <summary>
        /// RoleDeleteStatus
        /// </summary>
        public bool RoleDeleteStatus
        {
            get { return _RoleDeleteStatus; }
            set { _RoleDeleteStatus = value; }
        }
        #endregion

    }
}