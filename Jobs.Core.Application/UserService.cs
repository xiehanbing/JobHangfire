using System.Linq;
using System.Threading.Tasks;
using Jobs.Core.Common;
using Jobs.Entity;

namespace Jobs.Core.Application
{
    public class UserService:IUserService
    {
        private readonly IRepository<Jobs.Entity.User> _repository;
        public UserService(IRepository<Jobs.Entity.User> repository)
        {
            _repository = repository;
        }
        public async Task<User> VerifyUser(string account, string password)
        {
           var data=await _repository.FirstOrDefaultAsync(o =>
                o.Account.Equals(account) && o.Password.Equals(password) && !o.IsDeleted);
            return data;
        }
    }
}