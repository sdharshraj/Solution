using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class Program  // Change this from implicit internal to explicit public
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddControllers();

        // ✅ Add Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure any additional services you might need (e.g., logging, authentication)
        builder.Services.AddLogging();

        var app = builder.Build();

        // Use Swagger UI for API Documentation
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Add any additional middlewares like Authentication or Authorization
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
