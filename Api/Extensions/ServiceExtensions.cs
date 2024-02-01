using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Application.Logger;
using StackExchange.Redis;
using Api.Middlewares;
using Api.Configurations;
using Persistense;
using FluentValidation.AspNetCore;
using Application.Helpers;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Persistense.AppDBContext;

namespace Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials();
            }));
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }


    public static void ConfigureIisIntegration(this IServiceCollection serviceCollection) =>
        serviceCollection.Configure<IISOptions>(options => { });

    public static void ConfigurePostgresSqlContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<AppDbContext>(
          opts =>
          {
              opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
          });
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var jwtUserSecret = jwtSettings.GetSection("Secret").Value;

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetSection("ValidIssuer").Value,
                ValidAudience = jwtSettings.GetSection("ValidAudience").Value,
                IssuerSigningKey = new
                    SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtUserSecret))
            };
            
        });
    }

    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configurationOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("RedisConnection"), true);
            return ConnectionMultiplexer.Connect(configurationOptions); 
        });
    }

    public static void ConfigureMvc(this IServiceCollection services)
    {
        services.AddMvc()
            .ConfigureApiBehaviorOptions(o =>
            {
                o.InvalidModelStateResponseFactory = context => new ValidationFailedResult(context.ModelState);
            }).AddFluentValidation();
    }

    public static void ConfigureHangFire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
            config.UseSqlServerStorage(configuration.GetConnectionString("HangFireConnection")));
        services.AddHangfireServer();
    }

    public static void ConfigureAWSServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IWebHelper, WebHelper>();
    }


    public static void ConfigureApiVersioning(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersioning(opt =>
        {
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ReportApiVersions = true;
        });
        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddMvcCore().AddApiExplorer();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.OperationFilter<RemoveVersionFromParameter>();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

   
}