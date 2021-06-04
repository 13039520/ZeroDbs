using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:IT_MATERIAL
    /// </summary>
    [Serializable]
    public partial class tIT_MATERIAL
    {
        
        private string _FS_WL = "";
        /// <summary>
        /// 物料编号
        /// </summary>
        public string FS_WL
        {
            get { return _FS_WL; }
            set { _FS_WL = value; }
        }
        private string _FS_MATERIALNAME = "";
        /// <summary>
        /// 物料名称
        /// </summary>
        public string FS_MATERIALNAME
        {
            get { return _FS_MATERIALNAME; }
            set { _FS_MATERIALNAME = value; }
        }
        private string _FS_MATERIALTYPE;
        /// <summary>
        /// 物料类型
        /// </summary>
        public string FS_MATERIALTYPE
        {
            get { return _FS_MATERIALTYPE; }
            set { _FS_MATERIALTYPE = value; }
        }
        private string _FS_MATERIALGROUP;
        /// <summary>
        /// 物料组
        /// </summary>
        public string FS_MATERIALGROUP
        {
            get { return _FS_MATERIALGROUP; }
            set { _FS_MATERIALGROUP = value; }
        }
        private string _FS_WEIGHTUNIT;
        /// <summary>
        /// 计量单位
        /// </summary>
        public string FS_WEIGHTUNIT
        {
            get { return _FS_WEIGHTUNIT; }
            set { _FS_WEIGHTUNIT = value; }
        }
        private string _FS_GROUPDESCRIBE;
        /// <summary>
        /// 物料组描述
        /// </summary>
        public string FS_GROUPDESCRIBE
        {
            get { return _FS_GROUPDESCRIBE; }
            set { _FS_GROUPDESCRIBE = value; }
        }
        private string _FS_BATCHMANAGE;
        /// <summary>
        /// 是否批次管理
        /// </summary>
        public string FS_BATCHMANAGE
        {
            get { return _FS_BATCHMANAGE; }
            set { _FS_BATCHMANAGE = value; }
        }
        private string _FS_FROM = "";
        /// <summary>
        /// SAP-来自SAP MCMS- 计量系统创建
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
        private double? _FN_FACTOR;
        /// <summary>
        /// 皮带秤 F201 G101 特种物料系数
        /// </summary>
        public double? FN_FACTOR
        {
            get { return _FN_FACTOR; }
            set { _FN_FACTOR = value; }
        }
        private string _FS_SMALLCATEGORYCODE;
        /// <summary>
        /// 物料小类编号（外键）
        /// </summary>
        public string FS_SMALLCATEGORYCODE
        {
            get { return _FS_SMALLCATEGORYCODE; }
            set { _FS_SMALLCATEGORYCODE = value; }
        }

    }
}