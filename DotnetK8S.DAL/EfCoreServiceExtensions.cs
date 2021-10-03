using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotnetK8S.DAL
{
    public static class EfCoreServicesExtensions
    {
        public static IServiceCollection AddEfCore(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionPath)
        {
            SqlStorageConfig sqlStorageConfigurationOptions =
                configuration.GetSection(sectionPath).Get<SqlStorageConfig>();
            services.AddDbContextFactory<FibonacciContext>(
                options =>
                    options.UseNpgsql(sqlStorageConfigurationOptions.SqlStorageConnectionString));
            services.AddTransient<IFibResultRepository, FibResultRepository>();
            CreateDbIfNotExists(services);

            return services;
        }

        private static void CreateDbIfNotExists(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            try
            {
                var dbContextFactory = sp.GetRequiredService<IDbContextFactory<FibonacciContext>>();
                using var context = dbContextFactory.CreateDbContext();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var logger = sp.GetRequiredService<ILogger<FibonacciContext>>();
                logger.LogError(ex, "An error occurred creating the DB");
            }
        }
    }
}