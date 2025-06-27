

using LinkShortener.Account.Domain.Entities.Base;
using System.Reflection;

namespace LinkShortener.Account.Persistence.Contexts;

public class UserAccountReadDbContext:DbContext
{
    public UserAccountReadDbContext(DbContextOptions<UserAccountReadDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("UserAccount");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.AddSoftDeleteFilter();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

}
