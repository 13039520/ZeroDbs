using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 操作员
    /// </summary>
    [Serializable]
    public partial class tOperator
    {
        #region --标准字段--
        private int _ID;
        /// <summary>
        /// [PrimaryKey]ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _Code;
        /// <summary>
        /// 编号
        /// </summary>
        public int Code
        {
            get { return _Code; }
            set { _Code = value; }
        }
        private string _Name = "";
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private string _Permission;
        /// <summary>
        /// 权限
        /// </summary>
        public string Permission
        {
            get { return _Permission; }
            set { _Permission = value; }
        }
        private int _OperatorTypeID;
        /// <summary>
        /// 操作员类别ID
        /// </summary>
        public int OperatorTypeID
        {
            get { return _OperatorTypeID; }
            set { _OperatorTypeID = value; }
        }
        #endregion

    }
}