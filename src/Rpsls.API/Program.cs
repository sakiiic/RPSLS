using Microsoft.EntityFrameworkCore;
using Rpsls.API.Infrastructure.Data;
using FluentValidation;
using Rpsls.API.Services;
using Rpsls.API.Services.Strategy;
using Rpsls.API.Services.RandomGeneratorService;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;  // Reports the supported versions in response headers
    options.AssumeDefaultVersionWhenUnspecified = true;  // Assume default version when none is specified
    options.DefaultApiVersion = new ApiVersion(1, 0);  // Set default version to 1.0
    options.ApiVersionReader = new UrlSegmentApiVersionReader();  // Use URL segment versioning
});

// Add versioned API explorer
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";  // e.g., "v1", "v2"
    options.SubstituteApiVersionInUrl = true;
});

// Set up Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Set the minimum log level for the logger
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Suppress Microsoft logs below Warning level
    .MinimumLevel.Override("System", LogEventLevel.Warning) // Suppress System logs below Warning level
    .WriteTo.File("logs/rpsls.txt", rollingInterval: RollingInterval.Day) // Log to a file
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

// Add Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger generation
builder.Services.AddSwaggerGen(options =>
{
    // Create a temporary service provider to access IApiVersionDescriptionProvider
    var serviceProvider = builder.Services.BuildServiceProvider();
    var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

    // Loop through each version description and create Swagger docs for each
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName, new OpenApiInfo
        {
            Title = $"My API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = $"A sample API for version {description.ApiVersion}",
        });
    }
});

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<UserChoiceValidator>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Register services
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameResultStrategy, ComputerWinStrategy>();
builder.Services.AddScoped<IGameResultStrategy, UserWinStrategy>();
builder.Services.AddScoped<IGameResultStrategy, DrawStrategy>();
builder.Services.AddScoped<IRandomGenerator, DefaultRandomGenerator>();

var app = builder.Build();

// Use middleware for global error handling
app.UseMiddleware<GlobalErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();