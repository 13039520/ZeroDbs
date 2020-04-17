using System;

namespace Huanr.Services
{
    public class SystemService
    {
        private readonly ZeroDbs.Interfaces.IDbService zeroService = null;
        public SystemService(ZeroDbs.Interfaces.IDbService zeroService)
        {
            this.zeroService = zeroService;
        }

        #region -- tSystemMenu --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemMenu> GetSystemMenuPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemMenu>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemMenu> GetSystemMenuPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemMenu>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemMenu> GetSystemMenuPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemMenu>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemMenu> GetSystemMenuPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemMenu>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tSystemMenu GetSystemMenu(Guid menuId)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSystemMenu>(menuId);
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemMenu> GetSystemMenuByRoleName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) { return new System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemMenu>(); }
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemMenu>("MenuDeleteStatus=0 AND MenuID IN(SELECT ConfigMenuID FROM T_SystemRoleMenuConfig WHERE ConfigRoleName='" + roleName + "')", "GroupID,MenuSort,MenuName");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemMenu> GetSystemMenuAll()
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemMenu>("MenuDeleteStatus=0", "MenuSort,MenuName");
        }
        public bool HasSystemMenu(Guid menuGroupID, string menuName)
        {
            if (menuGroupID==Guid.Empty || string.IsNullOrEmpty(menuName)) { return true; }
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tSystemMenu>("MenuGroupID='" + menuGroupID + "' AND MenuName='" + menuName + "'") > 0;
        }
        public int InsertSystemMenu(Huanr.Models.NativeSoil.tSystemMenu menu)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemMenu>(menu);
        }
        public int UpdateSystemMenu(Huanr.Models.NativeSoil.tSystemMenu menu)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemMenu>(menu);
        }
        public int UpdateSystemMenu(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemMenu>(nvc);
        }
        public int DeleteSystemMenu(Huanr.Models.NativeSoil.tSystemMenu menu)
        {
            return this.zeroService.DataOperator.Delete<Huanr.Models.NativeSoil.tSystemMenu>(menu);
        }
        #endregion

        #region -- tSystemMenuGroup --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemMenuGroup> GetSystemMenuGroupPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemMenuGroup>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemMenuGroup> GetSystemMenuGroupPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemMenuGroup>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemMenuGroup> GetSystemMenuGroupPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemMenuGroup>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemMenuGroup> GetSystemMenuGroupPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemMenuGroup>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tSystemMenuGroup GetSystemMenuGroup(Guid groupId)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSystemMenuGroup>(groupId);
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemMenuGroup> GetSystemMenuGroupAll()
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemMenuGroup>("GroupDeleteStatus=0", "GroupSort,GroupName");
        }
        public bool HasSystemMenuGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) { return true; }
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tSystemMenuGroup>("GroupName='" + groupName + "'") > 0;
        }
        public int InsertSystemMenuGroup(Huanr.Models.NativeSoil.tSystemMenuGroup group)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemMenuGroup>(group);
        }
        public int UpdateSystemMenuGroup(Huanr.Models.NativeSoil.tSystemMenuGroup group)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemMenuGroup>(group);
        }
        public int UpdateSystemMenuGroup(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemMenuGroup>(nvc);
        }
        public int DeleteSystemMenuGroup(Huanr.Models.NativeSoil.tSystemMenuGroup group)
        {
            var reval = 0;
            var trans = this.zeroService.DataOperator.GetDbTransactionScope<Huanr.Models.NativeSoil.tSystemMenuGroup>(System.Data.IsolationLevel.ReadUncommitted);
            trans.Execute(cmd => {
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSystemMenuGroup>("GroupID='"+group.GroupID+"'");
                reval+=cmd.ExecuteNonQuery();
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSystemMenu>("GroupID='" + group.GroupID + "'");
                reval += cmd.ExecuteNonQuery();

                trans.Complete(true);
            });
            return reval;
        }
        #endregion

        #region -- tSystemPermission --
        /// <summary>
        /// 获取系统定义的全部权限(缓存20分钟)
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission> GetSystemPermissionAllByCache()
        {
            var cacheKey = "tSystemPermissionAll";
            var cacheData = this.zeroService.Cache.Get<System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission>>(cacheKey);
            if (cacheData != null)
            {
                return cacheData;
            }
            var reval = this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemPermission>("PermissionDeleteStatus=0", "GroupName,PermissionSort,PermissionName");
            this.zeroService.Cache.Set<System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission>>(cacheKey, reval, DateTime.Now.AddMinutes(20));
            return reval;
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission> GetSystemPermissionAll()
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemPermission>("PermissionDeleteStatus=0", "GroupName,PermissionSort,PermissionName");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission> GetSystemPermissionByRoleName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) { return new System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermission>(); }
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemPermission>("PermissionDeleteStatus=0 AND PermissionName IN(SELECT ConfigPermissionName FROM T_SystemRolePermissionConfig WHERE ConfigRoleName='" + roleName + "')", "GroupName,PermissionSort,PermissionName");
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemPermission> GetSystemPermissionPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemPermission>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemPermission> GetSystemPermissionPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemPermission>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemPermission> GetSystemPermissionPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemPermission>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemPermission> GetSystemPermissionPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemPermission>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tSystemPermission GetSystemPermission(string permissionName)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSystemPermission>(permissionName);
        }
        public bool HasSystemPermission(string groupName, string permissionName)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(permissionName)) { return true; }
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tSystemPermission>("GroupName='" + groupName + "' AND PermissionName='" + permissionName + "'") > 0;
        }
        public int InsertSystemPermission(Huanr.Models.NativeSoil.tSystemPermission permission)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemPermission>(permission);
        }
        public int UpdateSystemPermission(Huanr.Models.NativeSoil.tSystemPermission permission)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemPermission>(permission);
        }
        public int UpdateSystemPermission(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemPermission>(nvc);
        }
        public int DeleteSystemPermission(Huanr.Models.NativeSoil.tSystemPermission permission)
        {
            return this.zeroService.DataOperator.Delete<Huanr.Models.NativeSoil.tSystemPermission>(permission);
        }
        #endregion

        #region -- tSystemPermissionGroup --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemPermissionGroup> GetSystemPermissionGroupPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemPermissionGroup>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemPermissionGroup> GetSystemPermissionGroupPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemPermissionGroup>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemPermissionGroup> GetSystemPermissionGroupPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemPermissionGroup>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemPermissionGroup> GetSystemPermissionGroupPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemPermissionGroup>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tSystemPermissionGroup GetSystemPermissionGroup(string groupName)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSystemPermissionGroup>(groupName);
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemPermissionGroup> GetSystemPermissionGroupAll()
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemPermissionGroup>("GroupDeleteStatus=0", "GroupSort,GroupName");
        }
        public bool HasSystemPermissionGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) { return true; }
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tSystemPermissionGroup>("GroupName='" + groupName + "'") > 0;
        }
        public int InsertSystemPermissionGroup(Huanr.Models.NativeSoil.tSystemPermissionGroup group)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemPermissionGroup>(group);
        }
        public int UpdateSystemPermissionGroup(Huanr.Models.NativeSoil.tSystemPermissionGroup group)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemPermissionGroup>(group);
        }
        public int UpdateSystemPermissionGroup(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemPermissionGroup>(nvc);
        }
        public int DeleteSystemPermissionGroup(Huanr.Models.NativeSoil.tSystemPermissionGroup group)
        {
            var reval = 0;
            var trans = this.zeroService.DataOperator.GetDbTransactionScope<Huanr.Models.NativeSoil.tSystemPermissionGroup>(System.Data.IsolationLevel.ReadUncommitted);
            trans.Execute(cmd => {
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSystemPermissionGroup>("GroupName='" + group.GroupName + "'");
                reval += cmd.ExecuteNonQuery();
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSystemPermission>("GroupName='" + group.GroupName + "'");
                reval += cmd.ExecuteNonQuery();

                trans.Complete(true);
            });
            return reval;
        }
        #endregion

        #region -- tSystemRole --
        /// <summary>
        /// 获取所有系统角色权限配置(缓存20分钟)
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRole> GetSystemRoleAllByCache()
        {
            var cacheKey = "tSystemRoleAll";
            var cacheData = this.zeroService.Cache.Get<System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRole>>(cacheKey);
            if (cacheData != null)
            {
                return cacheData;
            }
            var reval = this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRole>("");
            this.zeroService.Cache.Set<System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRole>>(cacheKey, reval, DateTime.Now.AddMinutes(20));
            return reval;
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRole> GetSystemRoleAllBy()
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRole>("","GroupName,RoleSort,RoleName");
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemRole> GetSystemRolePage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemRole>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemRole> GetSystemRolePage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemRole>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemRole> GetSystemRolePage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemRole>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemRole> GetSystemRolePage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemRole>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tSystemRole GetSystemRole(string roleName)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSystemRole>(roleName);
        }
        public bool HasSystemRole(string groupName, string roleName)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(roleName)) { return true; }
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tSystemRole>("GroupName='" + groupName + "' AND RoleName='" + roleName + "'") > 0;
        }
        public int InsertSystemRole(Huanr.Models.NativeSoil.tSystemRole role)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemRole>(role);
        }
        public int UpdateSystemRole(Huanr.Models.NativeSoil.tSystemRole role)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemRole>(role);
        }
        public int UpdateSystemRole(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemRole>(nvc);
        }
        public int UpdateRolePermission(string roleName,System.Collections.Generic.List<string> permissionNames)
        {
            var reval = 0;
            var trans = this.zeroService.GetDbTransactionScope<Huanr.Models.NativeSoil.tSystemRole>(System.Data.IsolationLevel.ReadUncommitted);
            trans.Execute(cmd => {
                cmd.CommandText =  cmd.DbSqlBuilder.BuildSqlSelectByKey<Huanr.Models.NativeSoil.tSystemRole>(roleName);
                var roleList = cmd.ExecuteReader<Huanr.Models.NativeSoil.tSystemRole>();
                if (roleList.Count < 1)
                {
                    throw new Exception("角色不存在");
                }
                var roleInfo = roleList[0];
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlSelect<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>("ConfigRoleName='" + roleName + "'","");
                var rolePermissionList = cmd.ExecuteReader<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>();
                var rplCount = rolePermissionList.Count;
                foreach (string permission in permissionNames)
                {
                    #region -- 插入不存在的 --
                    if (rolePermissionList.Find(o => o.ConfigPermissionName == permission) == null)
                    {
                        rplCount++;
                        cmd.CommandText = cmd.DbSqlBuilder.BuildSqlInsert<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>(new Huanr.Models.NativeSoil.tSystemRolePermissionConfig
                        {
                            ConfigCreateTime = DateTime.Now,
                            ConfigDeleteStatus = false,
                            ConfigID = Guid.NewGuid(),
                            ConfigPermissionName = permission,
                            ConfigRoleName = roleName,
                            ConfigSort = rplCount
                        },new string[] { });

                        reval += cmd.ExecuteNonQuery();
                    }
                    #endregion
                }
                foreach(var rpc in rolePermissionList)
                {
                    #region -- 删除被移除的 --
                    if (permissionNames.Contains(rpc.ConfigPermissionName))
                    {
                        continue;
                    }
                    cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>(rpc, new string[] { });
                    reval += cmd.ExecuteNonQuery();
                    #endregion
                }

                //提交事务
                trans.Complete(true);
            });
            return reval;
        }
        public int UpdateRoleMenu(string roleName, System.Collections.Generic.List<Guid> menuIds)
        {
            var reval = 0;
            var trans = this.zeroService.GetDbTransactionScope<Huanr.Models.NativeSoil.tSystemRole>(System.Data.IsolationLevel.ReadUncommitted);
            trans.Execute(cmd => {
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlSelectByKey<Huanr.Models.NativeSoil.tSystemRole>(roleName);
                var roleList = cmd.ExecuteReader<Huanr.Models.NativeSoil.tSystemRole>();
                if (roleList.Count < 1)
                {
                    throw new Exception("角色不存在");
                }
                var roleInfo = roleList[0];
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlSelect<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>("ConfigRoleName='" + roleName + "'", "");
                var roleMenuList = cmd.ExecuteReader<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>();
                var rmlCount = roleMenuList.Count;
                foreach (Guid menu in menuIds)
                {
                    #region -- 插入不存在的 --
                    if (roleMenuList.Find(o => o.ConfigMenuID == menu) == null)
                    {
                        rmlCount++;
                        cmd.CommandText = cmd.DbSqlBuilder.BuildSqlInsert<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>(new Huanr.Models.NativeSoil.tSystemRoleMenuConfig
                        {
                            ConfigCreateTime = DateTime.Now,
                            ConfigDeleteStatus = false,
                            ConfigID = Guid.NewGuid(),
                            ConfigMenuID = menu,
                            ConfigRoleName = roleName,
                            ConfigSort = rmlCount
                        }, new string[] { });

                        reval += cmd.ExecuteNonQuery();
                    }
                    #endregion
                }
                foreach (var rpc in roleMenuList)
                {
                    #region -- 删除被移除的 --
                    if (menuIds.Contains(rpc.ConfigMenuID))
                    {
                        continue;
                    }
                    cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>(rpc, new string[] { });
                    reval += cmd.ExecuteNonQuery();
                    #endregion
                }

                //提交事务
                trans.Complete(true);
            });
            return reval;
        }
        public int DeleteSystemRole(Huanr.Models.NativeSoil.tSystemRole role)
        {
            return this.zeroService.DataOperator.Delete<Huanr.Models.NativeSoil.tSystemRole>(role);
        }
        #endregion

        #region -- tSystemRoleGroup --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemRoleGroup> GetSystemRoleGroupPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemRoleGroup>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemRoleGroup> GetSystemRoleGroupPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemRoleGroup>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemRoleGroup> GetSystemRoleGroupPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemRoleGroup>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSystemRoleGroup> GetSystemRoleGroupPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSystemRoleGroup>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tSystemRoleGroup GetSystemRoleGroup(string groupName)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSystemRoleGroup>(groupName);
        }
        public bool HasSystemRoleGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) { return true; }
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tSystemRoleGroup>("GroupName='" + groupName + "'") > 0;
        }
        public int InsertSystemRoleGroup(Huanr.Models.NativeSoil.tSystemRoleGroup group)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemRoleGroup>(group);
        }
        public int UpdateSystemRoleGroup(Huanr.Models.NativeSoil.tSystemRoleGroup group)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemRoleGroup>(group);
        }
        public int UpdateSystemRoleGroup(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemRoleGroup>(nvc);
        }
        public int DeleteSystemRoleGroup(Huanr.Models.NativeSoil.tSystemRoleGroup group)
        {
            var reval = 0;
            var trans = this.zeroService.DataOperator.GetDbTransactionScope<Huanr.Models.NativeSoil.tSystemRoleGroup>(System.Data.IsolationLevel.ReadUncommitted);
            trans.Execute(cmd => {
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSystemRoleGroup>("GroupName='" + group.GroupName + "'");
                reval += cmd.ExecuteNonQuery();
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSystemRole>("GroupName='" + group.GroupName + "'");
                reval += cmd.ExecuteNonQuery();

                trans.Complete(true);
            });
            return reval;
        }
        #endregion

        #region -- tSystemRoleMenuConfig --
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRoleMenuConfig> GetSystemRoleMenuConfig()
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>("");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRoleMenuConfig> GetSystemRoleMenuConfigByRoleID(string roleName)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>("ConfigRoleName='"+ roleName + "'");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRoleMenuConfig> GetSystemRoleMenuConfigByMenuID(Guid menuId)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>("ConfigMenuID='"+menuId+"'");
        }
        public int InsertSystemRoleMenuConfig(Huanr.Models.NativeSoil.tSystemRoleMenuConfig systemRoleMenuConfig)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>(systemRoleMenuConfig);
        }
        public int UpdateSystemRoleMenuConfig(Huanr.Models.NativeSoil.tSystemRoleMenuConfig systemRoleMenuConfig)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>(systemRoleMenuConfig);
        }
        public int UpdateSystemRoleMenuConfig(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>(nvc);
        }
        public int DeleteSystemRoleMenuConfig(Huanr.Models.NativeSoil.tSystemRoleMenuConfig systemRoleMenuConfig)
        {
            return this.zeroService.DataOperator.Delete<Huanr.Models.NativeSoil.tSystemRoleMenuConfig>(systemRoleMenuConfig);
        }
        #endregion

        #region -- tSystemRolePermissionConfig --
        /// <summary>
        /// 获取所有系统角色权限配置(缓存20分钟)
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRolePermissionConfig> GetSystemRolePermissionConfigAllByCache()
        {
            var cacheKey = "tSystemRolePermissionConfigAll";
            var cacheData = this.zeroService.Cache.Get<System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>>(cacheKey);
            if (cacheData != null)
            {
                return cacheData;
            }
            var reval = this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>("");
            this.zeroService.Cache.Set<System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>>(cacheKey, reval, DateTime.Now.AddMinutes(20));
            return reval;
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRolePermissionConfig> GetSystemRolePermissionConfigByRoleID(string roleName)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>("ConfigRoleName='" + roleName + "'");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tSystemRolePermissionConfig> GetSystemRolePermissionConfigByPermissionName(string permissionName)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>("ConfigPermissionName='" + permissionName + "'");
        }
        public int InsertSystemRolePermissionConfig(Huanr.Models.NativeSoil.tSystemRolePermissionConfig systemRolePermissionConfig)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>(systemRolePermissionConfig);
        }
        public int UpdateSystemRolePermissionConfig(Huanr.Models.NativeSoil.tSystemRolePermissionConfig systemRolePermissionConfig)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>(systemRolePermissionConfig);
        }
        public int UpdateSystemRolePermissionConfig(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>(nvc);
        }
        public int DeleteSystemRolePermissionConfig(Huanr.Models.NativeSoil.tSystemRolePermissionConfig systemRolePermissionConfig)
        {
            return this.zeroService.DataOperator.Delete<Huanr.Models.NativeSoil.tSystemRolePermissionConfig>(systemRolePermissionConfig);
        }
        #endregion

        #region -- tBaseCategory --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tBaseCategory> GetBaseCategoryPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tBaseCategory>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tBaseCategory> GetBaseCategoryPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tBaseCategory>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tBaseCategory> GetBaseCategoryPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tBaseCategory>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tBaseCategory> GetBaseCategoryPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tBaseCategory>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tBaseCategory GetBaseCategory(string categoryId)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tBaseCategory>(categoryId);
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tBaseCategory> GetBaseCategoryByParent(string categoryId)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tBaseCategory>("BDeleteStatus=0 AND BParentID='"+ categoryId + "'");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tBaseCategory> GetBaseCategoryAll()
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tBaseCategory>("BDeleteStatus=0", "BSort,BName");
        }
        public bool HasBaseCategory(string categoryParentId, string categoryName)
        {
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tBaseCategory>("BParentID='" + categoryParentId + "' AND BName='" + categoryName + "'") > 0;
        }
        public int InsertBaseCategory(Huanr.Models.NativeSoil.tBaseCategory category)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tBaseCategory>(category);
        }
        public int UpdateBaseCategory(Huanr.Models.NativeSoil.tBaseCategory category)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tBaseCategory>(category);
        }
        public int UpdateBaseCategory(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tBaseCategory>(nvc);
        }
        public int DeleteBaseCategory(Huanr.Models.NativeSoil.tBaseCategory category)
        {
            var trans = this.zeroService.GetDbTransactionScope<Huanr.Models.NativeSoil.tBaseCategory>(System.Data.IsolationLevel.ReadUncommitted);
            var reval = 0;
            trans.Execute(cmd => {
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tBaseCategory>("BID='" + category.BID + "'");
                reval += cmd.ExecuteNonQuery();
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tBaseCategory>("BPath LIKE '%/" + category.BID + "/%'");
                reval += cmd.ExecuteNonQuery();

                trans.Complete(true);
            });
            return reval;
        }
        #endregion

        #region -- tArea --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tArea> GetAreaPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tArea>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tArea> GetAreaPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tArea>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tArea> GetAreaPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tArea>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tArea> GetAreaPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tArea>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tArea GetArea(string areaCode)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tArea>(areaCode);
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tArea> GetAreaByParent(string areaCode)
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tArea>("AreaDeleteStatus=0 AND AreaParent='" + areaCode + "'","AreaCode");
        }
        public System.Collections.Generic.List<Huanr.Models.NativeSoil.tArea> GetAreaAll()
        {
            return this.zeroService.DataOperator.Select<Huanr.Models.NativeSoil.tArea>("AreaDeleteStatus=0", "");
        }
        public bool HasArea(string areaParent, string areaName)
        {
            return this.zeroService.DataOperator.Count<Huanr.Models.NativeSoil.tArea>("AreaParent='" + areaParent + "' AND AreaName='" + areaName + "'") > 0;
        }
        public int InsertArea(Huanr.Models.NativeSoil.tArea area)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tArea>(area);
        }
        public int UpdateArea(Huanr.Models.NativeSoil.tArea area)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tArea>(area);
        }
        public int UpdateArea(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tArea>(nvc);
        }
        public int DeleteArea(Huanr.Models.NativeSoil.tArea area)
        {
            var trans = this.zeroService.GetDbTransactionScope<Huanr.Models.NativeSoil.tArea>(System.Data.IsolationLevel.ReadUncommitted);
            var reval = 0;
            trans.Execute(cmd => {
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tArea>("AreaCode='"+area.AreaCode+"'");
                reval+=cmd.ExecuteNonQuery();
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tArea>("AreaPath LIKE '%/"+area.AreaCode + "/%'");
                reval += cmd.ExecuteNonQuery();

                trans.Complete(true);
            });
            return reval;
        }
        #endregion

    }
}
