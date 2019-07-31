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
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        /// <summary>
        /// 每天早上6点执行一次
        /// </summary>
        [RecurringJob("0 0 6 * * ?",queue:HangfireQueue.Job)]
        [JobDisplayName("发送 每天早上6点执行一次")]
        public void Send()
        {
            _messageService.Send("hello message");
           
        }
        /// <summary>
        /// 每两个小时执行一次 （每19分钟）
        /// </summary>
        [RecurringJob("0 0 0/2 * * ?", queue: HangfireQueue.Default)]
        [JobDisplayName("接收 每两个小时执行一次")]
        public void Receive(PerformContext context)
        {
            context.WriteLine("Receive：hello message");
            _messageService.Receive("hello message");
        }
    }
}