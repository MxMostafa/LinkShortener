using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortener.Account.Application.Abstractions.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDbContextTransaction? Transaction { get; }

    bool HasActiveTransaction { get; }

    Task CommitAsync(CancellationToken cancellationToken);

    Task CommitWithTriggerAsync(CancellationToken cancellationToken, bool checkReferencesDeleted = true);

    void BeginTransaction();

    Task BeginTransactionAsync(CancellationToken cancellationToken);

    void CommitTransaction();

    Task CommitTransactionAsync(CancellationToken cancellationToken);

    void RollbackTransaction();

    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}
