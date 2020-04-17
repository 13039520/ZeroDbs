using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// VIEW:V_SendAddressTET
    /// </summary>
    [Serializable]
    public partial class vSendAddressTET
    {
        #region --标准字段--
        private Guid _TemplateID;
        /// <summary>
        /// TemplateID
        /// </summary>
        public Guid TemplateID
        {
            get { return _TemplateID; }
            set { _TemplateID = value; }
        }
        private Guid _TemplateSendAddressID;
        /// <summary>
        /// TemplateSendAddressID
        /// </summary>
        public Guid TemplateSendAddressID
        {
            get { return _TemplateSendAddressID; }
            set { _TemplateSendAddressID = value; }
        }
        private Guid _TemplateCreateUserID;
        /// <summary>
        /// TemplateCreateUserID
        /// </summary>
        public Guid TemplateCreateUserID
        {
            get { return _TemplateCreateUserID; }
            set { _TemplateCreateUserID = value; }
        }
        private Guid _TemplateExpressCompany;
        /// <summary>
        /// TemplateExpressCompany
        /// </summary>
        public Guid TemplateExpressCompany
        {
            get { return _TemplateExpressCompany; }
            set { _TemplateExpressCompany = value; }
        }
        private string _TemplateName = "";
        /// <summary>
        /// TemplateName
        /// </summary>
        public string TemplateName
        {
            get { return _TemplateName; }
            set { _TemplateName = value; }
        }
        private Guid _TemplateModifyUserID;
        /// <summary>
        /// TemplateModifyUserID
        /// </summary>
        public Guid TemplateModifyUserID
        {
            get { return _TemplateModifyUserID; }
            set { _TemplateModifyUserID = value; }
        }
        private DateTime _TemplateCreateTime;
        /// <summary>
        /// TemplateCreateTime
        /// </summary>
        public DateTime TemplateCreateTime
        {
            get { return _TemplateCreateTime; }
            set { _TemplateCreateTime = value; }
        }
        private int _TemplateSort;
        /// <summary>
        /// TemplateSort
        /// </summary>
        public int TemplateSort
        {
            get { return _TemplateSort; }
            set { _TemplateSort = value; }
        }
        private bool _TemplateDeleteStatus;
        /// <summary>
        /// TemplateDeleteStatus
        /// </summary>
        public bool TemplateDeleteStatus
        {
            get { return _TemplateDeleteStatus; }
            set { _TemplateDeleteStatus = value; }
        }
        private Guid? _BID;
        /// <summary>
        /// BID
        /// </summary>
        public Guid? BID
        {
            get { return _BID; }
            set { _BID = value; }
        }
        private Guid? _BParentID;
        /// <summary>
        /// BParentID
        /// </summary>
        public Guid? BParentID
        {
            get { return _BParentID; }
            set { _BParentID = value; }
        }
        private string _BName;
        /// <summary>
        /// BName
        /// </summary>
        public string BName
        {
            get { return _BName; }
            set { _BName = value; }
        }
        private string _BAsname;
        /// <summary>
        /// BAsname
        /// </summary>
        public string BAsname
        {
            get { return _BAsname; }
            set { _BAsname = value; }
        }
        private DateTime? _BCreateTime;
        /// <summary>
        /// BCreateTime
        /// </summary>
        public DateTime? BCreateTime
        {
            get { return _BCreateTime; }
            set { _BCreateTime = value; }
        }
        private int? _BSort;
        /// <summary>
        /// BSort
        /// </summary>
        public int? BSort
        {
            get { return _BSort; }
            set { _BSort = value; }
        }
        private bool? _BDeleteStatus;
        /// <summary>
        /// BDeleteStatus
        /// </summary>
        public bool? BDeleteStatus
        {
            get { return _BDeleteStatus; }
            set { _BDeleteStatus = value; }
        }
        #endregion

    }
}