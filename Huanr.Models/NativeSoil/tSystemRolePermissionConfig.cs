using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SystemRolePermissionConfig
    /// </summary>
    [Serializable]
    public partial class tSystemRolePermissionConfig
    {
        #region --标准字段--
        private Guid _ConfigID = System.Guid.NewGuid();
        /// <summary>
        /// [主键]ConfigID
        /// </summary>
        public Guid ConfigID
        {
            get { return _ConfigID; }
            set { _ConfigID = value; }
        }
        private string _ConfigRoleName = "";
        /// <summary>
        /// ConfigRoleName
        /// </summary>
        public string ConfigRoleName
        {
            get { return _ConfigRoleName; }
            set { _ConfigRoleName = value; }
        }
        private string _ConfigPermissionName = "";
        /// <summary>
        /// ConfigPermissionName
        /// </summary>
        public string ConfigPermissionName
        {
            get { return _ConfigPermissionName; }
            set { _ConfigPermissionName = value; }
        }
        private int _ConfigSort;
        /// <summary>
        /// ConfigSort
        /// </summary>
        public int ConfigSort
        {
            get { return _ConfigSort; }
            set { _ConfigSort = value; }
        }
        private DateTime _ConfigCreateTime = DateTime.Now;
        /// <summary>
        /// ConfigCreateTime
        /// </summary>
        public DateTime ConfigCreateTime
        {
            get { return _ConfigCreateTime; }
            set { _ConfigCreateTime = value; }
        }
        private bool _ConfigDeleteStatus;
        /// <summary>
        /// ConfigDeleteStatus
        /// </summary>
        public bool ConfigDeleteStatus
        {
            get { return _ConfigDeleteStatus; }
            set { _ConfigDeleteStatus = value; }
        }
        #endregion

    }
}