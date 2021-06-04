using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:M_Approval
    /// </summary>
    [Serializable]
    public partial class tM_Approval
    {
        
        private int _ID;
        /// <summary>
        /// [Identity][PrimaryKey]ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _BillID;
        /// <summary>
        /// BillID
        /// </summary>
        public int BillID
        {
            get { return _BillID; }
            set { _BillID = value; }
        }
        private string _ApprovalState = "";
        /// <summary>
        /// ApprovalState
        /// </summary>
        public string ApprovalState
        {
            get { return _ApprovalState; }
            set { _ApprovalState = value; }
        }
        private string _ApprovalPerson = "";
        /// <summary>
        /// ApprovalPerson
        /// </summary>
        public string ApprovalPerson
        {
            get { return _ApprovalPerson; }
            set { _ApprovalPerson = value; }
        }
        private DateTime _ApprovalDate = DateTime.Now;
        /// <summary>
        /// ApprovalDate
        /// </summary>
        public DateTime ApprovalDate
        {
            get { return _ApprovalDate; }
            set { _ApprovalDate = value; }
        }
        private string _ApprovalType = "";
        /// <summary>
        /// ApprovalType
        /// </summary>
        public string ApprovalType
        {
            get { return _ApprovalType; }
            set { _ApprovalType = value; }
        }
        private string _OperateRemark = "";
        /// <summary>
        /// OperateRemark
        /// </summary>
        public string OperateRemark
        {
            get { return _OperateRemark; }
            set { _OperateRemark = value; }
        }

    }
}