using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 优惠项目
    /// </summary>
    [Serializable]
    public partial class tPreferentialItems
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
        private string _PreferentialItem = "";
        /// <summary>
        /// 优惠项目
        /// </summary>
        public string PreferentialItem
        {
            get { return _PreferentialItem; }
            set { _PreferentialItem = value; }
        }
        private int _PreferentialMoney;
        /// <summary>
        /// 优惠项目优惠金额
        /// </summary>
        public int PreferentialMoney
        {
            get { return _PreferentialMoney; }
            set { _PreferentialMoney = value; }
        }
        #endregion

    }
}