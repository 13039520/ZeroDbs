using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Huanr.NativeSoilWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected ZeroDbs.Interfaces.IDbService currentZeroService = null;
        protected Huanr.Models.NativeSoil.tUser currentUser = null;
        public BaseController(ZeroDbs.Interfaces.IDbService zeroService)
        {
            currentZeroService = zeroService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var temp = new Tools.UserOnlineHelper(context.HttpContext);
            currentUser = temp.GetOnlineUserInfo();

            base.OnActionExecuting(context);
        }

        protected List<object> ListJsonCompress<T>(List<T> lis)
        {
            List<object> obj = new List<object>();
            if (lis == null || lis.Count < 1) { return obj; }
            System.Reflection.PropertyInfo[] pis = lis[0].GetType().GetProperties();
            obj.Add(pis.Length);
            for (var i = 0; i < pis.Length; i++)
            {
                obj.Add(pis[i].Name);
            }
            Type decimalType = typeof(Decimal);
            foreach (T t in lis)
            {
                for (var i = 0; i < pis.Length; i++)
                {
                    if (pis[i].PropertyType != decimalType)
                    {
                        obj.Add(pis[i].GetValue(t, null));
                    }
                    else
                    {
                        obj.Add(Convert.ToDouble(pis[i].GetValue(t, null)));
                    }
                }
            }
            return obj;
        }

        #region --从表单中获取数据填充或更新到实体对象--
        protected System.Collections.Specialized.NameValueCollection ConvertToNameValueCollection(IEnumerable<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> keyValuePairs)
        {
            System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
            foreach(var pairs in keyValuePairs)
            {
                nvc.Add(pairs.Key, pairs.Value.ToString());
            }
            return nvc;
        }
        protected T EntityGetByRequestForm<T>()
            where T : class, new()
        {
            return EntityGetByRequestForm<T>(ConvertToNameValueCollection(Request.Form));
        }
        protected T EntityGetByRequestForm<T>(System.Collections.Specialized.NameValueCollection nvc)
            where T : class, new()
        {
            return ZeroDbs.Tools.DataEntity.Get<T>(nvc);
        }
        protected T EntityUpdateByRequestForm<T>(T sourceEntity)
            where T : class, new()
        {
            return EntityUpdateByRequestForm<T>(sourceEntity, ConvertToNameValueCollection(Request.Form));
        }
        protected T EntityUpdateByRequestForm<T>(T sourceEntity, System.Collections.Specialized.NameValueCollection nvc)
            where T : class, new()
        {
            return ZeroDbs.Tools.DataEntity.Update<T>(sourceEntity, nvc);
        }
        #endregion

    }
}