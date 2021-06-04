using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:Q_ProductType
    /// </summary>
    [Serializable]
    public partial class tQ_ProductType
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
        private string _Name = "";
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Type = "";
        /// <summary>
        /// Type
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private int _Orderby;
        /// <summary>
        /// Orderby
        /// </summary>
        public int Orderby
        {
            get { return _Orderby; }
            set { _Orderby = value; }
        }

    }
}