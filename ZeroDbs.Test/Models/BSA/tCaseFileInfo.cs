using System;
using System.Collections.Generic;
using System.Text;
namespace Models.BSA
{
    /// <summary>
    /// TABLE:CaseFileInfo
    /// </summary>
    [Serializable]
    public partial class tCaseFileInfo
    {
        #region --标准字段--
        private long _CaseID;
        /// <summary>
        /// 隶属案子
        /// </summary>
        public long CaseID
        {
            get { return _CaseID; }
            set { _CaseID = value; }
        }
        private int _ID;
        /// <summary>
        /// [主键][自增]系统内文件编号
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _FileDirName = "";
        /// <summary>
        /// 文件目录名称（代表了文件的类型，如：基础资料，银行账单资料）
        /// </summary>
        public string FileDirName
        {
            get { return _FileDirName; }
            set { _FileDirName = value; }
        }
        private string _FileName = "";
        /// <summary>
        /// 文件名称(包含后缀名)
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        private bool _FileImportFlag;
        /// <summary>
        /// 文件导入标识(控制重复导入)
        /// </summary>
        public bool FileImportFlag
        {
            get { return _FileImportFlag; }
            set { _FileImportFlag = value; }
        }
        private string _FileImportStateRemark = "";
        /// <summary>
        /// 文件导入状态备注(成功或失败简述)
        /// </summary>
        public string FileImportStateRemark
        {
            get { return _FileImportStateRemark; }
            set { _FileImportStateRemark = value; }
        }
        #endregion

    }
}