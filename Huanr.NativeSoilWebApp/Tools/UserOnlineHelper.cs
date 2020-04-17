using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace  Huanr.NativeSoilWebApp.Tools
{
    #region --用户在线数据辅助--
    public class UserOnlineHelper
    {
        readonly string cookieName = "UserID";
        readonly ZeroDbs.Interfaces.IDbService zeroService = null;
        Microsoft.AspNetCore.Http.HttpContext httpContext = null;

        public UserOnlineHelper(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            this.httpContext = httpContext;
            zeroService = (ZeroDbs.Interfaces.IDbService)httpContext.RequestServices.GetService(typeof(ZeroDbs.Interfaces.IDbService));
            cookieName = HttpUtility.UrlEncode(zeroService.StrCommon.DESEncrypt(cookieName));
        }
        public void SetUserOnlineInfo(Huanr.Models.NativeSoil.tUser tUser)
        {
            if (httpContext != null)
            {
                string cookieValue = HttpUtility.UrlEncode(zeroService.StrCommon.DESEncrypt(tUser.UserID.ToString()));
                httpContext.Response.Cookies.Append(
                    cookieName,
                    cookieValue,
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Path = "/",
                        HttpOnly = true,
                        Expires = DateTime.Now.AddDays(15)
                    });
            }
        }
        public Guid GetOnlineUserID()
        {
            Guid userID = Guid.Empty;
            if (httpContext != null)
            {
                var Cookie = httpContext.Request.Cookies[cookieName];
                if (Cookie != null)
                {
                    string s = Cookie.ToString();
                    if (!string.IsNullOrEmpty(s))
                    {
                        try
                        {
                            s = zeroService.StrCommon.DESDecrypt(HttpUtility.UrlDecode(s));
                        }
                        catch { }
                        Guid.TryParse(s, out userID);
                    }
                }
            }
            return userID;
        }
        public Huanr.Models.NativeSoil.tUser GetOnlineUserInfo()
        {
           Huanr.Models.NativeSoil.tUser M = null;
            Guid uid = GetOnlineUserID();
            if (uid != Guid.Empty)
            {
                M = GetOnlineUserInfoByCache(uid);
            }
            return M;
        }
        private Huanr.Models.NativeSoil.tUser GetOnlineUserInfoByCache(Guid userId)
        {
           Huanr.Models.NativeSoil.tUser M = null;
            if (Guid.Equals(Guid.Empty, userId))
            {
                return M;
            }
            string cacheKey = "user" + userId.ToString();
            var obj = zeroService.Cache?.Get<Huanr.Models.NativeSoil.tUser>(cacheKey);
            if (obj != null)
            {
                M = obj;
            }
            else
            {
                var db = zeroService.DbGet<Huanr.Models.NativeSoil.tUser>();
                var list = db.Select<Huanr.Models.NativeSoil.tUser>(string.Format("UserID='{0}'", userId));
                if (list != null && list.Count > 0)
                {
                    M = list[0];
                    if (zeroService.Cache != null)
                    {
                        zeroService.Cache.Set<Huanr.Models.NativeSoil.tUser>(cacheKey, M, DateTime.Now.AddMinutes(20));
                    }
                }
            }
            return M;
        }
        public void ClearUserOnlineInfo()
        {
            if (httpContext != null)
            {
                Guid userID = GetOnlineUserID();
                if (!Guid.Equals(userID, Guid.Empty))
                {
                    string cacheKey = "user" + userID.ToString();
                    zeroService.Cache.Remove(cacheKey);
                    httpContext.Response.Cookies.Delete(cookieName);
                }
            }
        }

    }
    #endregion
}
