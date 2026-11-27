
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Data.Interceptors;
using LibrarySystemWebAPI.Entities;
using LibrarySystemWebAPI.Filters;
using LibrarySystemWebAPI.Helpers;
using LibrarySystemWebAPI.Middlewares;
using LibrarySystemWebAPI.Repositories;
using LibrarySystemWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            // Filters
            services.AddScoped<ActionExecutionLoggingFilter>();
            services.AddScoped<ActionTimingFilter>();
            services.AddScoped<ExceptionHandlingFilter>();


            // Authentication and Authorization
            services.AddDbContext<AuthenticationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("AuthenticationConnection")
    )
);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                    )
                };
            });

            services.AddScoped<IJwtService, JwtService>();


            // CORS Configuration
            var corsOrigins =
    configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? Array.Empty<string>();

            services.AddCors(options =>
            {
                options.AddPolicy("ConfigurationOrigins", policy =>
                {
                    policy.WithOrigins(corsOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
