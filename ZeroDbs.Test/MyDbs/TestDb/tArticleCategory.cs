using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:T_ArticleCategory
    /// </summary>
    [Serializable]
    public partial class tArticleCategory
    {
        
        private long _ID;
        /// <summary>
        /// [PrimaryKey]ID
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
        private bool _IsDel;
        /// <summary>
        /// IsDel
        /// </summary>
        public bool IsDel
        {
            get { return _IsDel; }
            set { _IsDel = value; }
        }

    }
}