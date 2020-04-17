using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_UserPermissionConfig
    /// </summary>
    [Serializable]
    public partial class tUserPermissionConfig
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
        private Guid _ConfigUserID;
        /// <summary>
        /// ConfigUserID
        /// </summary>
        public Guid ConfigUserID
        {
            get { return _ConfigUserID; }
            set { _ConfigUserID = value; }
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