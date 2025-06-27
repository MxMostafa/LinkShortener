using LinkShortener.Account.Domain.Entities.Base.Events;
using LinkShortener.Account.Persistence.Contexts;
using MediatR;


namespace LinkShortener.Account.Persistence.Contracts
{
    public class UnitOfWork : BaseUnitOfWork<UserAccountWriteDbContext>
    {
        public UnitOfWork(UserAccountWriteDbContext context, IPublisher publisher) : base(context, publisher)
        {
        }

        protected override Task Publish(IDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
