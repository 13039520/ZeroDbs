using System;
using System.Collections.Generic;
using System.Text;
namespace Models.Article
{
    /// <summary>
    /// TABLE:T_ArticleCategory
    /// </summary>
    [Serializable]
    public partial class tArticleCategory
    {
        #region --标准字段--
        private long _ID;
        /// <summary>
        /// [主键]ID
        /// </summary>
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _Name = "";
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private bool _IsDel = false;
        /// <summary>
        /// IsDel
        /// </summary>
        public bool IsDel
        {
            get { return _IsDel; }
            set { _IsDel = value; }
        }
        #endregion

    }
}