
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Extensions;
using LibrarySystemWebAPI.Helpers;
using LibrarySystemWebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;


namespace LibrarySystemWebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register Services
            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();

            // Automatic Migration + Seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var dbContext =
                        services.GetRequiredService<ApplicationDBContext>();

                    await dbContext.Database.MigrateAsync();

                    await StoreContextSeeding.SeedAsync(
                        dbContext,
                        app.Environment.ContentRootPath
                    );
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "An error occurred while migrating or seeding the database."
                    );
                }
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseMiddleware<RequestTimingMiddleware>();

            app.UseMiddleware<RequestIdHeaderMiddleware>();

            app.UseMiddleware<RateLimitingMiddleware>();

            // Configure Middleware
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("ConfigurationOrigins");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
