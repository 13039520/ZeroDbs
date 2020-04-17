using System;

namespace Huanr.Services
{
    public class UserService
    {
        private readonly ZeroDbs.Interfaces.IDbService zeroService = null;
        public UserService(ZeroDbs.Interfaces.IDbService zeroService)
        {
            this.zeroService = zeroService;
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tUser> GetUserPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tUser>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tUser> GetUserPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tUser>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tUser> GetUserPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tUser>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tUser> GetUserPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tUser>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tUser GetUser(Guid userId)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tUser>(userId);
        }
        public Huanr.Models.NativeSoil.tUser GetUser(string userAccount, string userPassword)
        {
            var list = this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tUser>("UserAccount='"+userAccount+"' AND UserPassword='"+ this.zeroService.StrCommon.MD5_32(userPassword) + "'");
            return list != null && list.Count > 0 ? list[0] : null;
        }
        public bool HasUserAccount(string userAccount)
        {
            if (string.IsNullOrEmpty(userAccount)) { return true; }
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tUser>("UserAccount='" + userAccount + "'") > 0;
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemMenu> GetUserSystemMenuList(Guid userId)
        {
            var userMenuConfig = "MenuID IN(SELECT ConfigMenuID FROM T_UserMenuConfig WHERE ConfigUserID='" + userId + "')";
            var systemRoleMenuConfig = "MenuID IN(SELECT ConfigMenuID FROM T_SystemRoleMenuConfig WHERE ConfigRoleName IN(SELECT ConfigRoleName FROM T_UserRoleConfig WHERE ConfigUserID='" + userId + "'))";
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemMenu>(userMenuConfig + " OR " + systemRoleMenuConfig, "GroupID,MenuSort,MenuName");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tUserRoleConfig> GetUserRoleConfig(Guid userId)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tUserRoleConfig>("ConfigUserID='" + userId + "'");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tUserMenuConfig> GetUserMenuConfig(Guid userId)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tUserMenuConfig>("ConfigUserID='" + userId + "'");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tUserPermissionConfig> GetUserPermissionConfig(Guid userId)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tUserPermissionConfig>("ConfigUserID='" + userId + "'");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRoleMenuConfig> GetUserSystemRoleMenuConfig(Guid userId)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>("ConfigRoleID IN(SELECT ConfigRoleID FROM T_UserRoleConfig WHERE ConfigUserID='" + userId+"')");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRolePermissionConfig> GetSystemRolePermissionConfig(Guid userId)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>("ConfigRoleID IN(SELECT ConfigRoleID FROM T_UserRoleConfig WHERE ConfigUserID='" + userId + "')");
        }

        public int InsertUser(Huanr.Models.NativeSoil.tUser user)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tUser>(user);
        }
        public int UpdateUser(Huanr.Models.NativeSoil.tUser user)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tUser>(user);
        }
        public int DeleteUser(Huanr.Models.NativeSoil.tUser user)
        {
            return this.zeroService.DataOperator.Delete<Huanr.Models.NativeSoil.tUser>(user);
        }

        /// <summary>
        /// 获取用户的全部权限(缓存20分钟，包括角色权限及用户个人的权限)
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission> GetUserPermissionAllByCache(Guid userId)
        {
            var cacheKey = "tUserSystemPermission"+ userId;
            var cacheData = this.zeroService.Cache.Get<System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission>>(cacheKey);
            if (cacheData != null)
            {
                return cacheData;
            }
            var where = "PermissionName IN(SELECT ConfigPermissionName FROM T_UserPermissionConfig WHERE ConfigUserID='" + userId + "')";
            where += " OR PermissionName IN(SELECT ConfigPermissionName FROM T_SystemRolePermissionConfig WHERE ConfigRoleID IN(SELECT ConfigRoleID FROM T_UserRoleConfig WHERE ConfigUserID='"+ userId + "'))";
            var reval = this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemPermission>(where);
            this.zeroService.Cache.Set<System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission>>(cacheKey, reval, DateTime.Now.AddMinutes(20));
            return reval;
        }
        public System.Collections.Generic.Dictionary<string,bool> CheckUserPermission(Guid userId, string[] permissionNames)
        {
            if (permissionNames == null || permissionNames.Length < 1)
            {
                return new System.Collections.Generic.Dictionary<string, bool>();
            }
            var permissions = GetUserPermissionAllByCache(userId);
            var dic= new System.Collections.Generic.Dictionary<string, bool>();
            for (var i = 0; i < permissionNames.Length; i++)
            {
                var key = permissionNames[i];
                if (!string.IsNullOrEmpty(key))
                {
                    if (!dic.ContainsKey(key))
                    {
                        var b = permissions.Find(o => string.Equals(key, o.PermissionName, StringComparison.OrdinalIgnoreCase)) != null;
                        dic.Add(key, b);
                    }
                }
            }
            return dic;
        }

    }
}
