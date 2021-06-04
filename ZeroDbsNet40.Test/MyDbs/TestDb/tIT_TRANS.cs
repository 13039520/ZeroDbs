using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:IT_TRANS
    /// </summary>
    [Serializable]
    public partial class tIT_TRANS
    {
        
        private string _FS_CY = "";
        /// <summary>
        /// 承运单位编号
        /// </summary>
        public string FS_CY
        {
            get { return _FS_CY; }
            set { _FS_CY = value; }
        }
        private string _FS_TRANSNAME = "";
        /// <summary>
        /// 承运单位名称
        /// </summary>
        public string FS_TRANSNAME
        {
            get { return _FS_TRANSNAME; }
            set { _FS_TRANSNAME = value; }
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
        private string _FS_FROM;
        /// <summary>
        /// 来源
        /// </summary>
        public string FS_FROM
        {
            get { return _FS_FROM; }
            set { _FS_FROM = value; }
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

    }
}