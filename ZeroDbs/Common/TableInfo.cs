using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class TableInfo : ITableInfo
    {
        private string _DbName = "";
        public string DbName { get { return _DbName; } set { _DbName = value; } }
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private bool _IsView = true;
        public bool IsView
        {
            get { return _IsView; }
            set { _IsView = value; }
        }
        private List<IColumnInfo> _Colunms = new List<IColumnInfo>();
        public List<IColumnInfo> Colunms
        {
            get { return _Colunms; }
            set { _Colunms = value; }
        }

        #region IComparable
        public int CompareTo(object obj)
        {
            if (obj is ITableInfo)
            {
                ITableInfo temp = obj as ITableInfo;
                return this.Name.CompareTo(temp.Name);
            }
            return -1;
        }
        public object Clone()
        {
            return new TableInfo { Colunms = this.Colunms, Name = this.Name, DbName = this.DbName, Description = this.Description, IsView = this.IsView };
        }
        #endregion

    }
}
