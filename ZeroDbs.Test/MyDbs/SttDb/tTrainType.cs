using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.SttDb
{
    /// <summary>
    /// 车次类别
    /// </summary>
    [Serializable]
    public partial class tTrainType
    {
        #region --标准字段--
        private int _ID;
        /// <summary>
        /// [PrimaryKey]ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _TrainType = "";
        /// <summary>
        /// 车次类别
        /// </summary>
        public string TrainType
        {
            get { return _TrainType; }
            set { _TrainType = value; }
        }
        private string _DepartureTime = "";
        /// <summary>
        /// 发车时间
        /// </summary>
        public string DepartureTime
        {
            get { return _DepartureTime; }
            set { _DepartureTime = value; }
        }
        private string _ReturnTrainType;
        /// <summary>
        /// 返程车次类别名称
        /// </summary>
        public string ReturnTrainType
        {
            get { return _ReturnTrainType; }
            set { _ReturnTrainType = value; }
        }
        private string _ReturnTime;
        /// <summary>
        /// 返程时间
        /// </summary>
        public string ReturnTime
        {
            get { return _ReturnTime; }
            set { _ReturnTime = value; }
        }
        private string _Remark;
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
        #endregion

    }
}