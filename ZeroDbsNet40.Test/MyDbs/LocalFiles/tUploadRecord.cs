using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.LocalFiles
{
    /// <summary>
    /// TABLE:UploadRecord
    /// </summary>
    [Serializable]
    public partial class tUploadRecord
    {
        
        private long _ID;
        /// <summary>
        /// [Identity][PrimaryKey]INTEGER
        /// </summary>
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _FilePath = "";
        /// <summary>
        /// TEXT
        /// </summary>
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }
        private DateTime _FileCreationTime = DateTime.Now;
        /// <summary>
        /// DATETIME
        /// </summary>
        public DateTime FileCreationTime
        {
            get { return _FileCreationTime; }
            set { _FileCreationTime = value; }
        }
        private DateTime _FileLastWriteTime = DateTime.Now;
        /// <summary>
        /// DATETIME
        /// </summary>
        public DateTime FileLastWriteTime
        {
            get { return _FileLastWriteTime; }
            set { _FileLastWriteTime = value; }
        }
        private DateTime _FileUploadTime = DateTime.Now;
        /// <summary>
        /// DATETIME
        /// </summary>
        public DateTime FileUploadTime
        {
            get { return _FileUploadTime; }
            set { _FileUploadTime = value; }
        }
        private long _FileUploadStatus = 0L;
        /// <summary>
        /// INTEGER(枚举：-1文件已经不存在0待上传1已经上传)
        /// </summary>
        public long FileUploadStatus
        {
            get { return _FileUploadStatus; }
            set { _FileUploadStatus = value; }
        }
        private string _FileUploadStatusRemark;
        /// <summary>
        /// TEXT
        /// </summary>
        public string FileUploadStatusRemark
        {
            get { return _FileUploadStatusRemark; }
            set { _FileUploadStatusRemark = value; }
        }

    }
}