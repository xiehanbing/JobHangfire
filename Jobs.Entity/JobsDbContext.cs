using Microsoft.EntityFrameworkCore;

namespace Jobs.Entity
{
    /// <summary>
    /// job 数据上下文
    /// </summary>
    public class JobsDbContext : DbContext
    {
        public JobsDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}