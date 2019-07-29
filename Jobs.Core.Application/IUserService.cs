using System.Threading.Tasks;
using Jobs.Core.Common;
using Jobs.Entity;

namespace Jobs.Core.Application
{
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> VerifyUser(string account, string password);
    }
}