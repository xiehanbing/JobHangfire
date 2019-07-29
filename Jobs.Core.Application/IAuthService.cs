namespace Jobs.Core.Application
{
    public interface IAuthService
    {
        /// <summary>
        /// 保存登录状态
        /// </summary>
        /// <param name="name">账号</param>
        void SignIn( string name);
    }
}