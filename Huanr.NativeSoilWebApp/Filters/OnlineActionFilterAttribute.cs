using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Huanr.NativeSoilWebApp.Filters
{
    public class OnlineActionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        Microsoft.AspNetCore.Http.HttpContext httpContext = null;
        Tools.UserOnlineHelper userOnlineHelper = null;

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            httpContext = context.HttpContext;
            userOnlineHelper = new Tools.UserOnlineHelper(httpContext);

            var user = userOnlineHelper.GetOnlineUserInfo();
            if (user == null)
            {
                throw new Exception("用户未登录或在线超时");
            }
            
            base.OnActionExecuting(context);
        }

    }
}
