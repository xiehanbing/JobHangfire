using Hangfire;
using Jobs.Core.Controllers;

namespace Jobs.Core.Extension
{
    /// <summary>
    /// 注入 方法
    /// </summary>
    public class HandfireExtension
    {
        //public static void Register()
        //{
        //    RecurringJob.AddOrUpdate<MessageController>(a => a.Send(), Cron.Minutely());
        //    RecurringJob.AddOrUpdate<MessageController>(a => a.Receive(), Cron.Minutely());
        //}
    }
}