using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.NativeSoil
{
    /// <summary>
    /// TABLE:T_User
    /// </summary>
    [Serializable]
    public partial class tUser
    {
        #region --标准字段--
        private Guid _UserID;
        /// <summary>
        /// [主键]UserID
        /// </summary>
        public Guid UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        private string _UserAccount = "";
        /// <summary>
        /// UserAccount
        /// </summary>
        public string UserAccount
        {
            get { return _UserAccount; }
            set { _UserAccount = value; }
        }
        private string _UserPassword = "";
        /// <summary>
        /// UserPassword
        /// </summary>
        public string UserPassword
        {
            get { return _UserPassword; }
            set { _UserPassword = value; }
        }
        private string _UserName = "";
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        private string _UserNickname = "";
        /// <summary>
        /// UserNickname
        /// </summary>
        public string UserNickname
        {
            get { return _UserNickname; }
            set { _UserNickname = value; }
        }
        private DateTime _UserBirthday;
        /// <summary>
        /// UserBirthday
        /// </summary>
        public DateTime UserBirthday
        {
            get { return _UserBirthday; }
            set { _UserBirthday = value; }
        }
        private int _UserSex;
        /// <summary>
        /// UserSex
        /// </summary>
        public int UserSex
        {
            get { return _UserSex; }
            set { _UserSex = value; }
        }
        private string _UserHeadPortrait = "";
        /// <summary>
        /// UserHeadPortrait
        /// </summary>
        public string UserHeadPortrait
        {
            get { return _UserHeadPortrait; }
            set { _UserHeadPortrait = value; }
        }
        private int _UserStatus;
        /// <summary>
        /// UserStatus
        /// </summary>
        public int UserStatus
        {
            get { return _UserStatus; }
            set { _UserStatus = value; }
        }
        private string _UserStatusRemark = "";
        /// <summary>
        /// UserStatusRemark
        /// </summary>
        public string UserStatusRemark
        {
            get { return _UserStatusRemark; }
            set { _UserStatusRemark = value; }
        }
        private string _UserEmail;
        /// <summary>
        /// UserEmail
        /// </summary>
        public string UserEmail
        {
            get { return _UserEmail; }
            set { _UserEmail = value; }
        }
        private int? _UserEmailVerifyStatus;
        /// <summary>
        /// UserEmailVerifyStatus
        /// </summary>
        public int? UserEmailVerifyStatus
        {
            get { return _UserEmailVerifyStatus; }
            set { _UserEmailVerifyStatus = value; }
        }
        private string _UserPhone;
        /// <summary>
        /// UserPhone
        /// </summary>
        public string UserPhone
        {
            get { return _UserPhone; }
            set { _UserPhone = value; }
        }
        private int? _UserPhoneVerifyStatus;
        /// <summary>
        /// UserPhoneVerifyStatus
        /// </summary>
        public int? UserPhoneVerifyStatus
        {
            get { return _UserPhoneVerifyStatus; }
            set { _UserPhoneVerifyStatus = value; }
        }
        private int _UserIdentityType;
        /// <summary>
        /// UserIdentityType
        /// </summary>
        public int UserIdentityType
        {
            get { return _UserIdentityType; }
            set { _UserIdentityType = value; }
        }
        private string _UserIdentityNumber = "";
        /// <summary>
        /// UserIdentityNumber
        /// </summary>
        public string UserIdentityNumber
        {
            get { return _UserIdentityNumber; }
            set { _UserIdentityNumber = value; }
        }
        private int _UserIdentityVerifyStatus;
        /// <summary>
        /// UserIdentityVerifyStatus
        /// </summary>
        public int UserIdentityVerifyStatus
        {
            get { return _UserIdentityVerifyStatus; }
            set { _UserIdentityVerifyStatus = value; }
        }
        private DateTime _UserCreateTime;
        /// <summary>
        /// UserCreateTime
        /// </summary>
        public DateTime UserCreateTime
        {
            get { return _UserCreateTime; }
            set { _UserCreateTime = value; }
        }
        private int _UserSort;
        /// <summary>
        /// UserSort
        /// </summary>
        public int UserSort
        {
            get { return _UserSort; }
            set { _UserSort = value; }
        }
        private bool _UserDeleteStatus;
        /// <summary>
        /// UserDeleteStatus
        /// </summary>
        public bool UserDeleteStatus
        {
            get { return _UserDeleteStatus; }
            set { _UserDeleteStatus = value; }
        }
        #endregion

    }
}