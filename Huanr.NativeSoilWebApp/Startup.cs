using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Huanr.NativeSoilWebApp
{
    public class Startup
    {
        IHttpContextAccessor _Accessor;

        public void ConfigureServices(IServiceCollection services)
        {
            //IHttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //IZeroService
            services.AddSingleton<ZeroDbs.Interfaces.IDbService, ZeroDbs.Interfaces.Common.DbService>((p) =>
            {
                var service = new ZeroDbs.Interfaces.Common.DbService(
                    new ZeroDbs.DataAccess.DbSearcher(Startup_OnZeroDbExecuteSqlEvent),
                    ZeroDbs.Logs.Factory.GetLogger("ZeroLog", 30),
                    new ZeroDbs.Caches.LocalMemCache(null)
                    );
                return service;
            });
            //HtmlEncoder
            services.AddSingleton(System.Text.Encodings.Web.HtmlEncoder.Create(System.Text.Unicode.UnicodeRanges.All));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            //mvc
            services.AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(); ;//禁止首字母小写(因为首字母小写有BUG)
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            _Accessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            app.UseStaticFiles();
            app.UseMvc(o =>
            {
                o.MapRoute("Areas", "{area:exists}/{controller=Main}/{action=Index}/{id?}");
                o.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Main", action = "Index", id = "" });
            });
        }

        private void Startup_OnZeroDbExecuteSqlEvent(object sender, ZeroDbs.Interfaces.Common.DbExecuteSqlEventArgs e)
        {
            var obj = this;
            string sql = "未获得相关Sql命令";
            if (e.ExecuteSql.Count > 0)
            {
                sql = string.Join(System.Environment.NewLine, e.ExecuteSql.ToArray());
            }

            var ipInfo = _Accessor.HttpContext.Connection.RemoteIpAddress;
            var url = _Accessor.HttpContext.Request.Scheme + "://" + _Accessor.HttpContext.Request.Host + _Accessor.HttpContext.Request.Path + _Accessor.HttpContext.Request.QueryString;

            string doAction = e.ExecuteType.ToString();
            ZeroDbs.Logs.Factory.GetLogger("Sql", 30).Writer(string.Format("DbKey={0}&Do={1}&Msg={2}&Ip={3}&Path={4}{5}{6}{7}",
                e.DbKey,
                doAction,
                e.Message,
                ipInfo.MapToIPv4(),
                url,
                System.Environment.NewLine,
                sql,
                System.Environment.NewLine));
        }

    }
}
