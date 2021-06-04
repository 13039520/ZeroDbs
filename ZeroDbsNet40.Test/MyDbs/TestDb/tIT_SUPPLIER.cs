using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:IT_SUPPLIER
    /// </summary>
    [Serializable]
    public partial class tIT_SUPPLIER
    {
        
        private string _FS_GY = "";
        /// <summary>
        /// 供应商代码
        /// </summary>
        public string FS_GY
        {
            get { return _FS_GY; }
            set { _FS_GY = value; }
        }
        private string _FS_SUPPLIERNAME = "";
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string FS_SUPPLIERNAME
        {
            get { return _FS_SUPPLIERNAME; }
            set { _FS_SUPPLIERNAME = value; }
        }
        private string _FS_FROM = "";
        /// <summary>
        /// 来源
        /// </summary>
        public string FS_FROM
        {
            get { return _FS_FROM; }
            set { _FS_FROM = value; }
        }
        private string _FS_HELPCODE;
        /// <summary>
        /// 拼音助记码
        /// </summary>
        public string FS_HELPCODE
        {
            get { return _FS_HELPCODE; }
            set { _FS_HELPCODE = value; }
        }
        private string _FS_SAPCODE;
        /// <summary>
        /// SAP代码
        /// </summary>
        public string FS_SAPCODE
        {
            get { return _FS_SAPCODE; }
            set { _FS_SAPCODE = value; }
        }
        private string _FS_USERROLES;
        /// <summary>
        /// 角色权限分配
        /// </summary>
        public string FS_USERROLES
        {
            get { return _FS_USERROLES; }
            set { _FS_USERROLES = value; }
        }
        private double? _FN_RECIPROCALWEIGHT;
        /// <summary>
        /// 是否参照对方重量:是为1；否为0。
        /// </summary>
        public double? FN_RECIPROCALWEIGHT
        {
            get { return _FN_RECIPROCALWEIGHT; }
            set { _FN_RECIPROCALWEIGHT = value; }
        }

    }
}