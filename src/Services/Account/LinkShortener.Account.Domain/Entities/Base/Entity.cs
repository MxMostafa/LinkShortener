using LinkShortener.Account.Domain.Entities.Base.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MessagePack;
using Newtonsoft.Json;
using Mapster;

namespace LinkShortener.Account.Domain.Entities.Base;

[MessagePackObject(true)]
public abstract class Entity<T, TIdType> : IEntity<TIdType>, IBaseEntity where T : IEntity<TIdType> where TIdType : IEquatable<TIdType>
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    [JsonInclude]
    [JsonProperty("Identifier")]
    [MessagePack.Key("Identifier")]
    public TIdType Id { get; protected set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }

    [NotMapped]
    [System.Text.Json.Serialization.JsonIgnore]
    [IgnoreMember]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public override bool Equals(object? obj)
    {
        Entity<T, TIdType> entity = obj as Entity<T, TIdType>;
        if ((object)this == entity)
        {
            return true;
        }

        if ((object)entity == null)
        {
            return false;
        }

        return Id.Equals(entity.Id);
    }

    public static bool operator ==(Entity<T, TIdType> a, Entity<T, TIdType> b)
    {
        if ((object)a == null && (object)b == null)
        {
            return true;
        }

        if ((object)a == null || (object)b == null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity<T, TIdType> a, Entity<T, TIdType> b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode() * 907 + Id.GetHashCode();
    }

    internal static T GetFromJsonObject(JsonObject jsonObject)
    {
        return jsonObject.Adapt<T>();
    }
}
