using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:BT_WEIGHTTYPE
    /// </summary>
    [Serializable]
    public partial class tBT_WEIGHTTYPE
    {
        
        private string _FS_TYPECODE = "";
        /// <summary>
        /// 流向编码
        /// </summary>
        public string FS_TYPECODE
        {
            get { return _FS_TYPECODE; }
            set { _FS_TYPECODE = value; }
        }
        private string _FS_TYPENAME = "";
        /// <summary>
        /// 流向名称
        /// </summary>
        public string FS_TYPENAME
        {
            get { return _FS_TYPENAME; }
            set { _FS_TYPENAME = value; }
        }
        private string _FS_MOVETYPE;
        /// <summary>
        /// 移动类型
        /// </summary>
        public string FS_MOVETYPE
        {
            get { return _FS_MOVETYPE; }
            set { _FS_MOVETYPE = value; }
        }
        private string _FS_STOREFLAG;
        /// <summary>
        /// 库存标志
        /// </summary>
        public string FS_STOREFLAG
        {
            get { return _FS_STOREFLAG; }
            set { _FS_STOREFLAG = value; }
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
        /// FS_FROM
        /// </summary>
        public string FS_FROM
        {
            get { return _FS_FROM; }
            set { _FS_FROM = value; }
        }
        private string _FS_MOVECODE;
        /// <summary>
        /// 移动代码
        /// </summary>
        public string FS_MOVECODE
        {
            get { return _FS_MOVECODE; }
            set { _FS_MOVECODE = value; }
        }

    }
}