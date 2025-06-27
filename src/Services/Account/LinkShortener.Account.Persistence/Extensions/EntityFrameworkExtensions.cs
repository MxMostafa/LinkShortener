

namespace LinkShortener.Account.Persistence.Extensions;

public static class EntityFrameworkExtensions
{
    public static void AddSoftDeleteFilter(this ModelBuilder modelBuilder)
    {
        var auditableInterface = typeof(IAuditableEntity<>);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            var implementedInterface = clrType
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == auditableInterface);

            if (implementedInterface != null)
            {
                var parameter = Expression.Parameter(clrType, "x");

                var property = Expression.Property(parameter, "IsDeleted");

                var notDeleted = Expression.Not(property);
                var lambda = Expression.Lambda(notDeleted, parameter);

                entityType.SetQueryFilter(lambda);
            }
        }
    }

}
