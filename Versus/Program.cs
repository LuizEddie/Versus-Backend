using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using Versus.Data;
using Versus.Services;
using Versus.Settings;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        ConnectToMySql(builder);
        // Add services to the container.
        AddSecrets(builder);
        AddServices(builder);

        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void ConnectToMySql(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("VersusContext") ?? throw new InvalidOperationException("Connection String 'VersusContext' not found.");

        builder.Services.AddDbContext<VersusContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );
    }

    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddSingleton<EmailTemplateService>();
        builder.Services.AddScoped<ResetPasswordService>();
        builder.Services.AddTransient<EmailService>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static void AddSecrets(WebApplicationBuilder builder)
    {
        builder.Configuration.AddUserSecrets<Program>();

        builder.Services.Configure<EmailSettings>(
            builder.Configuration.GetSection("EmailSettings"));

        builder.Services.Configure<TokenSettings>(
            builder.Configuration.GetSection("TokenSettings"));
    }
}