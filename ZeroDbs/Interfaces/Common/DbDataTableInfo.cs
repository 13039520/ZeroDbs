using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class DbDataTableInfo : IComparable
    {
        private string _DbName = "";
        public string DbName { get { return _DbName; } set { _DbName = value; } }
        private string _Name;
        /// <summary>
        /// 数据表（或视图）名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Description;
        /// <summary>
        /// 数据表（或视图）描述
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private bool _IsView = true;
        /// <summary>
        /// 是不是视图
        /// </summary>
        public bool IsView
        {
            get { return _IsView; }
            set { _IsView = value; }
        }
        private List<DbDataColumnInfo> _Colunms = new List<DbDataColumnInfo>();
        /// <summary>
        /// 字段集合
        /// </summary>
        public List<DbDataColumnInfo> Colunms
        {
            get { return _Colunms; }
            set { _Colunms = value; }
        }

        #region IComparable 成员  
        public int CompareTo(object obj)
        {
            if (obj is DbDataTableInfo)
            {
                DbDataTableInfo temp = obj as DbDataTableInfo;
                return this.Name.CompareTo(temp.Name);
            }
            throw new NotImplementedException("obj is not a ZeroDbDataTableInfo!");
        }
        #endregion

    }
}
