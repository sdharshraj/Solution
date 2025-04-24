using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using API.Middleware;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class Program  // Change this from implicit internal to explicit public
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Configure JWT Authentication
		var jwtSettings = builder.Configuration.GetSection("JwtSettings");

		builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings["Issuer"],
					ValidAudience = jwtSettings["Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
				};
			});

		// Add DbContext to the container
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

		// Add application services and repositories
		builder.Services.AddScoped<IProductService, ProductService>();
		builder.Services.AddScoped<IProductRepository, ProductRepository>();

		// Add controllers to the container
		builder.Services.AddControllers();

		// Add API versioning services
		builder.Services.AddApiVersioning(options =>
		{
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.DefaultApiVersion = new ApiVersion(1, 0); // v1.0 by default
			options.ReportApiVersions = true; // Adds API versions to response headers
			options.ApiVersionReader = ApiVersionReader.Combine(
				new QueryStringApiVersionReader("api-version"),
				new HeaderApiVersionReader("X-Version"),
				new MediaTypeApiVersionReader("ver")
			);
		});

		// Add API versioning explorer
		builder.Services.AddVersionedApiExplorer(options =>
		{
			options.GroupNameFormat = "'v'VVV"; // v1, v1.1 etc.
			options.SubstituteApiVersionInUrl = true; // Add version to the URL
		});

		// Add Swagger services
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(options =>
		{
			var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

			// Add Swagger Docs for all versions
			foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, new OpenApiInfo
				{
					Title = "Product API",
					Version = description.ApiVersion.ToString(),
					Description = "A simple API to manage products",
					Contact = new OpenApiContact
					{
						Name = "Harsh Raj",
						Email = "sdharshraj@gmail.com",
					}
				});
			}

			// Configure JWT Bearer Authentication for Swagger UI
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' followed by your token.",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey
			});
		});

		// Add FluentValidation for DTOs
		builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
		builder.Services.AddFluentValidationAutoValidation(); // Enables automatic model validation
		builder.Services.AddFluentValidationClientsideAdapters();

		// Add Logging
		builder.Services.AddLogging();

		var app = builder.Build();

		app.UseAuthentication();
		app.UseAuthorization();

		// Use custom error handling middleware
		app.UseMiddleware<ErrorHandlingMiddleware>();

		// Use Swagger UI for API Documentation (only in Development environment)
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

				foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
				{
					options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpper());
				}
			});
		}

		// Use Authorization middleware
		app.UseAuthorization();

		// Map the controllers
		app.MapControllers();

		// Run the app
		app.Run();
	}
}
