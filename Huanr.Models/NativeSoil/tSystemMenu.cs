using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SystemMenu
    /// </summary>
    [Serializable]
    public partial class tSystemMenu
    {
        #region --标准字段--
        private Guid _MenuID = System.Guid.NewGuid();
        /// <summary>
        /// [主键]MenuID
        /// </summary>
        public Guid MenuID
        {
            get { return _MenuID; }
            set { _MenuID = value; }
        }
        private Guid _GroupID;
        /// <summary>
        /// GroupID
        /// </summary>
        public Guid GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value; }
        }
        private string _MenuName = "";
        /// <summary>
        /// MenuName
        /// </summary>
        public string MenuName
        {
            get { return _MenuName; }
            set { _MenuName = value; }
        }
        private string _MenuLink = "";
        /// <summary>
        /// MenuLink
        /// </summary>
        public string MenuLink
        {
            get { return _MenuLink; }
            set { _MenuLink = value; }
        }
        private string _MenuRemark = "";
        /// <summary>
        /// MenuRemark
        /// </summary>
        public string MenuRemark
        {
            get { return _MenuRemark; }
            set { _MenuRemark = value; }
        }
        private int _MenuSort;
        /// <summary>
        /// MenuSort
        /// </summary>
        public int MenuSort
        {
            get { return _MenuSort; }
            set { _MenuSort = value; }
        }
        private DateTime _MenuCreateTime = DateTime.Now;
        /// <summary>
        /// MenuCreateTime
        /// </summary>
        public DateTime MenuCreateTime
        {
            get { return _MenuCreateTime; }
            set { _MenuCreateTime = value; }
        }
        private bool _MenuDeleteStatus;
        /// <summary>
        /// MenuDeleteStatus
        /// </summary>
        public bool MenuDeleteStatus
        {
            get { return _MenuDeleteStatus; }
            set { _MenuDeleteStatus = value; }
        }
        #endregion

    }
}