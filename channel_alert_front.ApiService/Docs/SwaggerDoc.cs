using System.Reflection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace channel_alert_front.ApiService.Docs;

public enum ESchemeType
{
    Jwt,
}

public class SwaggerDoc
{
    public const string V1 = "v1";
    public const string V2 = "v2";

    public const string Title = "Channel Alert";
    public const string Description = "ASP.NET Core Web API for managing Alerts";
    public const string TermsOfServiceUri = "https://tcopy.waktaverse.dev";
    
    private static readonly OpenApiSecurityScheme _jwtScheme = new()
    {
        In = ParameterLocation.Header,
        Description = "Place to add JWT with Bearer",
        Name = HeaderNames.Authorization,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    };

    public static OpenApiInfo GetInfo(string version, string title = Title, string description = Description, string termsOfServiceUri = TermsOfServiceUri)
    {
        OpenApiInfo info = new()
        {
            Version = version,
            Title = title,
            Description = description,
            TermsOfService = new Uri(termsOfServiceUri),
            Contact = new OpenApiContact
            {
                Name = title,
                Url = new Uri("https://tcopy.waktaverse.dev")
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://mit-license.org")
            }
        };


        return info;
    }

    public static OpenApiSecurityScheme GetSecurityScheme(ESchemeType type = ESchemeType.Jwt) => type switch
    {
        ESchemeType.Jwt => _jwtScheme,
        _ => _jwtScheme,
    };

    public static OpenApiSecurityRequirement GetSecurityRequire()
    {
        OpenApiSecurityRequirement jwtRequire = [];
        jwtRequire.Add(_jwtScheme, []);

        return jwtRequire;
    }

    public static string GetIncludeXmlPath()
    {
        string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        return xmlPath;
    }
}
