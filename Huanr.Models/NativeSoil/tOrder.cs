using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_Order
    /// </summary>
    [Serializable]
    public partial class tOrder
    {
        #region --标准字段--
        private Guid _OrderID;
        /// <summary>
        /// [主键]OrderID
        /// </summary>
        public Guid OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }
        private Guid _OrderPlatform;
        /// <summary>
        /// OrderPlatform
        /// </summary>
        public Guid OrderPlatform
        {
            get { return _OrderPlatform; }
            set { _OrderPlatform = value; }
        }
        private Guid _OrderType;
        /// <summary>
        /// OrderType
        /// </summary>
        public Guid OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; }
        }
        private Guid _OrderPayPlatform;
        /// <summary>
        /// OrderPayPlatform
        /// </summary>
        public Guid OrderPayPlatform
        {
            get { return _OrderPayPlatform; }
            set { _OrderPayPlatform = value; }
        }
        private string _OrderPayPlatformRemark = "";
        /// <summary>
        /// OrderPayPlatformRemark
        /// </summary>
        public string OrderPayPlatformRemark
        {
            get { return _OrderPayPlatformRemark; }
            set { _OrderPayPlatformRemark = value; }
        }
        private string _OrderPayPlatformSerialNumber;
        /// <summary>
        /// OrderPayPlatformSerialNumber
        /// </summary>
        public string OrderPayPlatformSerialNumber
        {
            get { return _OrderPayPlatformSerialNumber; }
            set { _OrderPayPlatformSerialNumber = value; }
        }
        private string _OrderTitle = "";
        /// <summary>
        /// OrderTitle
        /// </summary>
        public string OrderTitle
        {
            get { return _OrderTitle; }
            set { _OrderTitle = value; }
        }
        private int _OrderStatus;
        /// <summary>
        /// OrderStatus
        /// </summary>
        public int OrderStatus
        {
            get { return _OrderStatus; }
            set { _OrderStatus = value; }
        }
        private string _OrderStatusRemark = "";
        /// <summary>
        /// OrderStatusRemark
        /// </summary>
        public string OrderStatusRemark
        {
            get { return _OrderStatusRemark; }
            set { _OrderStatusRemark = value; }
        }
        private Guid _OrderRecommendUserID;
        /// <summary>
        /// OrderRecommendUserID
        /// </summary>
        public Guid OrderRecommendUserID
        {
            get { return _OrderRecommendUserID; }
            set { _OrderRecommendUserID = value; }
        }
        private Guid _OrderProductID;
        /// <summary>
        /// OrderProductID
        /// </summary>
        public Guid OrderProductID
        {
            get { return _OrderProductID; }
            set { _OrderProductID = value; }
        }
        private string _OrderProductName = "";
        /// <summary>
        /// OrderProductName
        /// </summary>
        public string OrderProductName
        {
            get { return _OrderProductName; }
            set { _OrderProductName = value; }
        }
        private decimal _OrderProductPrice;
        /// <summary>
        /// OrderProductPrice
        /// </summary>
        public decimal OrderProductPrice
        {
            get { return _OrderProductPrice; }
            set { _OrderProductPrice = value; }
        }
        private decimal _OrderProductSingleWeight;
        /// <summary>
        /// OrderProductSingleWeight
        /// </summary>
        public decimal OrderProductSingleWeight
        {
            get { return _OrderProductSingleWeight; }
            set { _OrderProductSingleWeight = value; }
        }
        private Guid _OrderProductUnitID;
        /// <summary>
        /// OrderProductUnitID
        /// </summary>
        public Guid OrderProductUnitID
        {
            get { return _OrderProductUnitID; }
            set { _OrderProductUnitID = value; }
        }
        private string _OrderProductUnitName = "";
        /// <summary>
        /// OrderProductUnitName
        /// </summary>
        public string OrderProductUnitName
        {
            get { return _OrderProductUnitName; }
            set { _OrderProductUnitName = value; }
        }
        private int _OrderProductCount;
        /// <summary>
        /// OrderProductCount
        /// </summary>
        public int OrderProductCount
        {
            get { return _OrderProductCount; }
            set { _OrderProductCount = value; }
        }
        private double _OrderProductTotalWeight;
        /// <summary>
        /// OrderProductTotalWeight
        /// </summary>
        public double OrderProductTotalWeight
        {
            get { return _OrderProductTotalWeight; }
            set { _OrderProductTotalWeight = value; }
        }
        private decimal _OrderProductTotalFee;
        /// <summary>
        /// OrderProductTotalFee
        /// </summary>
        public decimal OrderProductTotalFee
        {
            get { return _OrderProductTotalFee; }
            set { _OrderProductTotalFee = value; }
        }
        private decimal _OrderFirstWeightFee;
        /// <summary>
        /// OrderFirstWeightFee
        /// </summary>
        public decimal OrderFirstWeightFee
        {
            get { return _OrderFirstWeightFee; }
            set { _OrderFirstWeightFee = value; }
        }
        private decimal _OrderContinuedWeightFee;
        /// <summary>
        /// OrderContinuedWeightFee
        /// </summary>
        public decimal OrderContinuedWeightFee
        {
            get { return _OrderContinuedWeightFee; }
            set { _OrderContinuedWeightFee = value; }
        }
        private decimal _OrderExpressFee;
        /// <summary>
        /// OrderExpressFee
        /// </summary>
        public decimal OrderExpressFee
        {
            get { return _OrderExpressFee; }
            set { _OrderExpressFee = value; }
        }
        private decimal _OrderRecommendRate;
        /// <summary>
        /// OrderRecommendRate
        /// </summary>
        public decimal OrderRecommendRate
        {
            get { return _OrderRecommendRate; }
            set { _OrderRecommendRate = value; }
        }
        private decimal _OrderRecommendFee;
        /// <summary>
        /// OrderRecommendFee
        /// </summary>
        public decimal OrderRecommendFee
        {
            get { return _OrderRecommendFee; }
            set { _OrderRecommendFee = value; }
        }
        private decimal _OrderTotalFee;
        /// <summary>
        /// OrderTotalFee
        /// </summary>
        public decimal OrderTotalFee
        {
            get { return _OrderTotalFee; }
            set { _OrderTotalFee = value; }
        }
        private decimal _OrderAvailableFee;
        /// <summary>
        /// OrderAvailableFee
        /// </summary>
        public decimal OrderAvailableFee
        {
            get { return _OrderAvailableFee; }
            set { _OrderAvailableFee = value; }
        }
        private Guid _OrderConsumerUserID;
        /// <summary>
        /// OrderConsumerUserID
        /// </summary>
        public Guid OrderConsumerUserID
        {
            get { return _OrderConsumerUserID; }
            set { _OrderConsumerUserID = value; }
        }
        private string _OrderConsumerName = "";
        /// <summary>
        /// OrderConsumerName
        /// </summary>
        public string OrderConsumerName
        {
            get { return _OrderConsumerName; }
            set { _OrderConsumerName = value; }
        }
        private string _OrderConsumerPhone = "";
        /// <summary>
        /// OrderConsumerPhone
        /// </summary>
        public string OrderConsumerPhone
        {
            get { return _OrderConsumerPhone; }
            set { _OrderConsumerPhone = value; }
        }
        private long _OrderConsumerAreaID;
        /// <summary>
        /// OrderConsumerAreaID
        /// </summary>
        public long OrderConsumerAreaID
        {
            get { return _OrderConsumerAreaID; }
            set { _OrderConsumerAreaID = value; }
        }
        private string _OrderConsumerAddress = "";
        /// <summary>
        /// OrderConsumerAddress
        /// </summary>
        public string OrderConsumerAddress
        {
            get { return _OrderConsumerAddress; }
            set { _OrderConsumerAddress = value; }
        }
        private Guid _OrderSendAddressID;
        /// <summary>
        /// OrderSendAddressID
        /// </summary>
        public Guid OrderSendAddressID
        {
            get { return _OrderSendAddressID; }
            set { _OrderSendAddressID = value; }
        }
        private Guid _OrderSendAddressTET;
        /// <summary>
        /// OrderSendAddressTET
        /// </summary>
        public Guid OrderSendAddressTET
        {
            get { return _OrderSendAddressTET; }
            set { _OrderSendAddressTET = value; }
        }
        private Guid _OrderExpressCompanyID;
        /// <summary>
        /// OrderExpressCompanyID
        /// </summary>
        public Guid OrderExpressCompanyID
        {
            get { return _OrderExpressCompanyID; }
            set { _OrderExpressCompanyID = value; }
        }
        private string _OrderExpressCompanyName = "";
        /// <summary>
        /// OrderExpressCompanyName
        /// </summary>
        public string OrderExpressCompanyName
        {
            get { return _OrderExpressCompanyName; }
            set { _OrderExpressCompanyName = value; }
        }
        private string _OrderExpressTrackingNumber = "";
        /// <summary>
        /// OrderExpressTrackingNumber
        /// </summary>
        public string OrderExpressTrackingNumber
        {
            get { return _OrderExpressTrackingNumber; }
            set { _OrderExpressTrackingNumber = value; }
        }
        private Guid _OrderOperatorUserID;
        /// <summary>
        /// OrderOperatorUserID
        /// </summary>
        public Guid OrderOperatorUserID
        {
            get { return _OrderOperatorUserID; }
            set { _OrderOperatorUserID = value; }
        }
        private DateTime _OrderOperatingTime;
        /// <summary>
        /// OrderOperatingTime
        /// </summary>
        public DateTime OrderOperatingTime
        {
            get { return _OrderOperatingTime; }
            set { _OrderOperatingTime = value; }
        }
        private string _OrderOperatingRemark = "";
        /// <summary>
        /// OrderOperatingRemark
        /// </summary>
        public string OrderOperatingRemark
        {
            get { return _OrderOperatingRemark; }
            set { _OrderOperatingRemark = value; }
        }
        private bool _OrderLock;
        /// <summary>
        /// OrderLock
        /// </summary>
        public bool OrderLock
        {
            get { return _OrderLock; }
            set { _OrderLock = value; }
        }
        private DateTime _OrderLockTime;
        /// <summary>
        /// OrderLockTime
        /// </summary>
        public DateTime OrderLockTime
        {
            get { return _OrderLockTime; }
            set { _OrderLockTime = value; }
        }
        private string _OrderLockRemark = "";
        /// <summary>
        /// OrderLockRemark
        /// </summary>
        public string OrderLockRemark
        {
            get { return _OrderLockRemark; }
            set { _OrderLockRemark = value; }
        }
        private Guid _OrderLockUserID;
        /// <summary>
        /// OrderLockUserID
        /// </summary>
        public Guid OrderLockUserID
        {
            get { return _OrderLockUserID; }
            set { _OrderLockUserID = value; }
        }
        private DateTime _OrderCreateTime;
        /// <summary>
        /// OrderCreateTime
        /// </summary>
        public DateTime OrderCreateTime
        {
            get { return _OrderCreateTime; }
            set { _OrderCreateTime = value; }
        }
        private int _OrderSort;
        /// <summary>
        /// OrderSort
        /// </summary>
        public int OrderSort
        {
            get { return _OrderSort; }
            set { _OrderSort = value; }
        }
        private bool _OrderDeleteStatus;
        /// <summary>
        /// OrderDeleteStatus
        /// </summary>
        public bool OrderDeleteStatus
        {
            get { return _OrderDeleteStatus; }
            set { _OrderDeleteStatus = value; }
        }
        #endregion

    }
}