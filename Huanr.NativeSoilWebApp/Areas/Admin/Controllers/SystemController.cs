using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Huanr.NativeSoilWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Filters.OnlineActionFilter()]
    public class SystemController : Huanr.NativeSoilWebApp.Controllers.BaseController
    {
        Huanr.Services.UserService userService = null;
        Huanr.Services.SystemService systemService = null;
        public SystemController(ZeroDbs.Interfaces.IDbService zeroService) : base(zeroService)
        {
            userService = new Huanr.Services.UserService(zeroService);
            systemService = new Huanr.Services.SystemService(currentZeroService);
        }

        #region -- roleGroup --
        //[Filters.PermissionActionFilter("角色分组列表")]
        public IActionResult RoleGroupList()
        {
            return View();
        }
        //[Filters.PermissionActionFilter("角色分组列表")]
        public JsonResult RoleGroupListPage(long page,long size)
        {
            try
            {
                string keyword = Request.Query["keyword"];
                string where = "GroupDeleteStatus=0";
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = keyword.Trim();
                    if (keyword.Length > 30)
                    {
                        keyword = keyword.Substring(0, 30);
                    }
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    where += " AND GroupName LIKE '%"+keyword+"%'";
                }
                var list = systemService.GetSystemRoleGroupPage(page, size, where, "", 0);
                return Json(new { status = 1, msg = "OK", total = list.Total, data = list.Items });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("角色分组新增")]
        public IActionResult RoleGroupAdd()
        {
            return View();
        }
        //[Filters.PermissionActionFilter("角色分组新增")]
        public JsonResult RoleGroupAddSave(Huanr.Models.NativeSoil.tSystemRoleGroup roleGroup)
        {
            try
            {
                roleGroup.GroupCreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(roleGroup.GroupName))
                {
                    throw new Exception("角色分组名称不能为空");
                }
                if (string.IsNullOrEmpty(roleGroup.GroupRemark))
                {
                    roleGroup.GroupRemark = "";
                }
                if (systemService.InsertSystemRoleGroup(roleGroup) < 1)
                {
                    throw new Exception("角色分组新增失败");
                }
                return Json(new { status = 1, msg = "角色分组新增成功" });
            }
            catch(Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("角色分组编辑")]
        public IActionResult RoleGroupEdit()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemRoleGroup tSystemRoleGroup = null;
            if (!string.IsNullOrEmpty(key))
            {
                tSystemRoleGroup = systemService.GetSystemRoleGroup(key);
            }
            if (tSystemRoleGroup == null)
            {
                throw new Exception("要编辑的角色分组不存在或已经被删除");
            }

            ViewBag.tSystemRoleGroup = tSystemRoleGroup;

            return View();
        }
        //[Filters.PermissionActionFilter("角色分组编辑")]
        public JsonResult RoleGroupEditSave()
        {
            try
            {
                var nvc = base.ConvertToNameValueCollection(Request.Form);
                if (systemService.UpdateSystemRoleGroup(nvc) < 1)
                {
                    throw new Exception("角色分组更新失败");
                }
                return Json(new { status = 1, msg = "角色分组更新成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("角色分组明细")]
        public IActionResult RoleGroupDetail()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemRoleGroup tSystemRoleGroup = null;
            if (!string.IsNullOrEmpty(key))
            {
                tSystemRoleGroup = systemService.GetSystemRoleGroup(key);
            }
            if (tSystemRoleGroup == null)
            {
                throw new Exception("要查看的角色分组不存在或已经被删除");
            }
            
            ViewBag.tSystemRoleGroup = tSystemRoleGroup;

            return View();
        }
        //[Filters.PermissionActionFilter("角色分组删除")]
        public JsonResult RoleGroupDelete()
        {
            try
            {
                string key = Request.Query["key"];
                if (!string.IsNullOrEmpty(key))
                {
                    systemService.DeleteSystemRoleGroup(new Huanr.Models.NativeSoil.tSystemRoleGroup { GroupName = key });
                }
                return Json(new { status = 1, msg = "角色分组删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        #endregion

        #region -- role --
        //[Filters.PermissionActionFilter("角色列表")]
        public IActionResult RoleList()
        {
            var list = systemService.GetSystemRoleGroupPage(1, 10000, "GroupDeleteStatus=0", "GroupSort,GroupName", 0);
            List<Huanr.Models.NativeSoil.tSystemRoleGroup> tSystemRoleGroupList = list.Items;

            ViewBag.tSystemRoleGroupList = tSystemRoleGroupList;
            return View();
        }
        //[Filters.PermissionActionFilter("角色列表")]
        public JsonResult RoleListPage(long page, long size)
        {
            try
            {
                string group = Request.Query["group"];
                string keyword = Request.Query["keyword"];
                string where = "RoleDeleteStatus=0";
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = keyword.Trim();
                    if (keyword.Length > 30)
                    {
                        keyword = keyword.Substring(0, 30);
                    }
                }
                if (!string.IsNullOrEmpty(group))
                {
                    where += " AND GroupName='" + group + "'";
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    where += " AND RoleName LIKE '%" + keyword + "%'";
                }
                var list = systemService.GetSystemRolePage(page, size, where, "", 0);
                return Json(new { status = 1, msg = "OK", total = list.Total, data = list.Items });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("角色新增")]
        public IActionResult RoleAdd()
        {
            var list = systemService.GetSystemRoleGroupPage(1, 10000, "", "", 0);
            if (list.Total < 1)
            {
                throw new Exception("系统未定义角色分组");
            }
            List<Huanr.Models.NativeSoil.tSystemRoleGroup> tSystemRoleGroupList = list.Items;
            string selectedGroupName = Request.Query["key"];
            if (tSystemRoleGroupList.Find(o => o.GroupName == selectedGroupName) == null)
            {
                selectedGroupName = "";
            }

            ViewBag.tSystemRoleGroupList = tSystemRoleGroupList;
            ViewBag.selectedGroupName = selectedGroupName;

            return View();
        }
        //[Filters.PermissionActionFilter("角色新增")]
        public JsonResult RoleAddSave(Huanr.Models.NativeSoil.tSystemRole role)
        {
            try
            {
                role.RoleCreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(role.RoleName))
                {
                    throw new Exception("角色名称不能为空");
                }
                if (string.IsNullOrEmpty(role.RoleRemark))
                {
                    role.RoleRemark = "";
                }
                if (systemService.InsertSystemRole(role) < 1)
                {
                    throw new Exception("角色新增失败");
                }
                return Json(new { status = 1, msg = "角色新增成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("角色编辑")]
        public IActionResult RoleEdit()
        {
            var list = systemService.GetSystemRoleGroupPage(1, 10000, "", "", 0);
            if (list.Total < 1)
            {
                throw new Exception("系统未定义角色分组");
            }
            List<Huanr.Models.NativeSoil.tSystemRoleGroup> tSystemRoleGroupList = list.Items;
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemRole tSystemRole = null;
            if (!string.IsNullOrEmpty(key))
            {
                tSystemRole = systemService.GetSystemRole(key);
            }
            if (tSystemRole == null)
            {
                throw new Exception("要编辑的角色不存在或已经被删除");
            }

            ViewBag.tSystemRoleGroupList = tSystemRoleGroupList;
            ViewBag.tSystemRole = tSystemRole;

            return View();
        }
        //[Filters.PermissionActionFilter("角色编辑")]
        public JsonResult RoleEditSave()
        {
            try
            {
                var nvc = base.ConvertToNameValueCollection(Request.Form);
                if (systemService.UpdateSystemRole(nvc) < 1)
                {
                    throw new Exception("角色更新失败");
                }
                return Json(new { status = 1, msg = "角色更新成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("角色明细")]
        public IActionResult RoleDetail()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemRole tSystemRole = null;
            if (!string.IsNullOrEmpty(key))
            {
                tSystemRole = systemService.GetSystemRole(key);
            }
            if (tSystemRole == null)
            {
                throw new Exception("要查看的角色不存在或已经被删除");
            }

            ViewBag.tSystemRole = tSystemRole;

            return View();
        }
        //[Filters.PermissionActionFilter("角色删除")]
        public JsonResult RoleDelete()
        {
            try
            {
                string key = Request.Query["key"];
                if (!string.IsNullOrEmpty(key))
                {
                    systemService.DeleteSystemRole(new Huanr.Models.NativeSoil.tSystemRole { RoleName = key });
                }
                return Json(new { status = 1, msg = "角色删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("权限分配")]
        public IActionResult RolePermissonAssign()
        {
            string key = Request.Query["key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("缺少必须的参数");
            }
            var tSystemRole = systemService.GetSystemRole(key);
            if (tSystemRole==null)
            {
                throw new Exception("指定参数的角色不存在");
            }
            var permissionGroupList = systemService.GetSystemPermissionGroupAll();
            var permissionList = systemService.GetSystemPermissionAll();
            var rolePermissionList = systemService.GetSystemPermissionByRoleName(tSystemRole.RoleName);

            ViewBag.tSystemRole = tSystemRole;
            ViewBag.permissionGroupList = permissionGroupList;
            ViewBag.permissionList = permissionList;
            ViewBag.rolePermissionList = rolePermissionList;

            return View();
        }
        //[Filters.PermissionActionFilter("权限分配")]
        public JsonResult RolePermissonAssignSave()
        {
            try
            {
                string roleName = Request.Form["RoleName"];
                Huanr.Models.NativeSoil.tSystemRole tSystemRole = null;
                if (!string.IsNullOrEmpty(roleName))
                {
                    tSystemRole = systemService.GetSystemRole(roleName);
                }
                if (tSystemRole == null)
                {
                    throw new Exception("错误的角色参数");
                }
                string permissionNameStr = Request.Form["Permissions"];
                string[] permissionNames = new string[] { };
                if (!string.IsNullOrEmpty(permissionNameStr))
                {
                    permissionNames = permissionNameStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                var permissionList = systemService.GetSystemPermissionAll();
                foreach(var pn in permissionNames)
                {
                    if (permissionList.Find(o => o.PermissionName == pn) == null)
                    {
                        throw new Exception("权限“"+pn+"”不存在");
                    }
                }
                int result = systemService.UpdateRolePermission(roleName, permissionNames.ToList());
                return Json(new { status = 1, msg = "角色权限更新成功，受影响"+ result + "条" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("菜单分配")]
        public IActionResult RoleMenuAssign()
        {
            string key = Request.Query["key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("缺少必须的参数");
            }
            var tSystemRole = systemService.GetSystemRole(key);
            if (tSystemRole == null)
            {
                throw new Exception("指定参数的角色不存在");
            }
            var menuGroupList = systemService.GetSystemMenuGroupAll();
            var menuList = systemService.GetSystemMenuAll();
            var roleMenuList = systemService.GetSystemMenuByRoleName(tSystemRole.RoleName);

            ViewBag.tSystemRole = tSystemRole;
            ViewBag.menuGroupList = menuGroupList;
            ViewBag.menuList = menuList;
            ViewBag.roleMenuList = roleMenuList;

            return View();
        }
        //[Filters.PermissionActionFilter("菜单分配")]
        public JsonResult RoleMenuAssignSave()
        {
            try
            {
                string roleName = Request.Form["RoleName"];
                Huanr.Models.NativeSoil.tSystemRole tSystemRole = null;
                if (!string.IsNullOrEmpty(roleName))
                {
                    tSystemRole = systemService.GetSystemRole(roleName);
                }
                if (tSystemRole == null)
                {
                    throw new Exception("错误的角色参数");
                }
                string menuIdStr = Request.Form["menu"];
                string[] menuIdStrArray = new string[] { };
                if (!string.IsNullOrEmpty(menuIdStr))
                {
                    menuIdStrArray = menuIdStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                Guid[] menuIds = new Guid[menuIdStrArray.Length];
                for(var i=0;i<menuIdStrArray.Length;i++)
                {
                    menuIds[i] = Guid.Parse(menuIdStrArray[i]);
                }

                var menuList = systemService.GetSystemMenuAll();
                foreach (var menuId in menuIds)
                {
                    if (menuList.Find(o => o.MenuID == menuId) == null)
                    {
                        throw new Exception("菜单编号“" + menuId + "”不存在");
                    }
                }
                int result = systemService.UpdateRoleMenu(roleName, menuIds.ToList());
                return Json(new { status = 1, msg = "菜单权限更新成功，受影响" + result + "条" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        #endregion

        #region -- menuGroup --
        //[Filters.PermissionActionFilter("菜单分组列表")]
        public IActionResult MenuGroupList()
        {
            return View();
        }
        //[Filters.PermissionActionFilter("菜单分组列表")]
        public JsonResult MenuGroupListPage(long page, long size)
        {
            try
            {
                string keyword = Request.Query["keyword"];
                string where = "GroupDeleteStatus=0";
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = keyword.Trim();
                    if (keyword.Length > 30)
                    {
                        keyword = keyword.Substring(0, 30);
                    }
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    where += " AND GroupName LIKE '%" + keyword + "%'";
                }
                var list = systemService.GetSystemMenuGroupPage(page, size, where, "", 0);
                return Json(new { status = 1, msg = "OK", total = list.Total, data = list.Items });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("菜单分组新增")]
        public IActionResult MenuGroupAdd()
        {
            return View();
        }
        //[Filters.PermissionActionFilter("菜单分组新增")]
        public JsonResult MenuGroupAddSave(Huanr.Models.NativeSoil.tSystemMenuGroup menuGroup)
        {
            try
            {
                menuGroup.GroupCreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(menuGroup.GroupName))
                {
                    throw new Exception("菜单分组名称不能为空");
                }
                if (string.IsNullOrEmpty(menuGroup.GroupRemark))
                {
                    menuGroup.GroupRemark = "";
                }
                if (systemService.InsertSystemMenuGroup(menuGroup) < 1)
                {
                    throw new Exception("菜单分组新增失败");
                }
                return Json(new { status = 1, msg = "菜单分组新增成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("菜单分组编辑")]
        public IActionResult MenuGroupEdit()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemMenuGroup tSystemMenuGroup = null;
            if (!string.IsNullOrEmpty(key))
            {
                Guid id;
                if (Guid.TryParse(key, out id))
                {
                    tSystemMenuGroup = systemService.GetSystemMenuGroup(id);
                }
            }
            if (tSystemMenuGroup == null)
            {
                throw new Exception("要编辑的菜单分组不存在或已经被删除");
            }

            ViewBag.tSystemMenuGroup = tSystemMenuGroup;

            return View();
        }
        //[Filters.PermissionActionFilter("菜单分组编辑")]
        public JsonResult MenuGroupEditSave()
        {
            try
            {
                var nvc = base.ConvertToNameValueCollection(Request.Form);
                if (systemService.UpdateSystemMenuGroup(nvc) < 1)
                {
                    throw new Exception("菜单分组更新失败");
                }
                return Json(new { status = 1, msg = "菜单分组更新成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("菜单分组明细")]
        public IActionResult MenuGroupDetail()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemMenuGroup tSystemMenuGroup = null;
            if (!string.IsNullOrEmpty(key))
            {
                Guid id;
                if (Guid.TryParse(key, out id))
                {
                    tSystemMenuGroup = systemService.GetSystemMenuGroup(id);
                }
            }
            if (tSystemMenuGroup == null)
            {
                throw new Exception("要查看的菜单分组不存在或已经被删除");
            }

            ViewBag.tSystemMenuGroup = tSystemMenuGroup;

            return View();
        }
        //[Filters.PermissionActionFilter("菜单分组删除")]
        public JsonResult MenuGroupDelete()
        {
            try
            {
                string key = Request.Query["key"];
                if (!string.IsNullOrEmpty(key))
                {
                    Guid id;
                    if (Guid.TryParse(key, out id))
                    {
                        systemService.DeleteSystemMenuGroup(new Huanr.Models.NativeSoil.tSystemMenuGroup { GroupID = id });
                    }
                }
                return Json(new { status = 1, msg = "菜单分组删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        #endregion

        #region -- menu --
        //[Filters.PermissionActionFilter("菜单列表")]
        public IActionResult MenuList()
        {
            var list = systemService.GetSystemMenuGroupPage(1, 10000, "GroupDeleteStatus=0", "GroupSort,GroupName", 0);
            List<Huanr.Models.NativeSoil.tSystemMenuGroup> tSystemMenuGroupList = list.Items;

            ViewBag.tSystemMenuGroupList = tSystemMenuGroupList;
            return View();
        }
        //[Filters.PermissionActionFilter("菜单列表")]
        public JsonResult MenuListPage(long page, long size)
        {
            try
            {
                string keyword = Request.Query["keyword"];
                string group= Request.Query["group"];
                Guid groupID = Guid.Empty;
                string where = "MenuDeleteStatus=0";
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = keyword.Trim();
                    if (keyword.Length > 30)
                    {
                        keyword = keyword.Substring(0, 30);
                    }
                }
                if (!string.IsNullOrEmpty(group))
                {
                    Guid.TryParse(group, out groupID);
                }
                if (groupID != Guid.Empty)
                {
                    where += " AND GroupID='" + groupID + "'";
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    where += " AND MenuName LIKE '%" + keyword + "%'";
                }
                var list = systemService.GetSystemMenuPage(page, size, where, "GroupID,MenuSort,MenuName", 0);
                return Json(new { status = 1, msg = "OK", total = list.Total, data = list.Items });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("菜单新增")]
        public IActionResult MenuAdd()
        {
            var list = systemService.GetSystemMenuGroupPage(1, 10000, "GroupDeleteStatus=0", "GroupSort,GroupName", 0);
            List<Huanr.Models.NativeSoil.tSystemMenuGroup> tSystemMenuGroupList = list.Items;
            string key = Request.Query["key"];
            Guid selectedGroupID = Guid.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                Guid.TryParse(key, out selectedGroupID);
            }

            ViewBag.tSystemMenuGroupList = tSystemMenuGroupList;
            ViewBag.selectedGroupID = selectedGroupID;

            return View();
        }
        //[Filters.PermissionActionFilter("菜单新增")]
        public JsonResult MenuAddSave(Huanr.Models.NativeSoil.tSystemMenu menu)
        {
            try
            {
                menu.MenuID = Guid.NewGuid();
                menu.MenuCreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(menu.MenuName))
                {
                    throw new Exception("菜单名称不能为空");
                }
                if (string.IsNullOrEmpty(menu.MenuLink))
                {
                    throw new Exception("菜单链接不能为空");
                }
                if (string.IsNullOrEmpty(menu.MenuRemark))
                {
                    menu.MenuRemark = "";
                }
                if (systemService.InsertSystemMenu(menu) < 1)
                {
                    throw new Exception("菜单新增失败");
                }
                return Json(new { status = 1, msg = "菜单新增成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("菜单编辑")]
        public IActionResult MenuEdit()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemMenu tSystemMenu = null;
            if (!string.IsNullOrEmpty(key))
            {
                Guid id;
                if (Guid.TryParse(key, out id))
                {
                    tSystemMenu = systemService.GetSystemMenu(id);
                }
            }
            if (tSystemMenu == null)
            {
                throw new Exception("要编辑的菜单不存在或已经被删除");
            }
            var list = systemService.GetSystemMenuGroupPage(1, 10000, "GroupDeleteStatus=0", "GroupSort,GroupName", 0);
            List<Huanr.Models.NativeSoil.tSystemMenuGroup> tSystemMenuGroupList = list.Items;

            ViewBag.tSystemMenuGroupList = tSystemMenuGroupList;
            ViewBag.tSystemMenu = tSystemMenu;

            return View();
        }
        //[Filters.PermissionActionFilter("菜单编辑")]
        public JsonResult MenuEditSave()
        {
            try
            {
                var nvc = base.ConvertToNameValueCollection(Request.Form);
                if (systemService.UpdateSystemMenu(nvc) < 1)
                {
                    throw new Exception("菜单更新失败");
                }
                return Json(new { status = 1, msg = "菜单更新成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("菜单明细")]
        public IActionResult MenuDetail()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemMenu tSystemMenu = null;
            if (!string.IsNullOrEmpty(key))
            {
                Guid id;
                if (Guid.TryParse(key, out id))
                {
                    tSystemMenu = systemService.GetSystemMenu(id);
                }
            }
            if (tSystemMenu == null)
            {
                throw new Exception("要查看的菜单不存在或已经被删除");
            }
            var list = systemService.GetSystemMenuGroupPage(1, 10000, "GroupDeleteStatus=0", "GroupSort,GroupName", 0);
            List<Huanr.Models.NativeSoil.tSystemMenuGroup> tSystemMenuGroupList = list.Items;

            ViewBag.tSystemMenu = tSystemMenu;
            ViewBag.tSystemMenuGroupList = tSystemMenuGroupList;

            return View();
        }
        //[Filters.PermissionActionFilter("菜单删除")]
        public JsonResult MenuDelete()
        {
            try
            {
                string key = Request.Query["key"];
                if (!string.IsNullOrEmpty(key))
                {
                    Guid id;
                    if (Guid.TryParse(key, out id))
                    {
                        systemService.DeleteSystemMenu(new Huanr.Models.NativeSoil.tSystemMenu { MenuID = id });
                    }
                }
                return Json(new { status = 1, msg = "菜单删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        #endregion

        #region -- permissionGroup --
        //[Filters.PermissionActionFilter("权限分组列表")]
        public IActionResult PermissionGroupList()
        {
            return View();
        }
        //[Filters.PermissionActionFilter("权限分组列表")]
        public JsonResult PermissionGroupListPage(long page, long size)
        {
            try
            {
                string keyword = Request.Query["keyword"];
                string where = "GroupDeleteStatus=0";
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = keyword.Trim();
                    if (keyword.Length > 30)
                    {
                        keyword = keyword.Substring(0, 30);
                    }
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    where += " AND GroupName LIKE '%" + keyword + "%'";
                }
                var list = systemService.GetSystemPermissionGroupPage(page, size, where, "", 0);
                return Json(new { status = 1, msg = "OK", total = list.Total, data = list.Items });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("权限分组新增")]
        public IActionResult PermissionGroupAdd()
        {
            return View();
        }
        //[Filters.PermissionActionFilter("权限分组新增")]
        public JsonResult PermissionGroupAddSave(Huanr.Models.NativeSoil.tSystemPermissionGroup menuGroup)
        {
            try
            {
                menuGroup.GroupCreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(menuGroup.GroupName))
                {
                    throw new Exception("权限分组名称不能为空");
                }
                if (string.IsNullOrEmpty(menuGroup.GroupRemark))
                {
                    menuGroup.GroupRemark = "";
                }
                if (systemService.InsertSystemPermissionGroup(menuGroup) < 1)
                {
                    throw new Exception("权限分组新增失败");
                }
                return Json(new { status = 1, msg = "权限分组新增成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("权限分组编辑")]
        public IActionResult PermissionGroupEdit()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemPermissionGroup tSystemPermissionGroup = null;
            if (!string.IsNullOrEmpty(key))
            {
                tSystemPermissionGroup = systemService.GetSystemPermissionGroup(key);
            }
            if (tSystemPermissionGroup == null)
            {
                throw new Exception("要编辑的权限分组不存在或已经被删除");
            }

            ViewBag.tSystemPermissionGroup = tSystemPermissionGroup;

            return View();
        }
        //[Filters.PermissionActionFilter("权限分组编辑")]
        public JsonResult PermissionGroupEditSave()
        {
            try
            {
                var nvc = base.ConvertToNameValueCollection(Request.Form);
                if (systemService.UpdateSystemPermissionGroup(nvc) < 1)
                {
                    throw new Exception("权限分组更新失败");
                }
                return Json(new { status = 1, msg = "权限分组更新成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("权限分组明细")]
        public IActionResult PermissionGroupDetail()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemPermissionGroup tSystemPermissionGroup = null;
            if (!string.IsNullOrEmpty(key))
            {
                tSystemPermissionGroup = systemService.GetSystemPermissionGroup(key);
            }
            if (tSystemPermissionGroup == null)
            {
                throw new Exception("要查看的权限分组不存在或已经被删除");
            }

            ViewBag.tSystemPermissionGroup = tSystemPermissionGroup;

            return View();
        }
        //[Filters.PermissionActionFilter("权限分组删除")]
        public JsonResult PermissionGroupDelete()
        {
            try
            {
                string key = Request.Query["key"];
                if (!string.IsNullOrEmpty(key))
                {
                    systemService.DeleteSystemPermissionGroup(new Huanr.Models.NativeSoil.tSystemPermissionGroup { GroupName = key });
                }
                return Json(new { status = 1, msg = "权限分组删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        #endregion

        #region -- permission --
        //[Filters.PermissionActionFilter("权限列表")]
        public IActionResult PermissionList()
        {
            var list = systemService.GetSystemPermissionGroupPage(1, 10000, "GroupDeleteStatus=0", "GroupSort,GroupName", 0);
            List<Huanr.Models.NativeSoil.tSystemPermissionGroup> tSystemPermissionGroupList = list.Items;

            ViewBag.tSystemPermissionGroupList = tSystemPermissionGroupList;
            return View();
        }
        //[Filters.PermissionActionFilter("权限列表")]
        public JsonResult PermissionListPage(long page, long size)
        {
            try
            {
                string group = Request.Query["group"];
                string keyword = Request.Query["keyword"];
                string where = "PermissionDeleteStatus=0";
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = keyword.Trim();
                    if (keyword.Length > 30)
                    {
                        keyword = keyword.Substring(0, 30);
                    }
                }
                if (!string.IsNullOrEmpty(group))
                {
                    where += " AND GroupName='" + group + "'";
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    where += " AND PermissionName LIKE '%" + keyword + "%'";
                }
                var list = systemService.GetSystemPermissionPage(page, size, where, "GroupName,PermissionSort,PermissionName", 0);
                return Json(new { status = 1, msg = "OK", total = list.Total, data = list.Items });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("权限新增")]
        public IActionResult PermissionAdd()
        {
            var list = systemService.GetSystemPermissionGroupPage(1, 10000, "", "", 0);
            if (list.Total < 1)
            {
                throw new Exception("系统未定义权限分组");
            }
            List<Huanr.Models.NativeSoil.tSystemPermissionGroup> tSystemPermissionGroupList = list.Items;
            string selectedGroupName = Request.Query["key"];
            if (tSystemPermissionGroupList.Find(o => o.GroupName == selectedGroupName) == null)
            {
                selectedGroupName = "";
            }

            ViewBag.tSystemPermissionGroupList = tSystemPermissionGroupList;
            ViewBag.selectedGroupName = selectedGroupName;

            return View();
        }
        //[Filters.PermissionActionFilter("权限新增")]
        public JsonResult PermissionAddSave(Huanr.Models.NativeSoil.tSystemPermission role)
        {
            try
            {
                role.PermissionCreateTime = DateTime.Now;
                role.PermissionName = ("" + role.PermissionName).Replace(",", "");
                if (string.IsNullOrEmpty(role.PermissionName))
                {
                    throw new Exception("权限名称不能为空");
                }
                if (string.IsNullOrEmpty(role.PermissionRemark))
                {
                    role.PermissionRemark = "";
                }
                if (systemService.InsertSystemPermission(role) < 1)
                {
                    throw new Exception("权限新增失败");
                }
                return Json(new { status = 1, msg = "权限新增成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("权限编辑")]
        public IActionResult PermissionEdit()
        {
            var list = systemService.GetSystemPermissionGroupPage(1, 10000, "", "", 0);
            if (list.Total < 1)
            {
                throw new Exception("系统未定义权限分组");
            }
            List<Huanr.Models.NativeSoil.tSystemPermissionGroup> tSystemPermissionGroupList = list.Items;
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemPermission tSystemPermission = null;
            if (!string.IsNullOrEmpty(key))
            {
                tSystemPermission = systemService.GetSystemPermission(key);
            }
            if (tSystemPermission == null)
            {
                throw new Exception("要编辑的权限不存在或已经被删除");
            }

            ViewBag.tSystemPermissionGroupList = tSystemPermissionGroupList;
            ViewBag.tSystemPermission = tSystemPermission;

            return View();
        }
        //[Filters.PermissionActionFilter("权限编辑")]
        public JsonResult PermissionEditSave()
        {
            try
            {
                var nvc = base.ConvertToNameValueCollection(Request.Form);
                string name = nvc["PermissionName"];
                name=(""+name).Replace(",", "");
                nvc["PermissionName"] = name;
                if (systemService.UpdateSystemPermission(nvc) < 1)
                {
                    throw new Exception("权限更新失败");
                }
                return Json(new { status = 1, msg = "权限更新成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("权限明细")]
        public IActionResult PermissionDetail()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tSystemPermission tSystemPermission = null;
            if (!string.IsNullOrEmpty(key))
            {
                tSystemPermission = systemService.GetSystemPermission(key);
            }
            if (tSystemPermission == null)
            {
                throw new Exception("要查看的权限不存在或已经被删除");
            }

            ViewBag.tSystemPermission = tSystemPermission;

            return View();
        }
        //[Filters.PermissionActionFilter("权限删除")]
        public JsonResult PermissionDelete()
        {
            try
            {
                string key = Request.Query["key"];
                if (!string.IsNullOrEmpty(key))
                {
                    systemService.DeleteSystemPermission(new Huanr.Models.NativeSoil.tSystemPermission { PermissionName = key });
                }
                return Json(new { status = 1, msg = "权限删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        #endregion

        #region -- baseCategory --
        //[Filters.PermissionActionFilter("基础分类列表")]
        public IActionResult CategoryList()
        {
            return View();
        }
        //[Filters.PermissionActionFilter("基础分类列表")]
        public JsonResult CategoryListByParent(string parent)
        {
            try
            {
                var list = systemService.GetBaseCategoryByParent(parent);
                return Json(new { status = 1, msg = "OK", total = list.Count, data = list });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("基础分类明细")]
        public JsonResult CategoryDetail(string key)
        {
            try
            {
                var val = systemService.GetBaseCategory(key);
                return Json(new { status = 1, msg = "OK", total = val != null?1:0, data = val });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("基础分类新增")]
        public IActionResult CategoryAdd()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tBaseCategory tBaseCategoryParent = null;
            if (!string.IsNullOrEmpty(key))
            {
                tBaseCategoryParent = systemService.GetBaseCategory(key);
                if(tBaseCategoryParent == null)
                {
                    throw new Exception("指定的归属基础分类不存在或已经被删除");
                }
            }

            ViewBag.tBaseCategoryParent = tBaseCategoryParent;

            return View();
        }
        //[Filters.PermissionActionFilter("基础分类新增")]
        public JsonResult CategoryAddSave(Huanr.Models.NativeSoil.tBaseCategory category)
        {
            try
            {
                category.BCreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(category.BID))
                {
                    throw new Exception("基础分类编号不能为空");
                }
                if (string.IsNullOrEmpty(category.BName))
                {
                    throw new Exception("基础分类名称不能为空");
                }
                if (string.IsNullOrEmpty(category.BAsname))
                {
                    category.BAsname = category.BName;
                }
                if (string.IsNullOrEmpty(category.BParentID))
                {
                    throw new Exception("基础分类归属不能为空");
                }
                if (category.BParentID != "0")
                {
                    var tBaseCategoryParent = systemService.GetBaseCategory(category.BParentID);
                    if (tBaseCategoryParent == null)
                    {
                        throw new Exception("地区城市指定的归属无效");
                    }
                    category.BPath = tBaseCategoryParent.BPath + tBaseCategoryParent.BID + "/";
                }
                else
                {
                    category.BPath = "/0/";
                }
                if (systemService.HasBaseCategory(category.BParentID, category.BName))
                {
                    throw new Exception("同一父分类下已经存在该基础分类名称");
                }
                if (systemService.InsertBaseCategory(category) < 1)
                {
                    throw new Exception("基础分类新增失败");
                }
                return Json(new { status = 1, msg = "基础分类新增成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("基础分类编辑")]
        public IActionResult CategoryEdit()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tBaseCategory tBaseCategory = null;
            if (!string.IsNullOrEmpty(key))
            {
                tBaseCategory = systemService.GetBaseCategory(key);
                if (tBaseCategory == null)
                {
                    throw new Exception("要编辑的基础分类不存在或已经被删除");
                }
            }
           
            Huanr.Models.NativeSoil.tBaseCategory tBaseCategoryParent = systemService.GetBaseCategory(tBaseCategory.BParentID);
            if (tBaseCategoryParent == null)
            {
                tBaseCategoryParent = new Models.NativeSoil.tBaseCategory
                {
                    BAsname = "",
                    BCreateTime = DateTime.Now,
                    BDeleteStatus = false,
                    BID = "0",
                    BName = "根分类",
                    BParentID = "",
                    BPath = "",
                    BSort = 0
                };
            }

            ViewBag.tBaseCategory = tBaseCategory;
            ViewBag.tBaseCategoryParent = tBaseCategoryParent;

            return View();
        }
        //[Filters.PermissionActionFilter("基础分类编辑")]
        public JsonResult CategoryEditSave()
        {
            try
            {
                var nvc = base.ConvertToNameValueCollection(Request.Form);
                if (systemService.UpdateBaseCategory(nvc) < 1)
                {
                    throw new Exception("基础分类更新失败");
                }
                return Json(new { status = 1, msg = "基础分类更新成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("基础分类明细")]
        public IActionResult CategoryDetail()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tBaseCategory tBaseCategory = null;
            if (!string.IsNullOrEmpty(key))
            {
                tBaseCategory = systemService.GetBaseCategory(key);
                if (tBaseCategory == null)
                {
                    throw new Exception("要查看的基础分类不存在或已经被删除");
                }
            }
            
            Huanr.Models.NativeSoil.tBaseCategory tBaseCategoryParent = systemService.GetBaseCategory(tBaseCategory.BParentID);
            var tBaseCategoryChildren = systemService.GetBaseCategoryByParent(key);

            ViewBag.tBaseCategory = tBaseCategory;
            ViewBag.tBaseCategoryParent = tBaseCategoryParent;
            ViewBag.tBaseCategoryChildren = tBaseCategoryChildren;

            return View();
        }
        //[Filters.PermissionActionFilter("基础分类删除")]
        public JsonResult CategoryDelete()
        {
            try
            {
                string key = Request.Query["key"];
                if (!string.IsNullOrEmpty(key))
                {
                    systemService.DeleteBaseCategory(new Huanr.Models.NativeSoil.tBaseCategory { BID = key });
                }
                return Json(new { status = 1, msg = "基础分类删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        #endregion

        #region -- area --
        //[Filters.PermissionActionFilter("地区城市列表")]
        public IActionResult AreaList()
        {
            return View();
        }
        //[Filters.PermissionActionFilter("地区城市列表")]
        public JsonResult AreaListByParent(string parent)
        {
            try
            {
                var list = systemService.GetAreaByParent(parent);
                return Json(new { status = 1, msg = "OK", total = list.Count, data = ListJsonCompress(list) });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("地区城市明细")]
        public JsonResult AreaDetail(string key)
        {
            try
            {
                var val = systemService.GetArea(key);
                return Json(new { status = 1, msg = "OK", total = val != null ? 1 : 0, data = val });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message, total = 0, data = new string[] { } });
            }
        }
        //[Filters.PermissionActionFilter("地区城市新增")]
        public IActionResult AreaAdd()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tArea tAreaParent = null;
            if (!string.IsNullOrEmpty(key))
            {
                tAreaParent = systemService.GetArea(key);
            }
            if (key != "0" && tAreaParent == null)
            {
                throw new Exception("归属的地区城市不存在或已经被删除");
            }
            if (key == "0")
            {
                tAreaParent = new Huanr.Models.NativeSoil.tArea
                {
                    AreaCode = "0",
                    AreaName = "根",
                    AreaNameSuffix = "",
                    AreaParent = "-1",
                    AreaPath = "/",
                    AreaRemark = ""
                };
            }

            ViewBag.tAreaParent = tAreaParent;

            return View();
        }
        //[Filters.PermissionActionFilter("地区城市新增")]
        public JsonResult AreaAddSave(/*Huanr.Models.NativeSoil.tArea area*/)//怪异：不指定FromForm读取不到输入
        {
            try
            {
                var area = base.EntityGetByRequestForm<Huanr.Models.NativeSoil.tArea>();
                if (string.IsNullOrEmpty(area.AreaName))
                {
                    throw new Exception("地区城市名称不能为空");
                }
                if (string.IsNullOrEmpty(area.AreaNameSuffix))
                {
                    area.AreaNameSuffix = "";
                }
                if (string.IsNullOrEmpty(area.AreaPath))
                {
                    area.AreaPath = "";
                }
                if (string.IsNullOrEmpty(area.AreaRemark))
                {
                    area.AreaRemark = "";
                }
                if (area.AreaParent != "0")
                {
                    var tAreaParent= systemService.GetArea(area.AreaParent);
                    if (tAreaParent == null)
                    {
                        throw new Exception("地区城市指定的归属无效");
                    }
                    area.AreaPath = tAreaParent.AreaPath+ tAreaParent.AreaCode + "/";
                }
                else
                {
                    area.AreaPath = "/0/";
                }
                if (systemService.HasArea(area.AreaParent, area.AreaName))
                {
                    throw new Exception("同一父区域城市下已经存在该区域城市名称");
                }
                if (systemService.InsertArea(area) < 1)
                {
                    throw new Exception("地区城市新增失败");
                }
                return Json(new { status = 1, msg = "地区城市新增成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("地区城市编辑")]
        public IActionResult AreaEdit()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tArea tArea = null;
            if (!string.IsNullOrEmpty(key))
            {
                tArea = systemService.GetArea(key);
            }
            if (tArea == null)
            {
                throw new Exception("要编辑的地区城市不存在或已经被删除");
            }
            Huanr.Models.NativeSoil.tArea tAreaParent = null;
            if (tArea.AreaParent != "0")
            {
                tAreaParent = systemService.GetArea(tArea.AreaParent);
                if (tAreaParent == null)
                {
                    throw new Exception("要编辑的地区城市其归属不存在或已经被删除");
                }
            }
            else
            {
                tAreaParent = new Huanr.Models.NativeSoil.tArea
                {
                    AreaCode = "0",
                    AreaName = "根",
                    AreaNameSuffix = "",
                    AreaParent = "-1",
                    AreaPath = "/",
                    AreaRemark = ""
                };
            }

            ViewBag.tArea = tArea;
            ViewBag.tAreaParent = tAreaParent;

            return View();
        }
        //[Filters.PermissionActionFilter("地区城市编辑")]
        public JsonResult AreaEditSave()
        {
            try
            {
                var nvc = base.ConvertToNameValueCollection(Request.Form);
                if (systemService.UpdateArea(nvc) < 1)
                {
                    throw new Exception("地区城市更新失败");
                }
                return Json(new { status = 1, msg = "地区城市更新成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        //[Filters.PermissionActionFilter("地区城市明细")]
        public IActionResult AreaDetail()
        {
            string key = Request.Query["key"];
            Huanr.Models.NativeSoil.tArea tArea = null;
            if (!string.IsNullOrEmpty(key))
            {
                tArea = systemService.GetArea(key);
            }
            if (tArea == null)
            {
                throw new Exception("要查看的地区城市不存在或已经被删除");
            }
            Huanr.Models.NativeSoil.tArea tAreaParent = systemService.GetArea(tArea.AreaParent);
            var tAreaChildren = systemService.GetAreaByParent(tArea.AreaCode);

            ViewBag.tArea = tArea;
            ViewBag.tAreaParent = tAreaParent;
            ViewBag.tAreaChildren = tAreaChildren;

            return View();
        }
        //[Filters.PermissionActionFilter("地区城市删除")]
        public JsonResult AreaDelete()
        {
            try
            {
                string key = Request.Query["key"];
                if (!string.IsNullOrEmpty(key))
                {
                    systemService.DeleteArea(new Huanr.Models.NativeSoil.tArea { AreaCode = key });
                }
                return Json(new { status = 1, msg = "地区城市删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, msg = ex.Message });
            }
        }
        #endregion

    }
}