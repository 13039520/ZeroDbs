using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:M_AluminaDetail
    /// </summary>
    [Serializable]
    public partial class tM_AluminaDetail
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
        private double _C_Al2O3;
        /// <summary>
        /// C_Al2O3
        /// </summary>
        public double C_Al2O3
        {
            get { return _C_Al2O3; }
            set { _C_Al2O3 = value; }
        }
        private double _C_SiO2;
        /// <summary>
        /// C_SiO2
        /// </summary>
        public double C_SiO2
        {
            get { return _C_SiO2; }
            set { _C_SiO2 = value; }
        }
        private double _C_Fe2O3;
        /// <summary>
        /// C_Fe2O3
        /// </summary>
        public double C_Fe2O3
        {
            get { return _C_Fe2O3; }
            set { _C_Fe2O3 = value; }
        }
        private double _C_Na2O;
        /// <summary>
        /// C_Na2O
        /// </summary>
        public double C_Na2O
        {
            get { return _C_Na2O; }
            set { _C_Na2O = value; }
        }
        private double _C_NaOH;
        /// <summary>
        /// C_NaOH
        /// </summary>
        public double C_NaOH
        {
            get { return _C_NaOH; }
            set { _C_NaOH = value; }
        }
        private double _C_ZnO;
        /// <summary>
        /// C_ZnO
        /// </summary>
        public double C_ZnO
        {
            get { return _C_ZnO; }
            set { _C_ZnO = value; }
        }
        private double _C_TiO2;
        /// <summary>
        /// C_TiO2
        /// </summary>
        public double C_TiO2
        {
            get { return _C_TiO2; }
            set { _C_TiO2 = value; }
        }
        private double _C_V2O5;
        /// <summary>
        /// C_V2O5
        /// </summary>
        public double C_V2O5
        {
            get { return _C_V2O5; }
            set { _C_V2O5 = value; }
        }
        private double _C_MnO;
        /// <summary>
        /// C_MnO
        /// </summary>
        public double C_MnO
        {
            get { return _C_MnO; }
            set { _C_MnO = value; }
        }
        private double _C_Ga2O3;
        /// <summary>
        /// C_Ga2O3
        /// </summary>
        public double C_Ga2O3
        {
            get { return _C_Ga2O3; }
            set { _C_Ga2O3 = value; }
        }
        private double _C_P2O5;
        /// <summary>
        /// C_P2O5
        /// </summary>
        public double C_P2O5
        {
            get { return _C_P2O5; }
            set { _C_P2O5 = value; }
        }
        private double _P_Item1;
        /// <summary>
        /// P_Item1
        /// </summary>
        public double P_Item1
        {
            get { return _P_Item1; }
            set { _P_Item1 = value; }
        }
        private double _P_Item2;
        /// <summary>
        /// P_Item2
        /// </summary>
        public double P_Item2
        {
            get { return _P_Item2; }
            set { _P_Item2 = value; }
        }
        private double _P_Item3;
        /// <summary>
        /// P_Item3
        /// </summary>
        public double P_Item3
        {
            get { return _P_Item3; }
            set { _P_Item3 = value; }
        }
        private double _P_Item4;
        /// <summary>
        /// P_Item4
        /// </summary>
        public double P_Item4
        {
            get { return _P_Item4; }
            set { _P_Item4 = value; }
        }
        private double _P_Item5;
        /// <summary>
        /// P_Item5
        /// </summary>
        public double P_Item5
        {
            get { return _P_Item5; }
            set { _P_Item5 = value; }
        }
        private double _P_Item6;
        /// <summary>
        /// P_Item6
        /// </summary>
        public double P_Item6
        {
            get { return _P_Item6; }
            set { _P_Item6 = value; }
        }
        private double _P_Item7;
        /// <summary>
        /// P_Item7
        /// </summary>
        public double P_Item7
        {
            get { return _P_Item7; }
            set { _P_Item7 = value; }
        }
        private double _P_Item8;
        /// <summary>
        /// P_Item8
        /// </summary>
        public double P_Item8
        {
            get { return _P_Item8; }
            set { _P_Item8 = value; }
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
        private string _Quality = "";
        /// <summary>
        /// 正常情况下只有合格与不合格两个结果
        /// </summary>
        public string Quality
        {
            get { return _Quality; }
            set { _Quality = value; }
        }
        private string _QualityRemark = "";
        /// <summary>
        /// 进行正常或异常项备注
        /// </summary>
        public string QualityRemark
        {
            get { return _QualityRemark; }
            set { _QualityRemark = value; }
        }
        private string _RawIDs = "";
        /// <summary>
        /// RawIDs
        /// </summary>
        public string RawIDs
        {
            get { return _RawIDs; }
            set { _RawIDs = value; }
        }
        private string _RefMCategory = "";
        /// <summary>
        /// RefMCategory
        /// </summary>
        public string RefMCategory
        {
            get { return _RefMCategory; }
            set { _RefMCategory = value; }
        }
        private string _RefMTrademark = "";
        /// <summary>
        /// RefMTrademark
        /// </summary>
        public string RefMTrademark
        {
            get { return _RefMTrademark; }
            set { _RefMTrademark = value; }
        }

    }
}