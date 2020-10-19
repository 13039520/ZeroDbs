using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 操作员类别
    /// </summary>
    [Serializable]
    public partial class tOperatorType
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
        private string _OperatorType = "";
        /// <summary>
        /// 操作员类别
        /// </summary>
        public string OperatorType
        {
            get { return _OperatorType; }
            set { _OperatorType = value; }
        }
        #endregion

    }
}