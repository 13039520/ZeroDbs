using System;
using System.Collections.Generic;
using System.Text;
namespace Huanr.Models.Test
{
    /// <summary>
    /// VIEW:dataTypesView
    /// </summary>
    [Serializable]
    public partial class vDataTypesView
    {
        #region --标准字段--
        private long? _MyBigint;
        /// <summary>
        /// BIGINT
        /// </summary>
        public long? MyBigint
        {
            get { return _MyBigint; }
            set { _MyBigint = value; }
        }
        private byte[] _MyBlob;
        /// <summary>
        /// BLOB
        /// </summary>
        public byte[] MyBlob
        {
            get { return _MyBlob; }
            set { _MyBlob = value; }
        }
        private bool? _MyBoolean;
        /// <summary>
        /// BOOLEAN
        /// </summary>
        public bool? MyBoolean
        {
            get { return _MyBoolean; }
            set { _MyBoolean = value; }
        }
        private string _MyChar;
        /// <summary>
        /// CHAR (36)
        /// </summary>
        public string MyChar
        {
            get { return _MyChar; }
            set { _MyChar = value; }
        }
        private DateTime? _MyDate;
        /// <summary>
        /// DATE
        /// </summary>
        public DateTime? MyDate
        {
            get { return _MyDate; }
            set { _MyDate = value; }
        }
        private DateTime? _MyDatetime;
        /// <summary>
        /// DATETIME
        /// </summary>
        public DateTime? MyDatetime
        {
            get { return _MyDatetime; }
            set { _MyDatetime = value; }
        }
        private decimal? _MyDecimal;
        /// <summary>
        /// DECIMAL
        /// </summary>
        public decimal? MyDecimal
        {
            get { return _MyDecimal; }
            set { _MyDecimal = value; }
        }
        private double? _MyDouble;
        /// <summary>
        /// DOUBLE
        /// </summary>
        public double? MyDouble
        {
            get { return _MyDouble; }
            set { _MyDouble = value; }
        }
        private long? _MyInteger;
        /// <summary>
        /// [自增]INTEGER
        /// </summary>
        public long? MyInteger
        {
            get { return _MyInteger; }
            set { _MyInteger = value; }
        }
        private int? _MyInt;
        /// <summary>
        /// INT
        /// </summary>
        public int? MyInt
        {
            get { return _MyInt; }
            set { _MyInt = value; }
        }
        private object _MyNone;
        /// <summary>
        /// NONE
        /// </summary>
        public object MyNone
        {
            get { return _MyNone; }
            set { _MyNone = value; }
        }
        private decimal? _MyNumeric;
        /// <summary>
        /// NUMERIC
        /// </summary>
        public decimal? MyNumeric
        {
            get { return _MyNumeric; }
            set { _MyNumeric = value; }
        }
        private double? _MyReal;
        /// <summary>
        /// REAL
        /// </summary>
        public double? MyReal
        {
            get { return _MyReal; }
            set { _MyReal = value; }
        }
        private string _MyString;
        /// <summary>
        /// STRING
        /// </summary>
        public string MyString
        {
            get { return _MyString; }
            set { _MyString = value; }
        }
        private string _MyText;
        /// <summary>
        /// TEXT
        /// </summary>
        public string MyText
        {
            get { return _MyText; }
            set { _MyText = value; }
        }
        private DateTime? _MyTime;
        /// <summary>
        /// TIME
        /// </summary>
        public DateTime? MyTime
        {
            get { return _MyTime; }
            set { _MyTime = value; }
        }
        private string _MyVarchar;
        /// <summary>
        /// VARCHAR (500)
        /// </summary>
        public string MyVarchar
        {
            get { return _MyVarchar; }
            set { _MyVarchar = value; }
        }
        #endregion

    }
}