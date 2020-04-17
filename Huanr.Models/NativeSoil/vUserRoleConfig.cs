using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// VIEW:V_UserRoleConfig
    /// </summary>
    [Serializable]
    public partial class vUserRoleConfig
    {
        #region --标准字段--
        private Guid _ConfigID;
        /// <summary>
        /// ConfigID
        /// </summary>
        public Guid ConfigID
        {
            get { return _ConfigID; }
            set { _ConfigID = value; }
        }
        private Guid _ConfigRoleID;
        /// <summary>
        /// ConfigRoleID
        /// </summary>
        public Guid ConfigRoleID
        {
            get { return _ConfigRoleID; }
            set { _ConfigRoleID = value; }
        }
        private string _RoleName = "";
        /// <summary>
        /// RoleName
        /// </summary>
        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
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
        private string _UserName = "";
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
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
        private DateTime _ConfigCreateTime;
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