using Microsoft.EntityFrameworkCore;

namespace Jobs.Entity
{
    public class GeneralDbContext: DbContext
    {
        public GeneralDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<TunnelRegister> TunnelRegisters { get; set; }
    }
}