using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:M_JudgePart
    /// </summary>
    [Serializable]
    public partial class tM_JudgePart
    {
        
        private int _ID;
        /// <summary>
        /// [PrimaryKey]ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _Category = "";
        /// <summary>
        /// 类别
        /// </summary>
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private string _Trademark = "";
        /// <summary>
        /// 牌号
        /// </summary>
        public string Trademark
        {
            get { return _Trademark; }
            set { _Trademark = value; }
        }
        private string _ItemType = "";
        /// <summary>
        /// ItemType
        /// </summary>
        public string ItemType
        {
            get { return _ItemType; }
            set { _ItemType = value; }
        }
        private string _ItemKey = "";
        /// <summary>
        /// 检查项Key(对应各个明细表的字段名称)
        /// </summary>
        public string ItemKey
        {
            get { return _ItemKey; }
            set { _ItemKey = value; }
        }
        private double _ItemValue;
        /// <summary>
        /// 检测项值(0值不参与评判)
        /// </summary>
        public double ItemValue
        {
            get { return _ItemValue; }
            set { _ItemValue = value; }
        }
        private string _ItemJudge = "";
        /// <summary>
        /// 检测项评判方法
        /// </summary>
        public string ItemJudge
        {
            get { return _ItemJudge; }
            set { _ItemJudge = value; }
        }
        private string _ItemRawName = "";
        /// <summary>
        /// 原始检测项名称(如果为空则视为与ItemKey一致)
        /// </summary>
        public string ItemRawName
        {
            get { return _ItemRawName; }
            set { _ItemRawName = value; }
        }

    }
}