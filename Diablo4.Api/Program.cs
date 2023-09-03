using System.Text.Json;
using Diablo4.Shared.Collections;
using Diablo4.Shared.Json.Converters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;

// Instantiate the main web application builder.
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure services used by the web application.
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new WorldBossJsonConverter());
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Add additional services to the DI container.
builder.Services.AddControllers();
builder.Services.AddSingleton<WorldBossIntervalSequencer>();
builder.Services.AddSingleton<WorldBossSequencer>();

// Configure Swagger and OpenAPI.
// Read more here: https://aka.ms/aspnetcore/swashbuckle.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Contact = new OpenApiContact
        {
            Name = "GitHub",
            Url = new Uri("https://github.com/codeaid/Diablo.NET")
        },
        Description = "Web API exposing information about Diablo 4 helltides, legion events and world bosses",
        Title = "Diablo 4 API",
        Version = "v1"
    });
});

// Build the web application.
WebApplication app = builder.Build();

// Configure the HTTP request pipeline in development environments.
if (app.Environment.IsDevelopment())
{
    // Make Swagger endpoints available to the consumers.
    app.UseSwagger();

    // Enable Swagger UI.
    app.UseSwaggerUI(options =>
    {
        // Remove the "Schemas" section from the UI.
        options.DefaultModelsExpandDepth(-1);
    });

    // Enable detailed exception pages.
    app.UseDeveloperExceptionPage();
}

// Redirect HTTP requests to HTTPS when possible.
app.UseHttpsRedirection();

// Discover and register all available controllers.
app.MapControllers();

await app.RunAsync();
