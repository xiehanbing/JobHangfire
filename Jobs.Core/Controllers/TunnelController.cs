using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using Jobs.Core.Application;
using Jobs.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TunnelController : ControllerBase
    {
        private readonly ITunnelRegisterService _tunnelRegisterService;
        public TunnelController(ITunnelRegisterService tunnelRegisterService)
        {
            _tunnelRegisterService = tunnelRegisterService;
        }
        /// <summary>
        /// 执行取消过期入廊申请 每1分钟执行一次
        /// </summary>
        [RecurringJob("0 0/1 * * * ?", queue: HangfireQueue.Job)]
        [JobDisplayName("执行取消过期入廊申请 每1分钟执行一次")]
        public void Cancel(PerformContext context)
        {
            context.WriteLine("执行取消过期入廊申请");
            var data =  _tunnelRegisterService.Cancel(context);
            context.WriteLine("结果:" + data);

        }
    }
}