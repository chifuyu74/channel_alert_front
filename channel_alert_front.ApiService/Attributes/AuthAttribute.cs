using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace channel_alert_front.ApiService.Attributes;

public class AuthAttribute : AuthorizeAttribute
{
    public AuthAttribute()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}
