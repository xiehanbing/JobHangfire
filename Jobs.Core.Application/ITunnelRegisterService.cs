using System.Threading.Tasks;
using Hangfire.Server;

namespace Jobs.Core.Application
{
    public interface ITunnelRegisterService
    {
        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        bool Cancel(PerformContext context);
    }
}