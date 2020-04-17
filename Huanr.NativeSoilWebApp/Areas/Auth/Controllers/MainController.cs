using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Huanr.NativeSoilWebApp.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class MainController : Huanr.NativeSoilWebApp.Controllers.BaseController
    {
        Huanr.Services.UserService userService = null;
        Huanr.Services.SystemService systemService = null;
        ZeroDbs.Interfaces.IDbService zeroService = null;
        public MainController(ZeroDbs.Interfaces.IDbService zeroService) : base(zeroService)
        {
            this.zeroService = zeroService;
            userService = new Huanr.Services.UserService(zeroService);
            systemService = new Huanr.Services.SystemService(currentZeroService);
        }

        public IActionResult Login(long page=1, long size=1)
        {
            /**/
            var data= zeroService.DataOperator.Page<Huanr.Models.Test.tDataTypes>(page, size, "");
            var total = data.Total;
            var items = data.Items;
            return View();
        }
        public IActionResult LoginSuccess()
        {
            if (currentUser == null)
            {
                Response.Redirect("/auth/main/login");
                return new EmptyResult();
            }

            var userRoleList = systemService.GetSystemRolePage(1, 1000, "RoleName IN(SELECT ConfigRoleName FROM T_UserRoleConfig WHERE ConfigUserID='" + currentUser.UserID + "')", "", 0).Items;

            ViewBag.currentUser = currentUser;
            ViewBag.currentUserRoleList = userRoleList;
            return View();
        }
        public IActionResult Logout()
        {
            if (currentUser != null)
            {
                var online = new Tools.UserOnlineHelper(HttpContext);
                online.ClearUserOnlineInfo();
            }
            Response.Redirect("/");
            return new EmptyResult();
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
            return new JsonResult(new { status = status, msg = msg, data = new string[] { }, total = 0 });
        }

    }
}