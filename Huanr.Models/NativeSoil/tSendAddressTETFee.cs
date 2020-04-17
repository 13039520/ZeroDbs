using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_SendAddressTETFee
    /// </summary>
    [Serializable]
    public partial class tSendAddressTETFee
    {
        #region --标准字段--
        private Guid _FeeID;
        /// <summary>
        /// [主键]FeeID
        /// </summary>
        public Guid FeeID
        {
            get { return _FeeID; }
            set { _FeeID = value; }
        }
        private Guid _FeeTemplateID;
        /// <summary>
        /// FeeTemplateID
        /// </summary>
        public Guid FeeTemplateID
        {
            get { return _FeeTemplateID; }
            set { _FeeTemplateID = value; }
        }
        private Guid _FeeCreateUserID;
        /// <summary>
        /// FeeCreateUserID
        /// </summary>
        public Guid FeeCreateUserID
        {
            get { return _FeeCreateUserID; }
            set { _FeeCreateUserID = value; }
        }
        private string _FeeProvinceAreaID = "";
        /// <summary>
        /// FeeProvinceAreaID
        /// </summary>
        public string FeeProvinceAreaID
        {
            get { return _FeeProvinceAreaID; }
            set { _FeeProvinceAreaID = value; }
        }
        private decimal _FeeFirstWeightFee;
        /// <summary>
        /// FeeFirstWeightFee
        /// </summary>
        public decimal FeeFirstWeightFee
        {
            get { return _FeeFirstWeightFee; }
            set { _FeeFirstWeightFee = value; }
        }
        private decimal _FeeContinuedWeightFee;
        /// <summary>
        /// FeeContinuedWeightFee
        /// </summary>
        public decimal FeeContinuedWeightFee
        {
            get { return _FeeContinuedWeightFee; }
            set { _FeeContinuedWeightFee = value; }
        }
        private DateTime _FeeModifyTime;
        /// <summary>
        /// FeeModifyTime
        /// </summary>
        public DateTime FeeModifyTime
        {
            get { return _FeeModifyTime; }
            set { _FeeModifyTime = value; }
        }
        private Guid _FeeModifyUserID;
        /// <summary>
        /// FeeModifyUserID
        /// </summary>
        public Guid FeeModifyUserID
        {
            get { return _FeeModifyUserID; }
            set { _FeeModifyUserID = value; }
        }
        private DateTime _FeeCreateTime;
        /// <summary>
        /// FeeCreateTime
        /// </summary>
        public DateTime FeeCreateTime
        {
            get { return _FeeCreateTime; }
            set { _FeeCreateTime = value; }
        }
        private int _FeeSort;
        /// <summary>
        /// FeeSort
        /// </summary>
        public int FeeSort
        {
            get { return _FeeSort; }
            set { _FeeSort = value; }
        }
        private bool _FeeDeleteStatus;
        /// <summary>
        /// FeeDeleteStatus
        /// </summary>
        public bool FeeDeleteStatus
        {
            get { return _FeeDeleteStatus; }
            set { _FeeDeleteStatus = value; }
        }
        #endregion

    }
}