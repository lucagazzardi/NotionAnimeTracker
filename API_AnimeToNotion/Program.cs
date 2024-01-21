using Azure.Identity;
using Data_AnimeToNotion.Context;
using Microsoft.EntityFrameworkCore;
using Data_AnimeToNotion.Repository;
using System.Text.Json.Serialization;
using JikanDotNet;
using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Integrations.Demo;
using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.MAL_Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAnyOrigin",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

#region DI

builder.Services.AddScoped<IMAL_Integration, MAL_Integration>();
builder.Services.AddScoped<IAnimeShowRepository, AnimeShowRepository>();
builder.Services.AddScoped<IDemo_Integration, Demo_Integration>();
builder.Services.AddScoped<IInternal_Integration, Internal_Integration>();
builder.Services.AddScoped<ISyncToNotionRepository, SyncToNotionRepository>();
builder.Services.AddScoped<IMalAuth, MalAuth>();


builder.Services.AddSingleton<IJikan, Jikan>();

#endregion

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
    new DefaultAzureCredential());
}

builder.Services
    .AddDbContext<AnimeShowContext>(options =>
    {
        options.UseSqlServer(builder.Configuration["DatabaseAnimeConnectionString"]);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
//{
//    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
//    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var app = builder.Build();

DatabaseMigrate(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAnyOrigin");

app.MapControllers();

app.Run();

static void DatabaseMigrate(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AnimeShowContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}