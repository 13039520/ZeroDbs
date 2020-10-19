using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 销售类别
    /// </summary>
    [Serializable]
    public partial class tSaleType
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
        private string _SaleType = "";
        /// <summary>
        /// 销售类别
        /// </summary>
        public string SaleType
        {
            get { return _SaleType; }
            set { _SaleType = value; }
        }
        #endregion

    }
}