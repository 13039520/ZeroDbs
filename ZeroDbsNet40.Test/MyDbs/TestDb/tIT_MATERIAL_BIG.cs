using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:IT_MATERIAL_BIG
    /// </summary>
    [Serializable]
    public partial class tIT_MATERIAL_BIG
    {
        
        private string _FS_BIGCATEGORYCODE = "";
        /// <summary>
        /// 物料大类编码（DL00000001）
        /// </summary>
        public string FS_BIGCATEGORYCODE
        {
            get { return _FS_BIGCATEGORYCODE; }
            set { _FS_BIGCATEGORYCODE = value; }
        }
        private string _FS_BIGCATEGORYNAME;
        /// <summary>
        /// 大类名称
        /// </summary>
        public string FS_BIGCATEGORYNAME
        {
            get { return _FS_BIGCATEGORYNAME; }
            set { _FS_BIGCATEGORYNAME = value; }
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
        private string _FS_REMARK;
        /// <summary>
        /// 备注
        /// </summary>
        public string FS_REMARK
        {
            get { return _FS_REMARK; }
            set { _FS_REMARK = value; }
        }

    }
}