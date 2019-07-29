using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.RecurringJobExtensions;
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
        public void Send()
        {
            _messageService.Send("hello message");
           
        }
        /// <summary>
        /// 每两个小时执行一次
        /// </summary>
        [RecurringJob("0 0 */2 * * ?", queue: HangfireQueue.Default)]
        public void Receive()
        {
            _messageService.Receive("hello message");
        }
    }
}