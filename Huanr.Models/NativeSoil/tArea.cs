using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_Area
    /// </summary>
    [Serializable]
    public partial class tArea
    {
        #region --标准字段--
        private string _AreaCode = "";
        /// <summary>
        /// [主键]AreaCode
        /// </summary>
        public string AreaCode
        {
            get { return _AreaCode; }
            set { _AreaCode = value; }
        }
        private string _AreaParent = "";
        /// <summary>
        /// AreaParent
        /// </summary>
        public string AreaParent
        {
            get { return _AreaParent; }
            set { _AreaParent = value; }
        }
        private string _AreaPath = "";
        /// <summary>
        /// AreaPath
        /// </summary>
        public string AreaPath
        {
            get { return _AreaPath; }
            set { _AreaPath = value; }
        }
        private string _AreaName = "";
        /// <summary>
        /// AreaName
        /// </summary>
        public string AreaName
        {
            get { return _AreaName; }
            set { _AreaName = value; }
        }
        private string _AreaNameSuffix = "";
        /// <summary>
        /// AreaNameSuffix
        /// </summary>
        public string AreaNameSuffix
        {
            get { return _AreaNameSuffix; }
            set { _AreaNameSuffix = value; }
        }
        private bool _AreaDeleteStatus;
        /// <summary>
        /// AreaDeleteStatus
        /// </summary>
        public bool AreaDeleteStatus
        {
            get { return _AreaDeleteStatus; }
            set { _AreaDeleteStatus = value; }
        }
        private string _AreaRemark = "";
        /// <summary>
        /// AreaRemark
        /// </summary>
        public string AreaRemark
        {
            get { return _AreaRemark; }
            set { _AreaRemark = value; }
        }
        #endregion

    }
}