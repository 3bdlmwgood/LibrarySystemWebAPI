
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Helpers;
using Microsoft.EntityFrameworkCore;

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
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultSqlConnection")
                )
            );

            // AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AuthorProfile>();
                cfg.AddProfile<BookProfile>();
                cfg.AddProfile<LoanProfile>();
            });

            return services;
        }
    }
}
