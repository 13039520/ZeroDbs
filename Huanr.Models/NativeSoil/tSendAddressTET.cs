using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SendAddressTET
    /// </summary>
    [Serializable]
    public partial class tSendAddressTET
    {
        #region --标准字段--
        private Guid _TemplateID;
        /// <summary>
        /// [主键]TemplateID
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
        private string _TemplateExpressCompany = "";
        /// <summary>
        /// TemplateExpressCompany
        /// </summary>
        public string TemplateExpressCompany
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
        #endregion

    }
}