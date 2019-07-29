namespace Jobs.Core.Common
{
    public class HangfireJobOptions
    {
        /// <summary>
        /// 队列序号
        /// </summary>
        public static object[] Queues ={"default", "apis", "job"};
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "hangfire";
        /// <summary>
        /// 并发任务数
        /// </summary>
        public static int WorkCount = 40;
    }
    /// <summary>
    /// 队列列表
    /// </summary>
    public class HangfireQueue
    {
        /// <summary>
        /// default
        /// </summary>
        public const string Default = "default";
        /// <summary>
        /// apis
        /// </summary>
        public const string Apis = "apis";
        /// <summary>
        /// job
        /// </summary>
        public const string Job = "job";
    }


    
}