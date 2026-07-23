using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Api;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TaskManagementDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<TaskManagementDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TaskManagementTestDb");
                });

                var provider = services.BuildServiceProvider();

                using var scope = provider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            });
        }
    }
}