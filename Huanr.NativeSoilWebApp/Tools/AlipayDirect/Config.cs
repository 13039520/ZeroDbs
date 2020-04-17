using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace  Huanr.NativeSoilWebApp.Tools.AlipayDirect
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.4
    /// 修改日期：2016-03-08
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// </summary>
    public class Config
    {
        #region --成员+属性访问器--
        //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        private string _partner = "2088621585109382";
        private string _seller_id = "2088621585109382";
        private string _key = "lym124tfib1fc0a367xi032qafk0szyh";
        private string _notify_url = "";
        private string _return_url = "";
        private string _sign_type = "MD5";
        private string _log_path = "";// HttpRuntime.AppDomainAppPath.ToString() + "log\\";
        private string _input_charset = "utf-8";
        private string _payment_type = "1";
        private string _service = "create_direct_pay_by_user";
        private string _anti_phishing_key = "";
        private string _exter_invoke_ip = "";

        // 合作身份者ID，签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        public string partner { get { return _partner; } set { _partner = value; } }

        // 收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
        public string seller_id { get { return _seller_id; } set { _seller_id = value; } }
		
        // MD5密钥，安全检验码，由数字和字母组成的32位字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        public string key { get { return _key; } set { _key = value; } }

        // 服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
        public string notify_url { get { return _notify_url; } set { _notify_url = value; } }

        // 页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
        public string return_url { get { return _return_url; } set { _return_url = value; } }

        // 签名方式
        public string sign_type { get { return _sign_type; } set { _sign_type = value; } }

        // 调试用，创建TXT日志文件夹路径，见AlipayCore.cs类中的LogResult(string sWord)打印方法。
        public string log_path { get { return _log_path; } set { _log_path = value; } }

        // 字符编码格式 目前支持 gbk 或 utf-8
        public string input_charset { get { return _input_charset; } set { _input_charset = value; } }

        // 支付类型 ，无需修改
        public string payment_type { get { return _payment_type; } set { _payment_type = value; } }

        // 调用的接口名，无需修改
        public string service { get { return _service; } set { _service = value; } }

        //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        //↓↓↓↓↓↓↓↓↓↓请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        //防钓鱼时间戳  若要使用请调用类文件submit中的Query_timestamp函数
        public string anti_phishing_key { get { return _anti_phishing_key; } set { _anti_phishing_key = value; } }
        //客户端的IP地址 非局域网的外网IP地址，如：221.0.0.1
        public string exter_invoke_ip { get { return _exter_invoke_ip; } set { _exter_invoke_ip = value; } }

        //↑↑↑↑↑↑↑↑↑↑请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        #endregion
        ZeroDbs.Interfaces.IDbService zeroService;
        Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor;
        Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public Config(ZeroDbs.Interfaces.IDbService zeroService, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            this.zeroService = zeroService;
            this.httpContextAccessor = httpContextAccessor;
            this.hostingEnvironment = (Microsoft.AspNetCore.Hosting.IHostingEnvironment)httpContextAccessor.HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment));
            Init();
        }
        private void Init()
        {
            try
            {
                string CacheKey = this.GetType().FullName.ToLower();
                System.Xml.XmlDocument Doc = null;
                object DocCacheObj = zeroService.Cache.Get<System.Xml.XmlDocument>(CacheKey);//NativeSoil.Core.Common.CacheHelper.Get(CacheKey);
                string XmlFilePath = "";
                bool IsReadyByFile = false;
                if (DocCacheObj != null)
                {
                    Doc = (System.Xml.XmlDocument)DocCacheObj;
                }
                else
                {
                    string XmlFilePathKey = "PaymentConfigXmlFilePath";
                    XmlFilePath = System.Configuration.ConfigurationManager.AppSettings[XmlFilePathKey];
                    if (string.IsNullOrEmpty(XmlFilePath))
                    {
                        throw new ArgumentException("缺少支付配置文件配置节：PaymentConfigXmlFilePath");
                    }
                    if (httpContextAccessor.HttpContext == null)
                    {
                        throw new ArgumentException("HttpContext对象为空");
                    }
                    
                    XmlFilePath = System.IO.Path.Combine(AppContext.BaseDirectory, XmlFilePath);
                    Doc =  Huanr.NativeSoilWebApp.Tools.XMLCommon.LoadXmlDoc(XmlFilePath);
                    IsReadyByFile = true;
                }
                if (Doc == null)
                {
                    throw new ArgumentException("未能成功载入支付配置文件");
                }
                System.Xml.XmlNode alipay_direct_node = Doc.SelectSingleNode(@"payment_config/alipay_direct");
                if (alipay_direct_node == null)
                {
                    throw new ArgumentException("配置文件缺少配置节：alipay_direct");
                }

                #region --读取逐个配置节的value--
                System.Xml.XmlNode node = alipay_direct_node.SelectSingleNode(@"partner");
                string partner = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(partner))
                {
                    throw new ArgumentException("配置文件缺少配置节partner或配置节partner的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"seller_id");
                string seller_id = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(seller_id))
                {
                    throw new ArgumentException("配置文件缺少配置节seller_id或配置节seller_id的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"key");
                string key = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("配置文件缺少配置节key或配置节key的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"notify_url");
                string notify_url = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(notify_url))
                {
                    throw new ArgumentException("配置文件缺少配置节notify_url或配置节notify_url的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"return_url");
                string return_url = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(return_url))
                {
                    throw new ArgumentException("配置文件缺少配置节return_url或配置节return_url的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"sign_type");
                string sign_type = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(sign_type))
                {
                    throw new ArgumentException("配置文件缺少配置节sign_type或配置节sign_type的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"log_path");
                string log_path = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(log_path))
                {
                    throw new ArgumentException("配置文件缺少配置节log_path或配置节log_path的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"input_charset");
                string input_charset = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(input_charset))
                {
                    throw new ArgumentException("配置文件缺少配置节input_charset或配置节input_charset的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"payment_type");
                string payment_type = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(payment_type))
                {
                    throw new ArgumentException("配置文件缺少配置节payment_type或配置节payment_type的value属性为空值");
                }
                node = alipay_direct_node.SelectSingleNode(@"service");
                string service = GetAttributeValueByName(node, "value");
                if (string.IsNullOrWhiteSpace(service))
                {
                    throw new ArgumentException("配置文件缺少配置节service或配置节service的value属性为空值");
                }
                #endregion

                this._anti_phishing_key = "";
                this._exter_invoke_ip = "";
                this._input_charset = input_charset.Trim();
                this._key = key.Trim();
                this._log_path = System.IO.Path.Combine(hostingEnvironment.WebRootPath, log_path);
                this._notify_url = notify_url.Trim();
                this._partner = partner.Trim();
                this._payment_type = payment_type.Trim();
                this._return_url = return_url.Trim();
                this._seller_id = seller_id.Trim();
                this._service = service.Trim();
                this._sign_type = sign_type.Trim();

                if (!System.IO.Directory.Exists(this._log_path))
                {
                    System.IO.Directory.CreateDirectory(this.log_path);
                }

                if (IsReadyByFile)
                {
                    zeroService.Cache.Set<System.Xml.XmlDocument>(CacheKey, Doc, DateTime.Now.AddMinutes(5));// NativeSoil.Core.Common.CacheHelper.Insert(CacheKey, Doc, XmlFilePath);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("读取支付配置发生异常", Ex);
            }
        }
        private string GetAttributeValueByName(System.Xml.XmlNode Node, string Name)
        {
            string s = "";
            if (Node != null && Node.Attributes.Count > 0)
            {
                System.Xml.XmlAttribute attr = Node.Attributes[Name];
                if (attr != null)
                {
                    s = attr.Value;
                }
            }
            return s;
        }

    }
}