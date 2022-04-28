using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class ColumnInfo : IColumnInfo
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private bool _IsNullable;
        public bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }
        private bool _IsPrimaryKey;
        public bool IsPrimaryKey
        {
            get { return _IsPrimaryKey; }
            set { _IsPrimaryKey = value; }
        }
        private bool _IsIdentity;
        public bool IsIdentity
        {
            get { return _IsIdentity; }
            set { _IsIdentity = value; }
        }
        private long _Byte;
        public long Byte
        {
            get { return _Byte; }
            set { _Byte = value; }
        }
        private long _MaxLength;
        public long MaxLength
        {
            get { return _MaxLength; }
            set { _MaxLength = value; }
        }
        private int _DecimalDigits;
        public int DecimalDigits
        {
            get { return _DecimalDigits; }
            set { _DecimalDigits = value; }
        }
        private string _DefaultValue;
        public string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }
        private string _Description;
        public string Description
        {
            get { return System.Text.RegularExpressions.Regex.Replace(_Description, @"(\r\n|\n)", " "); }
            set { _Description = value; }
        }
    }
}
