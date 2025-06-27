

namespace LinkShortener.Account.Persistence.Extensions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ShadowAttribute : Attribute
{
    public Type EntityType { get; }

    public ShadowAttribute(Type entityType)
    {
        EntityType = entityType;
    }
}
