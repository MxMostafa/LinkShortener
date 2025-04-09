

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace LinkShortener.Account.Api
{
    public static class ConfigureService
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddApiVersion(services);
            AddCorsService(services);
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddFluentValidationAutoValidation();
            AddSwaggerService(services);
            services.AddSwaggerGen();
           
            services.AddMediatR(x =>
            {
                x.Lifetime = ServiceLifetime.Scoped;
                x.RegisterServicesFromAssemblies(typeof(Program).Assembly);
            });

            services.AddCarter();
            return services;
        }

        public static IApplicationBuilder AddApiApplication(this WebApplication app)
        {

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.AddUiSwaggerService();       // این شامل UseSwagger و UseSwaggerUI هست

            app.MapControllers();
            app.MapCarter();
            app.AddHealthCheckEndpoint();

            return app;
        }

        private static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("api-version"),
                    new MediaTypeApiVersionReader("api-version"));
            });
        }

        private static void AddCorsService(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(p => p
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
            });
        }

        private static void AddSwaggerService(IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Account Server Api",
                    Description = "Account Api v1.0"
                });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.UseAllOfToExtendReferenceSchemas();
                opt.SupportNonNullableReferenceTypes();
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

                opt.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (var file in xmlFiles)
                {
                    opt.IncludeXmlComments(file);
                }
            });
        }

        public static void AddUiSwaggerService(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/Account/{documentname}/swagger.json";
                c.PreSerializeFilters.Clear();
                c.PreSerializeFilters.Add((swagger, httpRequest) =>
                {
                    swagger.Servers = new List<OpenApiServer>();
                });
            });
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/Account/v1.0/swagger.json", "Account Api v1.0");
                config.RoutePrefix = "swagger/Account";
            });
        }

        public static void AddHealthCheckEndpoint(this WebApplication app)
        {
            app.MapGet("/api/healthcheck", (Func<Result>)(() => Result.Success()));
        }
    }
}
