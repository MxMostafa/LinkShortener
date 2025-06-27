using LinkShortener.Account.Application.Abstractions.Interfaces;
using LinkShortener.Account.Application.Common.Errors;
using LinkShortener.Account.Application.Common.Results;
using LinkShortener.Account.Domain.Entities.Base;
using LinkShortener.Account.Domain.Entities.Base.Events;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortener.Account.Persistence.Contracts;

public abstract class BaseUnitOfWork<T> : IUnitOfWork, IDisposable where T : DbContext
{
    protected readonly T DbContext;

    protected readonly IPublisher Publisher;

    protected IDbContextTransaction? _transaction;

    public IDbContextTransaction? Transaction => _transaction;

    public bool HasActiveTransaction => _transaction != null;

    protected BaseUnitOfWork(T context, IPublisher publisher)
    {
        DbContext = context;
        Publisher = publisher;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    ~BaseUnitOfWork()
    {
        Dispose();
    }

    public void BeginTransaction()
    {
        if (_transaction == null)
        {
            _transaction = DbContext.Database.BeginTransaction();
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
        {
            _transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }

    public void RollbackTransaction()
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }

        _transaction.Rollback();
        DisposeTransaction();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }

        await _transaction.RollbackAsync(cancellationToken);
        await DisposeTransactionAsync();
    }

    public void CommitTransaction()
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }

        try
        {
            _transaction.Commit();
        }
        catch (Exception)
        {
            if (_transaction != null)
            {
                RollbackTransaction();
            }

            throw;
        }
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }

        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            if (_transaction != null)
            {
                await RollbackTransactionAsync(cancellationToken);
            }

            throw;
        }
    }

    private void DisposeTransaction()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        DateTime now = DateTime.Now;
        UpdateAuditableEntities(now);
        PublishDomainEvent();
        return DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CommitWithTriggerAsync(CancellationToken cancellationToken, bool checkReferencesDeleted = true)
    {
        DateTime now = DateTime.Now;
        UpdateAuditableEntities(now);
        PublishDomainEvent();
        AddHistoryData(now);
        if (checkReferencesDeleted)
        {
            Result result = await CheckReferencesForDeletedEntitiesAsync(cancellationToken);
            if (result.IsFailure)
            {
                throw new ArgumentException(result.Error?.Message ?? CommonErrors.UnknownError.Message);
            }
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    private void AddHistoryData(DateTime currentDate)
    {
        IEnumerable<EntityEntry> source = DbContext.ChangeTracker.Entries();
        if (!source.Any())
        {
            return;
        }

        foreach (EntityEntry item in (from e in source
                                      where e.State != EntityState.Unchanged && e.State != EntityState.Detached
                                      where (object)e.Entity.GetType().GetCustomAttribute<ShadowAttribute>()?.EntityType != null
                                      select e).ToList())
        {
            if ((object)item.Entity.GetType().GetCustomAttribute<ShadowAttribute>()?.EntityType == null)
            {
                continue;
            }

            object obj = Activator.CreateInstance(item.Entity.GetType().GetCustomAttribute<ShadowAttribute>().EntityType);
            foreach (PropertyEntry property2 in item.Properties)
            {
                PropertyInfo property = obj.GetType().GetProperty(property2.Metadata.Name);
                if ((object)property != null && property.CanWrite)
                {
                    property.SetValue(obj, property2.CurrentValue);
                }
            }

            if ((object)obj.GetType().GetProperty("InsertedAt") != null && obj.GetType().GetProperty("InsertedAt").CanWrite)
            {
                obj.GetType().GetProperty("InsertedAt")?.SetValue(obj, currentDate);
            }

            DbContext.Add(obj);
        }
    }

    private void UpdateAuditableEntities(DateTime now)
    {
        foreach (var entry in DbContext.ChangeTracker.Entries())
        {
            var entityType = entry.Entity.GetType();
            var interfaceType = entityType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAuditableEntity<>));

            if (interfaceType is null) continue;

            if (entry.State == EntityState.Added)
            {
                entry.Property("Created").CurrentValue = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("Updated").CurrentValue = now;
            }
        }
    }

    private void PublishDomainEvent()
    {
        List<EntityEntry<IBaseEntity>> list = (from entryEntity in DbContext.ChangeTracker.Entries<IBaseEntity>()
                                               where entryEntity.Entity.DomainEvents.Any()
                                               select entryEntity).ToList();
        List<IDomainEvent> list2 = list.SelectMany((EntityEntry<IBaseEntity> _) => _.Entity.DomainEvents).ToList();
        list.ForEach(delegate (EntityEntry<IBaseEntity> entity)
        {
            entity.Entity.ClearDomainEvents();
        });
        foreach (IDomainEvent item in list2)
        {
            Publish(item);
        }
    }

    private async Task<Result> CheckReferencesForDeletedEntitiesAsync(CancellationToken cancellationToken)
    {
        foreach (EntityEntry<IAuditableEntity<long>> item in DbContext.ChangeTracker.Entries<IAuditableEntity<long>>())
        {
            if (item.Entity.IsDeleted && item.State == EntityState.Modified)
            {
                IAuditableEntity<long> entity = item.Entity;
                Result result = await IsEntityReferencedAsync(entity, cancellationToken);
                if (result.IsFailure)
                {
                    return Result.Failure(result.Error ?? CommonErrors.UnknownError);
                }
            }
        }

        return Result.Success();
    }

    private async Task<Result> IsEntityReferencedAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
    {
        Type entityType = entity.GetType();
        PropertyInfo property = entityType.GetProperty("Id");
        if (property == null)
        {
            return Result.Failure(CommonErrors.ItemNotFound);
        }

        object entityId = property.GetValue(entity);
        IEnumerable<IEntityType> entityTypes = DbContext.Model.GetEntityTypes();
        foreach (IEntityType item in entityTypes)
        {
            Type entityClrType = item.ClrType;
            List<IForeignKey> list = (from fk in item.GetForeignKeys()
                                      where fk.PrincipalEntityType.ClrType == entityType
                                      select fk).ToList();
            foreach (IForeignKey item2 in list)
            {
                if (GetDbSetMethod(entityClrType).Invoke(DbContext, null) is IQueryable queryable)
                {
                    IProperty property2 = item2.Properties.First();
                    string name = property2.Name;
                    ParameterExpression parameterExpression = Expression.Parameter(entityClrType, "e");
                    MemberExpression expression = Expression.Property(parameterExpression, name);
                    Type clrType = property2.ClrType;
                    ConstantExpression expression2 = Expression.Constant(entityId, clrType);
                    UnaryExpression left = Expression.Convert(expression, typeof(long?));
                    UnaryExpression right = Expression.Convert(expression2, typeof(long?));
                    LambdaExpression arg = Expression.Lambda(Expression.Equal(left, right), parameterExpression);
                    MethodCallExpression expression3 = Expression.Call(typeof(Queryable).GetMethods().First((MethodInfo m) => m.Name == "Any" && m.GetParameters().Length == 2).MakeGenericMethod(entityClrType), queryable.Expression, arg);
                    if (await Task.FromResult(queryable.Provider.Execute<bool>(expression3)))
                    {
                        return Result.Failure(CommonErrors.ItemIsUsed);
                    }
                }
            }
        }

        return Result.Success();
    }

    private Result CheckReferencesForDeletedEntities()
    {
        foreach (EntityEntry<IAuditableEntity<long>> item in DbContext.ChangeTracker.Entries<IAuditableEntity<long>>())
        {
            if (item.Entity.IsDeleted && item.State == EntityState.Modified)
            {
                IAuditableEntity<long> entity = item.Entity;
                Result result = IsEntityReferenced(entity);
                if (result.IsFailure)
                {
                    return Result.Failure(result.Error ?? CommonErrors.UnknownError);
                }
            }
        }

        return Result.Success();
    }

    private Result IsEntityReferenced<TEntity>(TEntity entity) where TEntity : class
    {
        Type entityType = entity.GetType();
        PropertyInfo property = entityType.GetProperty("Id");
        if (property == null)
        {
            return Result.Failure(CommonErrors.ItemNotFound);
        }

        object value = property.GetValue(entity);
        foreach (IEntityType entityType2 in DbContext.Model.GetEntityTypes())
        {
            Type clrType = entityType2.ClrType;
            foreach (IForeignKey item in (from fk in entityType2.GetForeignKeys()
                                          where fk.PrincipalEntityType.ClrType == entityType
                                          select fk).ToList())
            {
                if (GetDbSetMethod(clrType).Invoke(DbContext, null) is IQueryable queryable)
                {
                    IProperty property2 = item.Properties.First();
                    string name = property2.Name;
                    ParameterExpression parameterExpression = Expression.Parameter(clrType, "e");
                    MemberExpression expression = Expression.Property(parameterExpression, name);
                    Type clrType2 = property2.ClrType;
                    ConstantExpression expression2 = Expression.Constant(value, clrType2);
                    UnaryExpression left = Expression.Convert(expression, typeof(long?));
                    UnaryExpression right = Expression.Convert(expression2, typeof(long?));
                    LambdaExpression arg = Expression.Lambda(Expression.Equal(left, right), parameterExpression);
                    MethodCallExpression expression3 = Expression.Call(typeof(Queryable).GetMethods().First((MethodInfo m) => m.Name == "Any" && m.GetParameters().Length == 2).MakeGenericMethod(clrType), queryable.Expression, arg);
                    if (queryable.Provider.Execute<bool>(expression3))
                    {
                        return Result.Failure(CommonErrors.ItemIsUsed);
                    }
                }
            }
        }

        return Result.Success();
    }

    private MethodInfo GetDbSetMethod(Type entityType)
    {
        return typeof(DbContext).GetMethods().First((MethodInfo m) => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0).MakeGenericMethod(entityType);
    }

    protected abstract Task Publish(IDomainEvent domainEvent);
}
