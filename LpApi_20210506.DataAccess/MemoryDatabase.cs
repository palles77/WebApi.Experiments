using Microsoft.EntityFrameworkCore;

namespace LpApi_20210506.DataAccess
{
    public class MemoryDatabase : DbContext
    {
        public MemoryDatabase(
            DbContextOptions<MemoryDatabase> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(x => x.MainKey);
            base.OnModelCreating(modelBuilder);
        }
    }
}
