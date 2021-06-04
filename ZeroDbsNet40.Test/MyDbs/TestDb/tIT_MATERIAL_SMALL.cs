using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:IT_MATERIAL_SMALL
    /// </summary>
    [Serializable]
    public partial class tIT_MATERIAL_SMALL
    {
        
        private string _FS_SMALLCATEGORYCODE = "";
        /// <summary>
        /// 物料小类编码（XL00000001）
        /// </summary>
        public string FS_SMALLCATEGORYCODE
        {
            get { return _FS_SMALLCATEGORYCODE; }
            set { _FS_SMALLCATEGORYCODE = value; }
        }
        private string _FS_SMALLCATEGORYNAME;
        /// <summary>
        /// 小类名称
        /// </summary>
        public string FS_SMALLCATEGORYNAME
        {
            get { return _FS_SMALLCATEGORYNAME; }
            set { _FS_SMALLCATEGORYNAME = value; }
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
        private string _FS_BIGCATEGORYCODE;
        /// <summary>
        /// 物料大类编码
        /// </summary>
        public string FS_BIGCATEGORYCODE
        {
            get { return _FS_BIGCATEGORYCODE; }
            set { _FS_BIGCATEGORYCODE = value; }
        }
        private string _FS_USERROLES;
        /// <summary>
        /// 角色
        /// </summary>
        public string FS_USERROLES
        {
            get { return _FS_USERROLES; }
            set { _FS_USERROLES = value; }
        }

    }
}