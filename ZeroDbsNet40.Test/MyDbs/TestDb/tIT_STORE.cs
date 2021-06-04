using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:IT_STORE
    /// </summary>
    [Serializable]
    public partial class tIT_STORE
    {
        
        private string _FS_SH = "";
        /// <summary>
        /// 库存点代码
        /// </summary>
        public string FS_SH
        {
            get { return _FS_SH; }
            set { _FS_SH = value; }
        }
        private string _FS_MEMO;
        /// <summary>
        /// 库存点描述
        /// </summary>
        public string FS_MEMO
        {
            get { return _FS_MEMO; }
            set { _FS_MEMO = value; }
        }
        private string _FS_FACTORYNO;
        /// <summary>
        /// 工厂代码
        /// </summary>
        public string FS_FACTORYNO
        {
            get { return _FS_FACTORYNO; }
            set { _FS_FACTORYNO = value; }
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
        private string _FS_CUSTOMERERCODE;
        /// <summary>
        /// 客户代码
        /// </summary>
        public string FS_CUSTOMERERCODE
        {
            get { return _FS_CUSTOMERERCODE; }
            set { _FS_CUSTOMERERCODE = value; }
        }

    }
}