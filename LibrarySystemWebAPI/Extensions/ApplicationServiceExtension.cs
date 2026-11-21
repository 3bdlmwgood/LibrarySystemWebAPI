
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using LibrarySystemWebAPI.Repositories;
using LibrarySystemWebAPI.Services;
using LibrarySystemWebAPI.Data.Interceptors;
using LibrarySystemWebAPI.Middlewares;


namespace LibrarySystemWebAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddOpenApi();

            // Database Connection
            services.AddDbContext<ApplicationDBContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultSqlConnection"));

                options.AddInterceptors(
                    serviceProvider.GetRequiredService<CreatedAtInterceptor>(),
                    serviceProvider.GetRequiredService<ModifiedAtInterceptor>(),
                    serviceProvider.GetRequiredService<SoftDeleteInterceptor>()
                );
            });

            // AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AuthorProfile>();
                cfg.AddProfile<BookProfile>();
                cfg.AddProfile<LoanProfile>();
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorService, AuthorService>();

            services.AddScoped<CreatedAtInterceptor>();
            services.AddScoped<ModifiedAtInterceptor>();
            services.AddScoped<SoftDeleteInterceptor>();

            services.AddTransient<RequestIdHeaderMiddleware>();

            return services;
        }
    }
}
