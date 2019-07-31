using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using Jobs.Core.Common;
using Jobs.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Jobs.Core.Application
{
    public class TunnelRegisterService : ITunnelRegisterService
    {
        private readonly byte[] _outCancelList = { 3, 6, 8 };
        private const string AutoCancelComment = "时间过期,自动取消";
        private readonly IGeneralRepository<Jobs.Entity.TunnelRegister> _dbContext;
        public TunnelRegisterService(IGeneralRepository<TunnelRegister> dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// <see cref="ITunnelRegisterService.Cancel(PerformContext)"/>
        /// </summary>
        public bool Cancel(PerformContext context)
        {

            var data = _dbContext.Table.Where(o => o.EndTime < DateTime.Now && !_outCancelList.Contains(o.Status ?? (byte)0)&&(o.Status??0)!=6&&!(o.IsDeleted??false))
                .ToList();
            context.WriteLine(JsonConvert.SerializeObject(data));
            if (data.Count <= 0) return true;
            data.ForEach(o =>
           {
               o.Status = 6;
               o.Comment = AutoCancelComment;
           });
            return _dbContext.DbContext.SaveChanges() > 0;
        }
    }
}