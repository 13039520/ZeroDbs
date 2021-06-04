using System;
using System.Collections.Generic;
using System.Text;
namespace MyDbs.TestDb
{
    /// <summary>
    /// TABLE:DT_CARWEIGHT_WEIGHT
    /// </summary>
    [Serializable]
    public partial class tDT_CARWEIGHT_WEIGHT
    {
        
        private string _FS_WEIGHTNO = "";
        /// <summary>
        /// [PrimaryKey]作业编号 磅房编号+- + YYYYMMDDHH24MISS
        /// </summary>
        public string FS_WEIGHTNO
        {
            get { return _FS_WEIGHTNO; }
            set { _FS_WEIGHTNO = value; }
        }
        private string _FS_CONTRACTNO;
        /// <summary>
        /// 合同号
        /// </summary>
        public string FS_CONTRACTNO
        {
            get { return _FS_CONTRACTNO; }
            set { _FS_CONTRACTNO = value; }
        }
        private string _FS_CONTRACTITEM;
        /// <summary>
        /// 合同项目编号
        /// </summary>
        public string FS_CONTRACTITEM
        {
            get { return _FS_CONTRACTITEM; }
            set { _FS_CONTRACTITEM = value; }
        }
        private string _FS_CARDNUMBER;
        /// <summary>
        /// 车证号
        /// </summary>
        public string FS_CARDNUMBER
        {
            get { return _FS_CARDNUMBER; }
            set { _FS_CARDNUMBER = value; }
        }
        private string _FS_CARNO;
        /// <summary>
        /// 车号
        /// </summary>
        public string FS_CARNO
        {
            get { return _FS_CARNO; }
            set { _FS_CARNO = value; }
        }
        private string _FS_MATERIAL;
        /// <summary>
        /// 物资代码
        /// </summary>
        public string FS_MATERIAL
        {
            get { return _FS_MATERIAL; }
            set { _FS_MATERIAL = value; }
        }
        private string _FS_SENDER;
        /// <summary>
        /// 发货方代码
        /// </summary>
        public string FS_SENDER
        {
            get { return _FS_SENDER; }
            set { _FS_SENDER = value; }
        }
        private string _FS_TRANSER;
        /// <summary>
        /// 承运方代码
        /// </summary>
        public string FS_TRANSER
        {
            get { return _FS_TRANSER; }
            set { _FS_TRANSER = value; }
        }
        private string _FS_RECEIVER;
        /// <summary>
        /// 收货方代码
        /// </summary>
        public string FS_RECEIVER
        {
            get { return _FS_RECEIVER; }
            set { _FS_RECEIVER = value; }
        }
        private double? _FN_SENDGROSSWEIGHT;
        /// <summary>
        /// 预报总重
        /// </summary>
        public double? FN_SENDGROSSWEIGHT
        {
            get { return _FN_SENDGROSSWEIGHT; }
            set { _FN_SENDGROSSWEIGHT = value; }
        }
        private double? _FN_SENDTAREWEIGHT;
        /// <summary>
        /// 预报皮重
        /// </summary>
        public double? FN_SENDTAREWEIGHT
        {
            get { return _FN_SENDTAREWEIGHT; }
            set { _FN_SENDTAREWEIGHT = value; }
        }
        private double? _FN_SENDNETWEIGHT;
        /// <summary>
        /// 预报净量
        /// </summary>
        public double? FN_SENDNETWEIGHT
        {
            get { return _FN_SENDNETWEIGHT; }
            set { _FN_SENDNETWEIGHT = value; }
        }
        private string _FS_FLOW;
        /// <summary>
        /// 流向
        /// </summary>
        public string FS_FLOW
        {
            get { return _FS_FLOW; }
            set { _FS_FLOW = value; }
        }
        private double? _FN_GROSSWEIGHT;
        /// <summary>
        /// 毛重重量
        /// </summary>
        public double? FN_GROSSWEIGHT
        {
            get { return _FN_GROSSWEIGHT; }
            set { _FN_GROSSWEIGHT = value; }
        }
        private string _FS_GROSSPOINT;
        /// <summary>
        /// 毛重计量点
        /// </summary>
        public string FS_GROSSPOINT
        {
            get { return _FS_GROSSPOINT; }
            set { _FS_GROSSPOINT = value; }
        }
        private string _FS_GROSSPERSON;
        /// <summary>
        /// 毛重计量员
        /// </summary>
        public string FS_GROSSPERSON
        {
            get { return _FS_GROSSPERSON; }
            set { _FS_GROSSPERSON = value; }
        }
        private DateTime? _FD_GROSSDATETIME;
        /// <summary>
        /// 毛重计量时间
        /// </summary>
        public DateTime? FD_GROSSDATETIME
        {
            get { return _FD_GROSSDATETIME; }
            set { _FD_GROSSDATETIME = value; }
        }
        private string _FS_GROSSSHIFT;
        /// <summary>
        /// 毛重计量班次
        /// </summary>
        public string FS_GROSSSHIFT
        {
            get { return _FS_GROSSSHIFT; }
            set { _FS_GROSSSHIFT = value; }
        }
        private double? _FN_TAREWEIGHT;
        /// <summary>
        /// 皮重重量
        /// </summary>
        public double? FN_TAREWEIGHT
        {
            get { return _FN_TAREWEIGHT; }
            set { _FN_TAREWEIGHT = value; }
        }
        private string _FS_TAREPOINT;
        /// <summary>
        /// 皮重计量点
        /// </summary>
        public string FS_TAREPOINT
        {
            get { return _FS_TAREPOINT; }
            set { _FS_TAREPOINT = value; }
        }
        private string _FS_TAREPERSON;
        /// <summary>
        /// 皮重计量员
        /// </summary>
        public string FS_TAREPERSON
        {
            get { return _FS_TAREPERSON; }
            set { _FS_TAREPERSON = value; }
        }
        private DateTime? _FD_TAREDATETIME;
        /// <summary>
        /// 皮重计量时间
        /// </summary>
        public DateTime? FD_TAREDATETIME
        {
            get { return _FD_TAREDATETIME; }
            set { _FD_TAREDATETIME = value; }
        }
        private string _FS_TARESHIFT;
        /// <summary>
        /// 皮重计量班次
        /// </summary>
        public string FS_TARESHIFT
        {
            get { return _FS_TARESHIFT; }
            set { _FS_TARESHIFT = value; }
        }
        private string _FS_FIRSTLABELID;
        /// <summary>
        /// 一次磅单条码
        /// </summary>
        public string FS_FIRSTLABELID
        {
            get { return _FS_FIRSTLABELID; }
            set { _FS_FIRSTLABELID = value; }
        }
        private string _FS_FULLLABELID;
        /// <summary>
        /// 完整磅单条码
        /// </summary>
        public string FS_FULLLABELID
        {
            get { return _FS_FULLLABELID; }
            set { _FS_FULLLABELID = value; }
        }
        private string _FS_UNLOADFLAG;
        /// <summary>
        /// 卸车确认(1:收货确认,2:退货过磅,3:退货不过磅,4:复磅)
        /// </summary>
        public string FS_UNLOADFLAG
        {
            get { return _FS_UNLOADFLAG; }
            set { _FS_UNLOADFLAG = value; }
        }
        private string _FS_LOADFLAG;
        /// <summary>
        /// 装车确认
        /// </summary>
        public string FS_LOADFLAG
        {
            get { return _FS_LOADFLAG; }
            set { _FS_LOADFLAG = value; }
        }
        private double? _FN_NETWEIGHT;
        /// <summary>
        /// 净重
        /// </summary>
        public double? FN_NETWEIGHT
        {
            get { return _FN_NETWEIGHT; }
            set { _FN_NETWEIGHT = value; }
        }
        private string _FS_SAMPLEPERSON;
        /// <summary>
        /// 取样员
        /// </summary>
        public string FS_SAMPLEPERSON
        {
            get { return _FS_SAMPLEPERSON; }
            set { _FS_SAMPLEPERSON = value; }
        }
        private string _FS_AUDITOR;
        /// <summary>
        /// 审核员
        /// </summary>
        public string FS_AUDITOR
        {
            get { return _FS_AUDITOR; }
            set { _FS_AUDITOR = value; }
        }
        private DateTime? _FD_AUDITTIME;
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? FD_AUDITTIME
        {
            get { return _FD_AUDITTIME; }
            set { _FD_AUDITTIME = value; }
        }
        private string _FS_UPLOADFLAG;
        /// <summary>
        /// 上传标志
        /// </summary>
        public string FS_UPLOADFLAG
        {
            get { return _FS_UPLOADFLAG; }
            set { _FS_UPLOADFLAG = value; }
        }
        private DateTime? _FD_UPLOADTIME;
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime? FD_UPLOADTIME
        {
            get { return _FD_UPLOADTIME; }
            set { _FD_UPLOADTIME = value; }
        }
        private DateTime? _FD_ACCOUNTDATE;
        /// <summary>
        /// SAP记账日期
        /// </summary>
        public DateTime? FD_ACCOUNTDATE
        {
            get { return _FD_ACCOUNTDATE; }
            set { _FD_ACCOUNTDATE = value; }
        }
        private string _FS_TERMTARENO;
        /// <summary>
        /// 对应期限皮重操作编号
        /// </summary>
        public string FS_TERMTARENO
        {
            get { return _FS_TERMTARENO; }
            set { _FS_TERMTARENO = value; }
        }
        private string _FS_YCSFYC;
        /// <summary>
        /// 是否异常(车号、数据)(一次计量)(0:正常,1:异常)
        /// </summary>
        public string FS_YCSFYC
        {
            get { return _FS_YCSFYC; }
            set { _FS_YCSFYC = value; }
        }
        private double? _FS_YKL;
        /// <summary>
        /// 应扣量
        /// </summary>
        public double? FS_YKL
        {
            get { return _FS_YKL; }
            set { _FS_YKL = value; }
        }
        private DateTime? _FD_SAMPLETIME;
        /// <summary>
        /// 取样时间
        /// </summary>
        public DateTime? FD_SAMPLETIME
        {
            get { return _FD_SAMPLETIME; }
            set { _FD_SAMPLETIME = value; }
        }
        private string _FS_SAMPLEPLACE;
        /// <summary>
        /// 取样点
        /// </summary>
        public string FS_SAMPLEPLACE
        {
            get { return _FS_SAMPLEPLACE; }
            set { _FS_SAMPLEPLACE = value; }
        }
        private string _FS_SAMPLEFLAG;
        /// <summary>
        /// 取样确认
        /// </summary>
        public string FS_SAMPLEFLAG
        {
            get { return _FS_SAMPLEFLAG; }
            set { _FS_SAMPLEFLAG = value; }
        }
        private string _FS_UNLOADPERSON;
        /// <summary>
        /// 卸车员
        /// </summary>
        public string FS_UNLOADPERSON
        {
            get { return _FS_UNLOADPERSON; }
            set { _FS_UNLOADPERSON = value; }
        }
        private DateTime? _FD_UNLOADTIME;
        /// <summary>
        /// 卸车时间
        /// </summary>
        public DateTime? FD_UNLOADTIME
        {
            get { return _FD_UNLOADTIME; }
            set { _FD_UNLOADTIME = value; }
        }
        private string _FS_UNLOADPLACE;
        /// <summary>
        /// 卸车点
        /// </summary>
        public string FS_UNLOADPLACE
        {
            get { return _FS_UNLOADPLACE; }
            set { _FS_UNLOADPLACE = value; }
        }
        private string _FS_DRIVERNAME;
        /// <summary>
        /// 驾驶员姓名
        /// </summary>
        public string FS_DRIVERNAME
        {
            get { return _FS_DRIVERNAME; }
            set { _FS_DRIVERNAME = value; }
        }
        private string _FS_DRIVERIDCARD;
        /// <summary>
        /// 驾驶员身份证
        /// </summary>
        public string FS_DRIVERIDCARD
        {
            get { return _FS_DRIVERIDCARD; }
            set { _FS_DRIVERIDCARD = value; }
        }
        private string _FS_SENDERSTORE;
        /// <summary>
        /// 发货地点
        /// </summary>
        public string FS_SENDERSTORE
        {
            get { return _FS_SENDERSTORE; }
            set { _FS_SENDERSTORE = value; }
        }
        private string _FS_IFSAMPLING;
        /// <summary>
        /// 是否需要取样确认 (1:为需要,0:为不需要)
        /// </summary>
        public string FS_IFSAMPLING
        {
            get { return _FS_IFSAMPLING; }
            set { _FS_IFSAMPLING = value; }
        }
        private string _FS_IFUNLOAD;
        /// <summary>
        /// 是否需要卸货确认 (1:为需要,0:为不需要)
        /// </summary>
        public string FS_IFUNLOAD
        {
            get { return _FS_IFUNLOAD; }
            set { _FS_IFUNLOAD = value; }
        }
        private string _FS_REWEIGHTFLAG;
        /// <summary>
        /// 复磅标记(1:毛重复磅，2:复磅完成，3:皮重复磅)
        /// </summary>
        public string FS_REWEIGHTFLAG
        {
            get { return _FS_REWEIGHTFLAG; }
            set { _FS_REWEIGHTFLAG = value; }
        }
        private DateTime? _FD_REWEIGHTTIME;
        /// <summary>
        /// 复磅确认时间
        /// </summary>
        public DateTime? FD_REWEIGHTTIME
        {
            get { return _FD_REWEIGHTTIME; }
            set { _FD_REWEIGHTTIME = value; }
        }
        private string _FS_REWEIGHTPLACE;
        /// <summary>
        /// 复磅确认地点
        /// </summary>
        public string FS_REWEIGHTPLACE
        {
            get { return _FS_REWEIGHTPLACE; }
            set { _FS_REWEIGHTPLACE = value; }
        }
        private string _FS_REWEIGHTPERSON;
        /// <summary>
        /// 复磅确认员
        /// </summary>
        public string FS_REWEIGHTPERSON
        {
            get { return _FS_REWEIGHTPERSON; }
            set { _FS_REWEIGHTPERSON = value; }
        }
        private string _FS_LOCALTOSERVERSIGN;
        /// <summary>
        /// 应急预案本地上传到服务器标志(1:为已上传,空:为在本地所产生)
        /// </summary>
        public string FS_LOCALTOSERVERSIGN
        {
            get { return _FS_LOCALTOSERVERSIGN; }
            set { _FS_LOCALTOSERVERSIGN = value; }
        }
        private DateTime? _FD_ECJLSJ;
        /// <summary>
        /// 二次计量时间
        /// </summary>
        public DateTime? FD_ECJLSJ
        {
            get { return _FD_ECJLSJ; }
            set { _FD_ECJLSJ = value; }
        }
        private double? _FS_YKBL;
        /// <summary>
        /// 应扣比例
        /// </summary>
        public double? FS_YKBL
        {
            get { return _FS_YKBL; }
            set { _FS_YKBL = value; }
        }
        private double? _FS_KHJZ;
        /// <summary>
        /// 扣后净重
        /// </summary>
        public double? FS_KHJZ
        {
            get { return _FS_KHJZ; }
            set { _FS_KHJZ = value; }
        }
        private string _FS_RECEIVERSTORE;
        /// <summary>
        /// 卸货地点
        /// </summary>
        public string FS_RECEIVERSTORE
        {
            get { return _FS_RECEIVERSTORE; }
            set { _FS_RECEIVERSTORE = value; }
        }
        private string _FS_MEMO;
        /// <summary>
        /// 备注
        /// </summary>
        public string FS_MEMO
        {
            get { return _FS_MEMO; }
            set { _FS_MEMO = value; }
        }
        private string _FS_DATASTATE;
        /// <summary>
        /// 数据状态 0：试用 1：正式使用
        /// </summary>
        public string FS_DATASTATE
        {
            get { return _FS_DATASTATE; }
            set { _FS_DATASTATE = value; }
        }
        private string _FS_SAPSTORE;
        /// <summary>
        /// SAP库存地代码
        /// </summary>
        public string FS_SAPSTORE
        {
            get { return _FS_SAPSTORE; }
            set { _FS_SAPSTORE = value; }
        }
        private string _FS_ISMATCH;
        /// <summary>
        /// 上传确认
        /// </summary>
        public string FS_ISMATCH
        {
            get { return _FS_ISMATCH; }
            set { _FS_ISMATCH = value; }
        }
        private string _FS_ENTERFACNO;
        /// <summary>
        /// 进出厂编号
        /// </summary>
        public string FS_ENTERFACNO
        {
            get { return _FS_ENTERFACNO; }
            set { _FS_ENTERFACNO = value; }
        }
        private string _FS_STATE;
        /// <summary>
        /// 0：未改变  1：修改、添加 且未审核 2：修改添加已审核  3：删除未审核 4：删除已审核
        /// </summary>
        public string FS_STATE
        {
            get { return _FS_STATE; }
            set { _FS_STATE = value; }
        }
        private string _FS_ISCOMPUTE;
        /// <summary>
        /// 用作销售小系统的标识 0：未计算 1：已计算
        /// </summary>
        public string FS_ISCOMPUTE
        {
            get { return _FS_ISCOMPUTE; }
            set { _FS_ISCOMPUTE = value; }
        }
        private string _FS_OPERSTATE;
        /// <summary>
        /// 审核人
        /// </summary>
        public string FS_OPERSTATE
        {
            get { return _FS_OPERSTATE; }
            set { _FS_OPERSTATE = value; }
        }
        private string _FS_ORDERNO;
        /// <summary>
        /// 交货单/采购订单
        /// </summary>
        public string FS_ORDERNO
        {
            get { return _FS_ORDERNO; }
            set { _FS_ORDERNO = value; }
        }
        private string _FS_BATCHNO;
        /// <summary>
        /// 批次号
        /// </summary>
        public string FS_BATCHNO
        {
            get { return _FS_BATCHNO; }
            set { _FS_BATCHNO = value; }
        }
        private string _FS_SALESTYPE;
        /// <summary>
        /// 销售类型（31：袋装，32：散装，33：袋装R）
        /// </summary>
        public string FS_SALESTYPE
        {
            get { return _FS_SALESTYPE; }
            set { _FS_SALESTYPE = value; }
        }
        private string _FS_LOADPERSON;
        /// <summary>
        /// 装车员
        /// </summary>
        public string FS_LOADPERSON
        {
            get { return _FS_LOADPERSON; }
            set { _FS_LOADPERSON = value; }
        }
        private DateTime? _FD_LOADTIME;
        /// <summary>
        /// 装车时间
        /// </summary>
        public DateTime? FD_LOADTIME
        {
            get { return _FD_LOADTIME; }
            set { _FD_LOADTIME = value; }
        }
        private string _FS_LOADPLACE;
        /// <summary>
        /// 装车点
        /// </summary>
        public string FS_LOADPLACE
        {
            get { return _FS_LOADPLACE; }
            set { _FS_LOADPLACE = value; }
        }
        private string _FS_SAMPLENO;
        /// <summary>
        /// 取样编号
        /// </summary>
        public string FS_SAMPLENO
        {
            get { return _FS_SAMPLENO; }
            set { _FS_SAMPLENO = value; }
        }
        private string _FS_OBLIGATE1;
        /// <summary>
        /// 预留字段1(用做水泥区域编号)
        /// </summary>
        public string FS_OBLIGATE1
        {
            get { return _FS_OBLIGATE1; }
            set { _FS_OBLIGATE1 = value; }
        }
        private string _FS_OBLIGATE2;
        /// <summary>
        /// 预留字段2(用做水泥库号)
        /// </summary>
        public string FS_OBLIGATE2
        {
            get { return _FS_OBLIGATE2; }
            set { _FS_OBLIGATE2 = value; }
        }
        private string _FS_OBLIGATE3;
        /// <summary>
        /// 预留字段3
        /// </summary>
        public string FS_OBLIGATE3
        {
            get { return _FS_OBLIGATE3; }
            set { _FS_OBLIGATE3 = value; }
        }
        private string _FS_OBLIGATE4;
        /// <summary>
        /// 预留字段4
        /// </summary>
        public string FS_OBLIGATE4
        {
            get { return _FS_OBLIGATE4; }
            set { _FS_OBLIGATE4 = value; }
        }
        private string _FS_OBLIGATE5;
        /// <summary>
        /// 预留字段5
        /// </summary>
        public string FS_OBLIGATE5
        {
            get { return _FS_OBLIGATE5; }
            set { _FS_OBLIGATE5 = value; }
        }

    }
}