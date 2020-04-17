using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_Product
    /// </summary>
    [Serializable]
    public partial class tProduct
    {
        #region --标准字段--
        private Guid _ProductID;
        /// <summary>
        /// [主键]ProductID
        /// </summary>
        public Guid ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }
        private string _ProductShapeID = "";
        /// <summary>
        /// ProductShapeID
        /// </summary>
        public string ProductShapeID
        {
            get { return _ProductShapeID; }
            set { _ProductShapeID = value; }
        }
        private string _ProductPackageID = "";
        /// <summary>
        /// ProductPackageID
        /// </summary>
        public string ProductPackageID
        {
            get { return _ProductPackageID; }
            set { _ProductPackageID = value; }
        }
        private string _ProductCategoryID = "";
        /// <summary>
        /// ProductCategoryID
        /// </summary>
        public string ProductCategoryID
        {
            get { return _ProductCategoryID; }
            set { _ProductCategoryID = value; }
        }
        private string _ProductUnitID = "";
        /// <summary>
        /// ProductUnitID
        /// </summary>
        public string ProductUnitID
        {
            get { return _ProductUnitID; }
            set { _ProductUnitID = value; }
        }
        private string _ProductOriginAreaID = "";
        /// <summary>
        /// ProductOriginAreaID
        /// </summary>
        public string ProductOriginAreaID
        {
            get { return _ProductOriginAreaID; }
            set { _ProductOriginAreaID = value; }
        }
        private string _ProductName = "";
        /// <summary>
        /// ProductName
        /// </summary>
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }
        private decimal _ProductPrice;
        /// <summary>
        /// ProductPrice
        /// </summary>
        public decimal ProductPrice
        {
            get { return _ProductPrice; }
            set { _ProductPrice = value; }
        }
        private decimal _ProductSingleWeight;
        /// <summary>
        /// ProductSingleWeight
        /// </summary>
        public decimal ProductSingleWeight
        {
            get { return _ProductSingleWeight; }
            set { _ProductSingleWeight = value; }
        }
        private long _ProductStock;
        /// <summary>
        /// ProductStock
        /// </summary>
        public long ProductStock
        {
            get { return _ProductStock; }
            set { _ProductStock = value; }
        }
        private string _ProductSummary = "";
        /// <summary>
        /// ProductSummary
        /// </summary>
        public string ProductSummary
        {
            get { return _ProductSummary; }
            set { _ProductSummary = value; }
        }
        private string _ProductContent = "";
        /// <summary>
        /// ProductContent
        /// </summary>
        public string ProductContent
        {
            get { return _ProductContent; }
            set { _ProductContent = value; }
        }
        private string _ProductCoverImg = "";
        /// <summary>
        /// ProductCoverImg
        /// </summary>
        public string ProductCoverImg
        {
            get { return _ProductCoverImg; }
            set { _ProductCoverImg = value; }
        }
        private int _ProductClick;
        /// <summary>
        /// ProductClick
        /// </summary>
        public int ProductClick
        {
            get { return _ProductClick; }
            set { _ProductClick = value; }
        }
        private int _ProductStatus;
        /// <summary>
        /// ProductStatus
        /// </summary>
        public int ProductStatus
        {
            get { return _ProductStatus; }
            set { _ProductStatus = value; }
        }
        private bool _ProductAdultsOnly;
        /// <summary>
        /// ProductAdultsOnly
        /// </summary>
        public bool ProductAdultsOnly
        {
            get { return _ProductAdultsOnly; }
            set { _ProductAdultsOnly = value; }
        }
        private string _ProductTag = "";
        /// <summary>
        /// ProductTag
        /// </summary>
        public string ProductTag
        {
            get { return _ProductTag; }
            set { _ProductTag = value; }
        }
        private bool _ProductLock;
        /// <summary>
        /// ProductLock
        /// </summary>
        public bool ProductLock
        {
            get { return _ProductLock; }
            set { _ProductLock = value; }
        }
        private DateTime _ProductLockTime;
        /// <summary>
        /// ProductLockTime
        /// </summary>
        public DateTime ProductLockTime
        {
            get { return _ProductLockTime; }
            set { _ProductLockTime = value; }
        }
        private Guid _ProductLockUserID;
        /// <summary>
        /// ProductLockUserID
        /// </summary>
        public Guid ProductLockUserID
        {
            get { return _ProductLockUserID; }
            set { _ProductLockUserID = value; }
        }
        private string _ProductLockRemark = "";
        /// <summary>
        /// ProductLockRemark
        /// </summary>
        public string ProductLockRemark
        {
            get { return _ProductLockRemark; }
            set { _ProductLockRemark = value; }
        }
        private DateTime _ProductUnlockTime;
        /// <summary>
        /// ProductUnlockTime
        /// </summary>
        public DateTime ProductUnlockTime
        {
            get { return _ProductUnlockTime; }
            set { _ProductUnlockTime = value; }
        }
        private Guid _ProductUnlockUserID;
        /// <summary>
        /// ProductUnlockUserID
        /// </summary>
        public Guid ProductUnlockUserID
        {
            get { return _ProductUnlockUserID; }
            set { _ProductUnlockUserID = value; }
        }
        private string _ProductUnlockRemark = "";
        /// <summary>
        /// ProductUnlockRemark
        /// </summary>
        public string ProductUnlockRemark
        {
            get { return _ProductUnlockRemark; }
            set { _ProductUnlockRemark = value; }
        }
        private DateTime _ProductRefreshTime;
        /// <summary>
        /// ProductRefreshTime
        /// </summary>
        public DateTime ProductRefreshTime
        {
            get { return _ProductRefreshTime; }
            set { _ProductRefreshTime = value; }
        }
        private Guid _ProductCreateOrgID;
        /// <summary>
        /// ProductCreateOrgID
        /// </summary>
        public Guid ProductCreateOrgID
        {
            get { return _ProductCreateOrgID; }
            set { _ProductCreateOrgID = value; }
        }
        private Guid _ProductCreateUserID;
        /// <summary>
        /// ProductCreateUserID
        /// </summary>
        public Guid ProductCreateUserID
        {
            get { return _ProductCreateUserID; }
            set { _ProductCreateUserID = value; }
        }
        private DateTime _ProductCreateTime;
        /// <summary>
        /// ProductCreateTime
        /// </summary>
        public DateTime ProductCreateTime
        {
            get { return _ProductCreateTime; }
            set { _ProductCreateTime = value; }
        }
        private int _ProductSort;
        /// <summary>
        /// ProductSort
        /// </summary>
        public int ProductSort
        {
            get { return _ProductSort; }
            set { _ProductSort = value; }
        }
        private bool _ProductDeleteStatus;
        /// <summary>
        /// ProductDeleteStatus
        /// </summary>
        public bool ProductDeleteStatus
        {
            get { return _ProductDeleteStatus; }
            set { _ProductDeleteStatus = value; }
        }
        #endregion

    }
}