using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Huanr.NativeSoilWebApp.Tools
{
    public static class PageSeo
    {
        public static Huanr.Models.NativeSoil.tPageSeoSetting GetSeoModel(ZeroDbs.Interfaces.IDbService zeroService, string urlAbsolutePath)
        {
            Huanr.Models.NativeSoil.tPageSeoSetting m = null;
            urlAbsolutePath = urlAbsolutePath.ToLower().Trim();
            
            string key = zeroService.StrCommon.MD5_32(urlAbsolutePath);
            Huanr.Models.NativeSoil.tPageSeoSetting cacheData = zeroService.Cache != null ? zeroService.Cache.Get<Huanr.Models.NativeSoil.tPageSeoSetting>(key) : null;
            if (cacheData != null)
            {
                m = cacheData;
            }
            else
            {
                var db= zeroService.DbSearcher.GetDb<Huanr.Models.NativeSoil.tPageSeoSetting>();
                var list =  db.Select<Huanr.Models.NativeSoil.tPageSeoSetting>(string.Format("LOWER(PageSeoPath)='{0}'", urlAbsolutePath), "PageSeoSort DESC,PageSeoModifyTime DESC");
                if (list != null&& list.Count>0)
                {
                    m = list[0];
                }
                else
                {
                    m = new Huanr.Models.NativeSoil.tPageSeoSetting();
                    m.PageSeoTitle = "";
                    m.PageSeoKeywords = "";
                    m.PageSeoDescription = "";
                }
                if (zeroService.Cache != null)
                {
                    zeroService.Cache.Set<Huanr.Models.NativeSoil.tPageSeoSetting>(key, m, DateTime.Now.AddMinutes(5));
                }
            }

            return m;
        }

    }
}
