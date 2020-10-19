using System;

namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:T_ArticleComment
    /// </summary>
    [Serializable]
    public partial class tArticleComment
    {
        #region --标准字段--
        private long _ID;
        /// <summary>
        /// ID+[PrimaryKey]+[Identity]
        /// </summary>
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private long _ArticleID;
        /// <summary>
        /// ArticleID
        /// </summary>
        public long ArticleID
        {
            get { return _ArticleID; }
            set { _ArticleID = value; }
        }
        private string _Content = "";
        /// <summary>
        /// Content
        /// </summary>
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        private DateTime _CreateDateTime = DateTime.Now;
        /// <summary>
        /// CreateDateTime
        /// </summary>
        public DateTime CreateDateTime
        {
            get { return _CreateDateTime; }
            set { _CreateDateTime = value; }
        }
        private long _CreateUserID;
        /// <summary>
        /// CreateUserID
        /// </summary>
        public long CreateUserID
        {
            get { return _CreateUserID; }
            set { _CreateUserID = value; }
        }
        private int _Status;
        /// <summary>
        /// Status
        /// </summary>
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        #endregion


    }
}