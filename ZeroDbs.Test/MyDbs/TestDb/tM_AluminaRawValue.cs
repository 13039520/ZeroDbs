using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:M_AluminaRawValue
    /// </summary>
    [Serializable]
    public partial class tM_AluminaRawValue
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
        private string _Sample = "";
        /// <summary>
        /// Sample
        /// </summary>
        public string Sample
        {
            get { return _Sample; }
            set { _Sample = value; }
        }
        private double _Al2O3;
        /// <summary>
        /// Al2O3
        /// </summary>
        public double Al2O3
        {
            get { return _Al2O3; }
            set { _Al2O3 = value; }
        }
        private double _SiO2;
        /// <summary>
        /// SiO2
        /// </summary>
        public double SiO2
        {
            get { return _SiO2; }
            set { _SiO2 = value; }
        }
        private double _Fe2O3;
        /// <summary>
        /// Fe2O3
        /// </summary>
        public double Fe2O3
        {
            get { return _Fe2O3; }
            set { _Fe2O3 = value; }
        }
        private double _Na2O;
        /// <summary>
        /// Na2O
        /// </summary>
        public double Na2O
        {
            get { return _Na2O; }
            set { _Na2O = value; }
        }
        private double _NaOH;
        /// <summary>
        /// 灼碱
        /// </summary>
        public double NaOH
        {
            get { return _NaOH; }
            set { _NaOH = value; }
        }
        private double _ZnO;
        /// <summary>
        /// ZnO
        /// </summary>
        public double ZnO
        {
            get { return _ZnO; }
            set { _ZnO = value; }
        }
        private double _TiO2;
        /// <summary>
        /// TiO2
        /// </summary>
        public double TiO2
        {
            get { return _TiO2; }
            set { _TiO2 = value; }
        }
        private double _V2O5;
        /// <summary>
        /// V2O5
        /// </summary>
        public double V2O5
        {
            get { return _V2O5; }
            set { _V2O5 = value; }
        }
        private double _MnO;
        /// <summary>
        /// MnO
        /// </summary>
        public double MnO
        {
            get { return _MnO; }
            set { _MnO = value; }
        }
        private double _Ga2O3;
        /// <summary>
        /// Ga2O3
        /// </summary>
        public double Ga2O3
        {
            get { return _Ga2O3; }
            set { _Ga2O3 = value; }
        }
        private double _P2O5;
        /// <summary>
        /// P2O5
        /// </summary>
        public double P2O5
        {
            get { return _P2O5; }
            set { _P2O5 = value; }
        }
        private DateTime _CreateTime = DateTime.Now;
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

    }
}