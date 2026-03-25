
using Microsoft.EntityFrameworkCore;
using TestWebApplication.Data;

namespace TestApplictionBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Add services to the container.

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TestWebApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("TestWebApplicationContext") ?? throw new InvalidOperationException("Connection string 'TestWebApplicationContext' not found.")));


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
