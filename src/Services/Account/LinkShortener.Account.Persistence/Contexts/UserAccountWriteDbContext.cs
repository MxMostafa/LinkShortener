
using System.Reflection;
namespace LinkShortener.Account.Persistence.Contexts
{
    public class UserAccountWriteDbContext : DbContext
    {
        public UserAccountWriteDbContext(DbContextOptions<UserAccountWriteDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("UserAccount");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }

    }
}