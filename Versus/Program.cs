using Microsoft.EntityFrameworkCore;
using System;
using Versus.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        ConnectToMySql(builder);
        // Add services to the container.
        AddServices(builder);
       
        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<VersusContext>();
            try
            {
                if (db.Database.CanConnect())
                {
                    logger.LogInformation("✅ Conexão com MySQL bem-sucedida!");
                }
                else
                {
                    logger.LogError("❌ Falha na conexão com MySQL!");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Erro ao conectar ao MySQL");
            }
        }

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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
}