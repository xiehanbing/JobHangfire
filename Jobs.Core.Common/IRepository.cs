using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jobs.Core.Common
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// db 上下文
        /// </summary>
        DbContext DbContext { get; }
        /// <summary>
        /// dbset 实体
        /// </summary>
        DbSet<TEntity> Entities { get; }

        IQueryable<TEntity> Table { get; }

        TEntity GetById(object id);

        object Insert(TEntity entity, bool isSave = true);

        int Update(TEntity entity, bool isSave = true);

        int Delete(TEntity entity, bool isSave = true);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}