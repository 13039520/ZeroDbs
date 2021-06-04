using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:M_Alumina
    /// </summary>
    [Serializable]
    public partial class tM_Alumina
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
        private string _BillNo = "";
        /// <summary>
        /// BillNo
        /// </summary>
        public string BillNo
        {
            get { return _BillNo; }
            set { _BillNo = value; }
        }
        private DateTime _BillCreateTime = DateTime.Now;
        /// <summary>
        /// BillCreateTime
        /// </summary>
        public DateTime BillCreateTime
        {
            get { return _BillCreateTime; }
            set { _BillCreateTime = value; }
        }
        private string _BillCreatePerson = "";
        /// <summary>
        /// BillCreatePerson
        /// </summary>
        public string BillCreatePerson
        {
            get { return _BillCreatePerson; }
            set { _BillCreatePerson = value; }
        }
        private int _BillCustomState;
        /// <summary>
        /// BillCustomState
        /// </summary>
        public int BillCustomState
        {
            get { return _BillCustomState; }
            set { _BillCustomState = value; }
        }
        private string _BillRemark = "";
        /// <summary>
        /// BillRemark
        /// </summary>
        public string BillRemark
        {
            get { return _BillRemark; }
            set { _BillRemark = value; }
        }
        private string _SampleName = "";
        /// <summary>
        /// SampleName
        /// </summary>
        public string SampleName
        {
            get { return _SampleName; }
            set { _SampleName = value; }
        }
        private string _SampleFromPerson = "";
        /// <summary>
        /// SampleFromPerson
        /// </summary>
        public string SampleFromPerson
        {
            get { return _SampleFromPerson; }
            set { _SampleFromPerson = value; }
        }
        private string _SampleFromOrg = "";
        /// <summary>
        /// SampleFromOrg
        /// </summary>
        public string SampleFromOrg
        {
            get { return _SampleFromOrg; }
            set { _SampleFromOrg = value; }
        }
        private string _SampleReceivePerson = "";
        /// <summary>
        /// SampleReceivePerson
        /// </summary>
        public string SampleReceivePerson
        {
            get { return _SampleReceivePerson; }
            set { _SampleReceivePerson = value; }
        }
        private string _SampleReceiveOrg = "";
        /// <summary>
        /// SampleReceiveOrg
        /// </summary>
        public string SampleReceiveOrg
        {
            get { return _SampleReceiveOrg; }
            set { _SampleReceiveOrg = value; }
        }
        private DateTime _SampleReceiveDate = DateTime.Now;
        /// <summary>
        /// SampleReceiveDate
        /// </summary>
        public DateTime SampleReceiveDate
        {
            get { return _SampleReceiveDate; }
            set { _SampleReceiveDate = value; }
        }
        private string _ApproveState = "";
        /// <summary>
        /// ApproveState
        /// </summary>
        public string ApproveState
        {
            get { return _ApproveState; }
            set { _ApproveState = value; }
        }
        private string _ApproveRemark = "";
        /// <summary>
        /// ApproveRemark
        /// </summary>
        public string ApproveRemark
        {
            get { return _ApproveRemark; }
            set { _ApproveRemark = value; }
        }
        private string _ApprovePerson = "";
        /// <summary>
        /// ApprovePerson
        /// </summary>
        public string ApprovePerson
        {
            get { return _ApprovePerson; }
            set { _ApprovePerson = value; }
        }
        private DateTime _ApproveTime = DateTime.Now;
        /// <summary>
        /// ApproveTime
        /// </summary>
        public DateTime ApproveTime
        {
            get { return _ApproveTime; }
            set { _ApproveTime = value; }
        }
        private string _ExamineState = "";
        /// <summary>
        /// ExamineState
        /// </summary>
        public string ExamineState
        {
            get { return _ExamineState; }
            set { _ExamineState = value; }
        }
        private string _ExamineRemark = "";
        /// <summary>
        /// ExamineRemark
        /// </summary>
        public string ExamineRemark
        {
            get { return _ExamineRemark; }
            set { _ExamineRemark = value; }
        }
        private string _ExaminePerson = "";
        /// <summary>
        /// ExaminePerson
        /// </summary>
        public string ExaminePerson
        {
            get { return _ExaminePerson; }
            set { _ExaminePerson = value; }
        }
        private DateTime _ExamineTime = DateTime.Now;
        /// <summary>
        /// ExamineTime
        /// </summary>
        public DateTime ExamineTime
        {
            get { return _ExamineTime; }
            set { _ExamineTime = value; }
        }

    }
}