using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_Article
    /// </summary>
    [Serializable]
    public partial class tArticle
    {
        #region --标准字段--
        private Guid _ArticleID;
        /// <summary>
        /// [主键]ArticleID
        /// </summary>
        public Guid ArticleID
        {
            get { return _ArticleID; }
            set { _ArticleID = value; }
        }
        private Guid _ArticleBaseID;
        /// <summary>
        /// ArticleBaseID
        /// </summary>
        public Guid ArticleBaseID
        {
            get { return _ArticleBaseID; }
            set { _ArticleBaseID = value; }
        }
        private string _ArticleTitle = "";
        /// <summary>
        /// ArticleTitle
        /// </summary>
        public string ArticleTitle
        {
            get { return _ArticleTitle; }
            set { _ArticleTitle = value; }
        }
        private string _ArticleTitle2 = "";
        /// <summary>
        /// ArticleTitle2
        /// </summary>
        public string ArticleTitle2
        {
            get { return _ArticleTitle2; }
            set { _ArticleTitle2 = value; }
        }
        private string _ArticleSummary = "";
        /// <summary>
        /// ArticleSummary
        /// </summary>
        public string ArticleSummary
        {
            get { return _ArticleSummary; }
            set { _ArticleSummary = value; }
        }
        private string _ArticleTag = "";
        /// <summary>
        /// ArticleTag
        /// </summary>
        public string ArticleTag
        {
            get { return _ArticleTag; }
            set { _ArticleTag = value; }
        }
        private string _ArticlePageBackgroungPath = "";
        /// <summary>
        /// ArticlePageBackgroungPath
        /// </summary>
        public string ArticlePageBackgroungPath
        {
            get { return _ArticlePageBackgroungPath; }
            set { _ArticlePageBackgroungPath = value; }
        }
        private string _ArticleCoverImagePath = "";
        /// <summary>
        /// ArticleCoverImagePath
        /// </summary>
        public string ArticleCoverImagePath
        {
            get { return _ArticleCoverImagePath; }
            set { _ArticleCoverImagePath = value; }
        }
        private string _ArticleContent = "";
        /// <summary>
        /// ArticleContent
        /// </summary>
        public string ArticleContent
        {
            get { return _ArticleContent; }
            set { _ArticleContent = value; }
        }
        private int _ArticleClick;
        /// <summary>
        /// ArticleClick
        /// </summary>
        public int ArticleClick
        {
            get { return _ArticleClick; }
            set { _ArticleClick = value; }
        }
        private int _ArticleCommentOption;
        /// <summary>
        /// ArticleCommentOption
        /// </summary>
        public int ArticleCommentOption
        {
            get { return _ArticleCommentOption; }
            set { _ArticleCommentOption = value; }
        }
        private int _ArticleCommentLayers;
        /// <summary>
        /// ArticleCommentLayers
        /// </summary>
        public int ArticleCommentLayers
        {
            get { return _ArticleCommentLayers; }
            set { _ArticleCommentLayers = value; }
        }
        private int _ArticleStatus;
        /// <summary>
        /// ArticleStatus
        /// </summary>
        public int ArticleStatus
        {
            get { return _ArticleStatus; }
            set { _ArticleStatus = value; }
        }
        private bool _ArticleLock;
        /// <summary>
        /// ArticleLock
        /// </summary>
        public bool ArticleLock
        {
            get { return _ArticleLock; }
            set { _ArticleLock = value; }
        }
        private Guid _ArticleLockUserID;
        /// <summary>
        /// ArticleLockUserID
        /// </summary>
        public Guid ArticleLockUserID
        {
            get { return _ArticleLockUserID; }
            set { _ArticleLockUserID = value; }
        }
        private DateTime _ArticleLockTime;
        /// <summary>
        /// ArticleLockTime
        /// </summary>
        public DateTime ArticleLockTime
        {
            get { return _ArticleLockTime; }
            set { _ArticleLockTime = value; }
        }
        private string _ArticleLockRemark = "";
        /// <summary>
        /// ArticleLockRemark
        /// </summary>
        public string ArticleLockRemark
        {
            get { return _ArticleLockRemark; }
            set { _ArticleLockRemark = value; }
        }
        private DateTime _ArticleUnlockTime;
        /// <summary>
        /// ArticleUnlockTime
        /// </summary>
        public DateTime ArticleUnlockTime
        {
            get { return _ArticleUnlockTime; }
            set { _ArticleUnlockTime = value; }
        }
        private string _ArticleUnlockRemark = "";
        /// <summary>
        /// ArticleUnlockRemark
        /// </summary>
        public string ArticleUnlockRemark
        {
            get { return _ArticleUnlockRemark; }
            set { _ArticleUnlockRemark = value; }
        }
        private Guid _ArticleCreateUserID;
        /// <summary>
        /// ArticleCreateUserID
        /// </summary>
        public Guid ArticleCreateUserID
        {
            get { return _ArticleCreateUserID; }
            set { _ArticleCreateUserID = value; }
        }
        private DateTime _ArticleCreateTime;
        /// <summary>
        /// ArticleCreateTime
        /// </summary>
        public DateTime ArticleCreateTime
        {
            get { return _ArticleCreateTime; }
            set { _ArticleCreateTime = value; }
        }
        private int _ArticleSort;
        /// <summary>
        /// ArticleSort
        /// </summary>
        public int ArticleSort
        {
            get { return _ArticleSort; }
            set { _ArticleSort = value; }
        }
        private bool _ArticleDeleteStatus;
        /// <summary>
        /// ArticleDeleteStatus
        /// </summary>
        public bool ArticleDeleteStatus
        {
            get { return _ArticleDeleteStatus; }
            set { _ArticleDeleteStatus = value; }
        }
        #endregion

    }
}