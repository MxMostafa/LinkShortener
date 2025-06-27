

using LinkShortener.Account.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Account.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.Id)
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(_ => _.PhoneNumber)
               .HasMaxLength(15)
               .IsRequired();


    }
}
