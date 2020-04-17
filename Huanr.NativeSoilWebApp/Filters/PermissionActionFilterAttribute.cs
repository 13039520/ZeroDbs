using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Huanr.NativeSoilWebApp.Filters
{
    public enum PermissionActionFilterAttributeOption
    {
        OR, AND
    }
    public class PermissionActionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        Microsoft.AspNetCore.Http.HttpContext httpContext = null;
        List<string> checkPermissions = new List<string>();
        PermissionActionFilterAttributeOption checkOption = PermissionActionFilterAttributeOption.AND;
        Tools.UserOnlineHelper userOnlineHelper = null;
        Huanr.Services.UserService userService = null;
        public PermissionActionFilterAttribute(string permission)
        {
            checkPermissions.Add(permission);
        }
        public PermissionActionFilterAttribute(string[] permissions, PermissionActionFilterAttributeOption option)
        {
            foreach (var permission in permissions)
            {
                if (checkPermissions.Contains(permission))
                {
                    continue;
                }
                checkPermissions.Add(permission);
            }
            checkOption = option;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            httpContext = context.HttpContext;
            var zeroService = (ZeroDbs.Interfaces.IDbService)httpContext.RequestServices.GetService(typeof(ZeroDbs.Interfaces.IDbService));

            userOnlineHelper = new Tools.UserOnlineHelper(httpContext);
            userService = new Services.UserService(zeroService);

            var user = userOnlineHelper.GetOnlineUserInfo();
            if (user == null)
            {
                throw new Exception("用户未登录或在线超时");
            }

            var permissions = userService.GetUserPermissionAllByCache(user.UserID);
            if (permissions == null || permissions.Count < 1)
            {
                throw new Exception("用户没有任何权限配置");
            }
            var permissionNames = permissions.Select(o => o.PermissionName).Distinct().ToArray();
            var dic = userService.CheckUserPermission(user.UserID, permissionNames);
            var isAnd = checkOption == PermissionActionFilterAttributeOption.AND;
            if (isAnd)
            {
                if (dic.Values.Select(o => o == false) == null)
                {
                    throw new Exception("用户不完全具有权限(" + string.Join(",", permissionNames) + ")");
                }
            }
            else
            {
                if (dic.Values.Select(o => o == true) == null) {
                    throw new Exception("用户缺少任一权限(" + string.Join(",", permissionNames) + ")");
                }
            }

            context.HttpContext.Items.Add("Permissions", checkPermissions);

            base.OnActionExecuting(context);
        }

    }
}
