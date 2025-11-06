using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using Versus.Data;
using Versus.Dtos;
using Versus.Helpers;
using Versus.Services;
using Versus.Settings;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        ConnectToMySql(builder);
        AddSecrets(builder);
        AddServices(builder);

        AddJwt(builder);

        var app = builder.Build();

        app.UseCors("AllowLocalhost");

        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
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
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost", policy =>
            {
                policy
                .WithOrigins("https://localhost:4200", "http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        }); 

        builder.Services.AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
            });
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                 );

                var errorResponse = new ResponseDto
                {
                    Success = false,
                    Message = "Um ou mais erros foram encontrados",
                    Data = errors
                };

                return new BadRequestObjectResult( errorResponse );
            };
        });

        builder.Services.AddScoped<UserService>();
        builder.Services.AddSingleton<EmailTemplateService>();
        builder.Services.AddScoped<ResetPasswordService>();
        builder.Services.AddTransient<EmailService>();
        builder.Services.AddTransient<AuthService>();
        builder.Services.AddScoped<TournamentService>();
        builder.Services.AddScoped<PlayerService>();
        builder.Services.AddScoped<TournamentSpecService>();
        builder.Services.AddScoped<MatchService>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Por favor digite o seu 'Bearer {token JWT}'"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    private static void AddSecrets(WebApplicationBuilder builder)
    {
        builder.Configuration.AddUserSecrets<Program>();

        builder.Services.Configure<EmailSettings>(
            builder.Configuration.GetSection("EmailSettings"));

        builder.Services.Configure<TokenSettings>(
            builder.Configuration.GetSection("TokenSettings"));
    }

    private static void AddJwt(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", options =>
        {

            var jwtSettings = builder.Configuration.GetSection("Jwt");

            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"])
                )
            };
        });

        builder.Services.AddAuthorization();
    }
}