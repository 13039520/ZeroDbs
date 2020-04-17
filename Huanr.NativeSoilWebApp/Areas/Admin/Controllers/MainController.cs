using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Huanr.NativeSoilWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Filters.OnlineActionFilter()]
    public class MainController : Huanr.NativeSoilWebApp.Controllers.BaseController
    {
        Huanr.Services.UserService userService = null;
        Huanr.Services.SystemService systemService = null;
        public MainController(ZeroDbs.Interfaces.IDbService zeroService) : base(zeroService) {
            userService = new Huanr.Services.UserService(zeroService);
            systemService = new Huanr.Services.SystemService(currentZeroService);
        }

        public IActionResult Index()
        {
            var currentUserSystemMenuList = userService.GetUserSystemMenuList(currentUser.UserID);
            var ids = currentUserSystemMenuList.Select(o => o.GroupID).Distinct().ToArray();
            List<Huanr.Models.NativeSoil.tSystemMenuGroup> currentUserSystemMenuGroupList = new List<Huanr.Models.NativeSoil.tSystemMenuGroup>();
            if (ids.Length > 0)
            {
                currentUserSystemMenuGroupList = systemService.GetSystemMenuGroupPage(1, 1000, "GroupID IN('"+ string.Join("','", ids) + "')", "", 0).Items;
            }
            var userRoleList = systemService.GetSystemRolePage(1, 1000, "RoleName IN(SELECT ConfigRoleName FROM T_UserRoleConfig WHERE ConfigUserID='" + currentUser.UserID+"')", "", 0).Items;

            ViewBag.currentUser = currentUser;
            ViewBag.currentUserRoleList = userRoleList;
            ViewBag.currentUserSystemMenuGroupList = currentUserSystemMenuGroupList;
            ViewBag.currentUserSystemMenuList = currentUserSystemMenuList;

            return View();
        }
        public IActionResult Welcome()
        {
            return View();
        }
        public IActionResult Login()
        {
            var md5 = currentZeroService.StrCommon.MD5_32("admin");
            ViewBag.md5 = md5;
            return View();
        }
        public JsonResult DoLogin()
        {
            string msg = "登录成功";
            int status = 1;
            try
            {
                var account = Request.Form["account"];
                if (string.IsNullOrEmpty(account))
                {
                    throw new Exception("账号不能为空");
                }
                var pwd = Request.Form["pwd"];
                if (string.IsNullOrEmpty(pwd))
                {
                    throw new Exception("密码不能为空");
                }
                var user = userService.GetUser(account, pwd);
                if (user == null)
                {
                    throw new Exception("账号或密码错误");
                }
                var temp = new Tools.UserOnlineHelper(HttpContext);
                temp.SetUserOnlineInfo(user);
            }
            catch (Exception ex)
            {
                status = 0;
                msg = ex.Message;
            }
            return new JsonResult(new { status=status,msg=msg,data=new string[] { },total=0 });
        }

        public JsonResult ListTestApi(long page, long size, string columns = "", string orderby = "")
        {
            var total = 123456;
            if (page < 1) { page = 1; }
            if (size < 1) { size = 1; }
            var pages = Convert.ToInt64(total % size == 0 ? total / size : (Math.Floor((decimal)total / size) + 1));
            if (page > pages)
            {
                page = pages;
            }
            var startIndex = page * size - size;
            var endIndex = startIndex + size;
            if (endIndex > total)
            {
                endIndex = total;
            }
            /*var list = new List<object>();
            while (startIndex < endIndex)
            {
                startIndex++;
                list.Add(new {
                    id=startIndex,
                    city="昆明",
                    name="张珊"+startIndex,
                    birthday = DateTime.Now.AddYears(-18),
                    createTime =DateTime.Now,
                    sex=(startIndex%2==0?1:2)
                });
            }*/
            var list2 = new List<object>(new object[] { 6, "id", "city", "name", "birthday", "createTime", "sex" });
            while (startIndex < endIndex)
            {
                startIndex++;
                list2.Add(startIndex);
                list2.Add("昆明");
                list2.Add("张珊" + startIndex);
                list2.Add(DateTime.Now.AddYears(-18));
                list2.Add(DateTime.Now);
                list2.Add((startIndex % 2 == 0 ? 1 : 2));
            }

            return new JsonResult(new { status = 1, total, data = list2, msg = "OK" }, new Newtonsoft.Json.JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        }
        public JsonResult areaInfo(int areaId = 0)
        {
            var dataOperator = new ZeroDbs.Interfaces.Common.DbOperator(currentZeroService);
            var list2 = dataOperator.Select<Huanr.Models.NativeSoil.tArea>("AreaParent='" + areaId+"'", "");
            return new JsonResult(new { status = 1, total = list2.Count, data = ListJsonCompress(list2), msg = "OK" }, new Newtonsoft.Json.JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        }
        public JsonResult areaAll()
        {
            var dataOperator = new ZeroDbs.Interfaces.Common.DbOperator(currentZeroService);
            var list2 = dataOperator.Select<Huanr.Models.NativeSoil.tArea>("AreaDeleteStatus=0");
            List<object> lis = ListJsonCompress(list2);

            return new JsonResult(new { status = 1, total = list2.Count, data = lis, msg = "OK" }, new Newtonsoft.Json.JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        }
        

        public JsonResult uploadFile()
        {
            if (Request.Form.Files.Count < 1)
            {
                return new JsonResult(new { status = 0, msg = "没有上传文件" });
            }
            var s = System.IO.Path.GetFileName(Request.Form.Files[0].FileName);
            var t = Request.Form.Files[0].Length;
            return new JsonResult(new { status = 1, msg = "上传成功：" + s, length = t });
        }
    }
}