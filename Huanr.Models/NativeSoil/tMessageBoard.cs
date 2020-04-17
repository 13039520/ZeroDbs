using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// 消息板
    /// </summary>
    [Serializable]
    public partial class tMessageBoard
    {
        #region --标准字段--
        private Guid _MessageID = System.Guid.NewGuid();
        /// <summary>
        /// [主键]消息ID
        /// </summary>
        public Guid MessageID
        {
            get { return _MessageID; }
            set { _MessageID = value; }
        }
        private Guid _MessageParentID;
        /// <summary>
        /// 消息父ID（无父ID为Guid初始值）
        /// </summary>
        public Guid MessageParentID
        {
            get { return _MessageParentID; }
            set { _MessageParentID = value; }
        }
        private Guid _MessageRecipientUserID;
        /// <summary>
        /// 消息接收者ID（无接收者为Guid初始值）
        /// </summary>
        public Guid MessageRecipientUserID
        {
            get { return _MessageRecipientUserID; }
            set { _MessageRecipientUserID = value; }
        }
        private Guid _MessageThemeID;
        /// <summary>
        /// 消息主题ID（来自T_BaseCategory）
        /// </summary>
        public Guid MessageThemeID
        {
            get { return _MessageThemeID; }
            set { _MessageThemeID = value; }
        }
        private string _MessageContent = "";
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent
        {
            get { return _MessageContent; }
            set { _MessageContent = value; }
        }
        private DateTime _MessageCreateTime = DateTime.Now;
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime MessageCreateTime
        {
            get { return _MessageCreateTime; }
            set { _MessageCreateTime = value; }
        }
        private Guid _MessageCreateUserID;
        /// <summary>
        /// 消息创建人ID（游客为Guid初始值）
        /// </summary>
        public Guid MessageCreateUserID
        {
            get { return _MessageCreateUserID; }
            set { _MessageCreateUserID = value; }
        }
        private string _MessageCreateIP = "";
        /// <summary>
        /// 消息创建人IP
        /// </summary>
        public string MessageCreateIP
        {
            get { return _MessageCreateIP; }
            set { _MessageCreateIP = value; }
        }
        private string _MessageCreateIPAddress = "";
        /// <summary>
        /// 消息创建人IP地址
        /// </summary>
        public string MessageCreateIPAddress
        {
            get { return _MessageCreateIPAddress; }
            set { _MessageCreateIPAddress = value; }
        }
        private int _MessageInterveneStatus;
        /// <summary>
        /// 消息干预枚举(0无需干预1待干预2已经干预)
        /// </summary>
        public int MessageInterveneStatus
        {
            get { return _MessageInterveneStatus; }
            set { _MessageInterveneStatus = value; }
        }
        private string _MessageInterveneRemark = "";
        /// <summary>
        /// 消息干预备注
        /// </summary>
        public string MessageInterveneRemark
        {
            get { return _MessageInterveneRemark; }
            set { _MessageInterveneRemark = value; }
        }
        private Guid _MessageInterveneUserID;
        /// <summary>
        /// 消息干预处理人ID
        /// </summary>
        public Guid MessageInterveneUserID
        {
            get { return _MessageInterveneUserID; }
            set { _MessageInterveneUserID = value; }
        }
        private bool _MessageDeleteStatus;
        /// <summary>
        /// 消息删除标识
        /// </summary>
        public bool MessageDeleteStatus
        {
            get { return _MessageDeleteStatus; }
            set { _MessageDeleteStatus = value; }
        }
        #endregion

    }
}