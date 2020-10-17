using System;

namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:T_Article
    /// </summary>
    [Serializable]
    public partial class T_Article
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
        private string _Title = "";
        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private long _CategoryID;
        /// <summary>
        /// CategoryID
        /// </summary>
        public long CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }
        private string _Tag = "";
        /// <summary>
        /// Tag
        /// </summary>
        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
        private string _Summary = "";
        /// <summary>
        /// Summary
        /// </summary>
        public string Summary
        {
            get { return _Summary; }
            set { _Summary = value; }
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
        private int _StatusValue;
        /// <summary>
        /// StatusValue
        /// </summary>
        public int StatusValue
        {
            get { return _StatusValue; }
            set { _StatusValue = value; }
        }
        private string _StatusRemark = "";
        /// <summary>
        /// StatusRemark
        /// </summary>
        public string StatusRemark
        {
            get { return _StatusRemark; }
            set { _StatusRemark = value; }
        }
        private DateTime _StatusChangeDateTime = DateTime.Now;
        /// <summary>
        /// StatusChangeDateTime
        /// </summary>
        public DateTime StatusChangeDateTime
        {
            get { return _StatusChangeDateTime; }
            set { _StatusChangeDateTime = value; }
        }
        private long _StatusChangeUserID;
        /// <summary>
        /// StatusChangeUserID
        /// </summary>
        public long StatusChangeUserID
        {
            get { return _StatusChangeUserID; }
            set { _StatusChangeUserID = value; }
        }
        #endregion

    }
}