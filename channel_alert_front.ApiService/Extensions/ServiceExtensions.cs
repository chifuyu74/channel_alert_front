using System.Net;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using NLog;

using channel_alert_front.ApiService.Configs;
using channel_alert_front.ApiService.Defines;
using channel_alert_front.ApiService.Docs;
using channel_alert_front.ApiService.Middlewares;
using channel_alert_front.ApiService.Services;
using Microsoft.AspNetCore.Identity;
using channel_alert_front.ApiService.DB;
using Microsoft.EntityFrameworkCore;
using channel_alert_front.ApiService.Repository;
using channel_alert_front.ApiService.Formatter;
using channel_alert_front.ApiService.Entities;

namespace channel_alert_front.ApiService.Extensions;

public static partial class ServiceExtensions
{
    public static void ConfigSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen((option) =>
        {
            option.SwaggerDoc(SwaggerDoc.V1, SwaggerDoc.GetInfo(SwaggerDoc.V1));
            option.SwaggerDoc(SwaggerDoc.V2, SwaggerDoc.GetInfo(SwaggerDoc.V2, "Code Maze"));
            option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, SwaggerDoc.GetSecurityScheme());
            option.AddSecurityRequirement(SwaggerDoc.GetSecurityRequire());
            option.IncludeXmlComments(SwaggerDoc.GetIncludeXmlPath());
        });
    }

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors((options) =>
        {
            options.AddPolicy("default", (policy) =>
            {
                string[] origins = ["http://localhost:5222"];
                policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                //.WithOrigins(origins);
            });
        });
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtConfig jwtConfig = new();
        configuration.Bind(JwtConfig.Section, jwtConfig);

        services.Configure<JwtConfig>(JwtConfig.Section, configuration.GetSection(JwtConfig.Section));

        byte[] keyBytes = Encoding.UTF8.GetBytes(jwtConfig.Key);
        SymmetricSecurityKey signingKey = new(keyBytes);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer((options) =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = signingKey
                };
            });
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization((options) =>
        {
            options.AddPolicy(AuthPolicy.AuthAdmin, (policy) =>
            {
                policy.RequireClaim(RoleDefine.Role, AuthUser.Admin);
            });
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, IdentityRole>((options) =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<RepositoryContext>((options) =>
        {
            string connectionString = builder.Configuration.GetConnectionString(DbConnections.SqlServer)!;
            options.UseSqlServer(connectionString);
        });
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<JwtService>()
            .AddSingleton<TokenBlackListService>()
            .AddScoped<IRepositoryManager, RepositoryManager>()
            .AddScoped<IServiceManager, ServiceManager>();
    }

    public static void AddConfigure(this IServiceCollection services, WebApplicationBuilder builder)
    {
        NLog.LogManager.Setup().LoadConfiguration((builder) =>
        {
            builder.Configuration = new NLog.Config.XmlLoggingConfiguration("nlog.config");
        });

        // Add service defaults & Aspire components.
        builder.AddServiceDefaults();

        services.ConfigureLoggerService();
        services
            .AddControllers((option) =>
            {
                option.InputFormatters.Add(new ByteArrayInputFormatter());
                option.OutputFormatters.Add(new CsvOutputFormatter());
            })
            .AddXmlDataContractSerializerFormatters();

        services.ConfigSwagger();

        services.ConfigureCors();

        services.ConfigureAuthentication(builder.Configuration);
        services.ConfigureAuthorization();
        services.ConfigureIdentity();

        services.AddAutoMapper(typeof(Program));
        services.ConfigureDbContext(builder);

        services.AddRateLimiter((options) =>
        {
            options.RejectionStatusCode = (int)HttpStatusCode.Gone;
        });
        services.AddDistributedMemoryCache();

        services.ConfigureServices();

        // Add services to the container.
        services.AddProblemDetails();
    }

    public static void UseConfigure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger((option) =>
            {
                option.SerializeAsV2 = true;
            });

            app.UseSwaggerUI((option) =>
            {
                option.SwaggerEndpoint($"/swagger/{SwaggerDoc.V1}/swagger.json", SwaggerDoc.V1);
                option.SwaggerEndpoint($"/swagger/{SwaggerDoc.V2}/swagger.json", SwaggerDoc.V2);
                option.RoutePrefix = string.Empty;
                //option.InjectStylesheet("");
            });
        }
        else
        {
            app.UseHsts();
        }

        //app.UseStaticFiles();

        app.UseRouting();
        app.UseRateLimiter();
        app.UseCors("default");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<JwtMiddleware>();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();

        app.MapDefaultEndpoints();
        app.MapControllers();
    }
}
