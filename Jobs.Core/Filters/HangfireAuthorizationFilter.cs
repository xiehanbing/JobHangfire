using Hangfire.Dashboard;

namespace Jobs.Core.Filters
{
    /// <summary>
    /// hangfire 权限
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {

        //这里需要配置权限规则
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var ac = httpContext.User.Identity;
            if (ac == null)
            {
                string urls = "/Login/Index";
                httpContext.Response.Redirect(urls);
                return false;
            }
            return true;
        }
    }
}