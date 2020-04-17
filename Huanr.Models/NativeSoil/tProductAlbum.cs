using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_ProductAlbum
    /// </summary>
    [Serializable]
    public partial class tProductAlbum
    {
        #region --标准字段--
        private Guid _AlbumID;
        /// <summary>
        /// [主键]AlbumID
        /// </summary>
        public Guid AlbumID
        {
            get { return _AlbumID; }
            set { _AlbumID = value; }
        }
        private Guid _AlbumProductID;
        /// <summary>
        /// AlbumProductID
        /// </summary>
        public Guid AlbumProductID
        {
            get { return _AlbumProductID; }
            set { _AlbumProductID = value; }
        }
        private string _AlbumAlt = "";
        /// <summary>
        /// AlbumAlt
        /// </summary>
        public string AlbumAlt
        {
            get { return _AlbumAlt; }
            set { _AlbumAlt = value; }
        }
        private string _AlbumPath = "";
        /// <summary>
        /// AlbumPath
        /// </summary>
        public string AlbumPath
        {
            get { return _AlbumPath; }
            set { _AlbumPath = value; }
        }
        private DateTime _AlbumCreateTime;
        /// <summary>
        /// AlbumCreateTime
        /// </summary>
        public DateTime AlbumCreateTime
        {
            get { return _AlbumCreateTime; }
            set { _AlbumCreateTime = value; }
        }
        private int _AlbumSort;
        /// <summary>
        /// AlbumSort
        /// </summary>
        public int AlbumSort
        {
            get { return _AlbumSort; }
            set { _AlbumSort = value; }
        }
        private bool _AlbumDeleteStatus;
        /// <summary>
        /// AlbumDeleteStatus
        /// </summary>
        public bool AlbumDeleteStatus
        {
            get { return _AlbumDeleteStatus; }
            set { _AlbumDeleteStatus = value; }
        }
        #endregion

    }
}