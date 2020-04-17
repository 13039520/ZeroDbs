using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_ArticleComment
    /// </summary>
    [Serializable]
    public partial class tArticleComment
    {
        #region --标准字段--
        private Guid _ArtCommentID;
        /// <summary>
        /// [主键]ArtCommentID
        /// </summary>
        public Guid ArtCommentID
        {
            get { return _ArtCommentID; }
            set { _ArtCommentID = value; }
        }
        private Guid _ArtCommentID2;
        /// <summary>
        /// ArtCommentID2
        /// </summary>
        public Guid ArtCommentID2
        {
            get { return _ArtCommentID2; }
            set { _ArtCommentID2 = value; }
        }
        private Guid _ArtCommentArtID;
        /// <summary>
        /// ArtCommentArtID
        /// </summary>
        public Guid ArtCommentArtID
        {
            get { return _ArtCommentArtID; }
            set { _ArtCommentArtID = value; }
        }
        private Guid _ArtCommentUserID;
        /// <summary>
        /// ArtCommentUserID
        /// </summary>
        public Guid ArtCommentUserID
        {
            get { return _ArtCommentUserID; }
            set { _ArtCommentUserID = value; }
        }
        private string _ArtComment = "";
        /// <summary>
        /// ArtComment
        /// </summary>
        public string ArtComment
        {
            get { return _ArtComment; }
            set { _ArtComment = value; }
        }
        private int _ArtCommentStatus;
        /// <summary>
        /// ArtCommentStatus
        /// </summary>
        public int ArtCommentStatus
        {
            get { return _ArtCommentStatus; }
            set { _ArtCommentStatus = value; }
        }
        private string _ArtCommentStatusRemark = "";
        /// <summary>
        /// ArtCommentStatusRemark
        /// </summary>
        public string ArtCommentStatusRemark
        {
            get { return _ArtCommentStatusRemark; }
            set { _ArtCommentStatusRemark = value; }
        }
        private Guid _ArtCommentStatusRemarkUserID;
        /// <summary>
        /// ArtCommentStatusRemarkUserID
        /// </summary>
        public Guid ArtCommentStatusRemarkUserID
        {
            get { return _ArtCommentStatusRemarkUserID; }
            set { _ArtCommentStatusRemarkUserID = value; }
        }
        private DateTime _ArtCommentStatusRemarkTime;
        /// <summary>
        /// ArtCommentStatusRemarkTime
        /// </summary>
        public DateTime ArtCommentStatusRemarkTime
        {
            get { return _ArtCommentStatusRemarkTime; }
            set { _ArtCommentStatusRemarkTime = value; }
        }
        private DateTime _ArtCommentCreateTime;
        /// <summary>
        /// ArtCommentCreateTime
        /// </summary>
        public DateTime ArtCommentCreateTime
        {
            get { return _ArtCommentCreateTime; }
            set { _ArtCommentCreateTime = value; }
        }
        private int _ArtCommentSort;
        /// <summary>
        /// ArtCommentSort
        /// </summary>
        public int ArtCommentSort
        {
            get { return _ArtCommentSort; }
            set { _ArtCommentSort = value; }
        }
        private bool _ArtCommentDeleteStatus;
        /// <summary>
        /// ArtCommentDeleteStatus
        /// </summary>
        public bool ArtCommentDeleteStatus
        {
            get { return _ArtCommentDeleteStatus; }
            set { _ArtCommentDeleteStatus = value; }
        }
        #endregion

    }
}