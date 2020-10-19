using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 站点
    /// </summary>
    [Serializable]
    public partial class tStation
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
        private string _Station = "";
        /// <summary>
        /// 站点
        /// </summary>
        public string Station
        {
            get { return _Station; }
            set { _Station = value; }
        }
        #endregion

    }
}