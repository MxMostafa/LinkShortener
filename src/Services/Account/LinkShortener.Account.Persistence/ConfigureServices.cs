using LinkShortener.Account.Application.Abstractions.Interfaces;
using LinkShortener.Account.Application.Abstractions.Repositories.Users;
using LinkShortener.Account.Application.Services.Users;
using LinkShortener.Account.Persistence.Contexts;
using LinkShortener.Account.Persistence.Contracts;
using LinkShortener.Account.Persistence.Repositories.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortener.Account.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserAccountReadDbContext>((provider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("UserAccountReadDbContext"),
                builder => builder
                    .MigrationsAssembly(typeof(UserAccountReadDbContext).Assembly.FullName));

            //options.AddInterceptors(
            //    provider.GetRequiredService<AuditableEntityInterceptor>(),
            //    provider.GetRequiredService<ClearEntityCacheInterceptor>(),
            //    provider.GetRequiredService<DbAuditLogInterceptor>());
        });


        services.AddDbContext<UserAccountWriteDbContext>((provider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("UserAccountWriteDbContext"),
                builder => builder
                    .MigrationsAssembly(typeof(UserAccountWriteDbContext).Assembly.FullName));

            //options.AddInterceptors(
            //    provider.GetRequiredService<AuditableEntityInterceptor>(),
            //    provider.GetRequiredService<ClearEntityCacheInterceptor>(),
            //    provider.GetRequiredService<DbAuditLogInterceptor>());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        return services;
    }
}
