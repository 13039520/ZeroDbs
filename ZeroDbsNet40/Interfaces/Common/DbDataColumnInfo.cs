using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class DbDataColumnInfo
    {
        private string _Name;
        /// <summary>
        /// 列名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Type;
        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private bool _IsNullable;
        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }
        private bool _IsPrimaryKey;
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get { return _IsPrimaryKey; }
            set { _IsPrimaryKey = value; }
        }
        private bool _IsIdentity;
        /// <summary>
        /// 是否自动增长
        /// </summary>
        public bool IsIdentity
        {
            get { return _IsIdentity; }
            set { _IsIdentity = value; }
        }
        private long _Byte;
        /// <summary>
        /// 占用字节
        /// </summary>
        public long Byte
        {
            get { return _Byte; }
            set { _Byte = value; }
        }
        private long _MaxLength;
        /// <summary>
        /// 长度限制
        /// </summary>
        public long MaxLength
        {
            get { return _MaxLength; }
            set { _MaxLength = value; }
        }
        private int _DecimalDigits;
        /// <summary>
        /// 小数位数（0表示无）
        /// </summary>
        public int DecimalDigits
        {
            get { return _DecimalDigits; }
            set { _DecimalDigits = value; }
        }
        private string _DefaultValue;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }
        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return System.Text.RegularExpressions.Regex.Replace(_Description, @"(\r\n|\n)", " "); }
            set { _Description = value; }
        }
    }
}
