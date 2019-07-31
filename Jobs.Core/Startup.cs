using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.Dashboard.RecurringJobExtensions;
using Hangfire.Heartbeat;
using Hangfire.Heartbeat.Server;
using Hangfire.RecurringJobExtensions;
using Jobs.Core.Application;
using Jobs.Core.Common;
using Jobs.Core.Controllers;
using Jobs.Core.Extension;
using Jobs.Core.Filters;
using Jobs.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jobs.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc(opt =>
            {
                opt.Filters.Add<AdminAuthFilter>();

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IMessageService, MessageService>();
            //注入hangfire 服务
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
                x.UseRecurringJob(new JobProvider());
                x.UseConsole();
                x.UseFilter(new LogEverythingAttribute());
                x.UseHeartbeatPage(checkInterval: TimeSpan.FromSeconds(1));
                x.UseDashboardRecurringJobExtensions();
            });
            ApiConfig.HangfireLogUrl = Configuration["HangfireLogFileUrl"];
            //GlobalJobFilters.Filters.Add(new LogEverythingAttribute());
            //GlobalConfiguration.Configuration.UseAutofacActivator(container);
            //add dbcontext
            services.AddDbContextPool<JobsDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieJobsAuthInfo.AdminAuthCookieScheme;
                o.DefaultChallengeScheme = CookieJobsAuthInfo.AdminAuthCookieScheme;
                o.DefaultSignInScheme = CookieJobsAuthInfo.AdminAuthCookieScheme;
                o.DefaultSignOutScheme = CookieJobsAuthInfo.AdminAuthCookieScheme;
            }).AddCookie(CookieJobsAuthInfo.AdminAuthCookieScheme, o =>
            {
                o.LoginPath = "/Login";
            });
            //泛型注入到di
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseMvc();
            app.UseStaticFiles();
            //app.UseHangfireServer();//启动hangfire 服务
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}");

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            });
            //配置任务属性
            var jobOptions = new BackgroundJobServerOptions()
            {
                Queues = new[] { "default", "apis", "job" },//队列名称，只能小写
                WorkerCount = Environment.ProcessorCount * 5,//并发任务数
                
                ServerName = "hangfire"//服务器名称
            };
            app.UseHangfireServer(jobOptions,additionalProcesses:new []
            {
                //Hangfire.Heartbeat
                new ProcessMonitor(checkInterval:TimeSpan.FromSeconds(1))
            });

            //配置访问权限
            var options = new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                
            };
            app.UseHangfireDashboard("/hangfire", options);//启动hangfire 面板
        }
    }
}
