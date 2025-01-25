using BookStoreRest.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;

namespace BookStoreRest
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

            // Добавление контекста базы данных с PostgreSQL
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(connectionString));
            var app = builder.Build();

            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}
