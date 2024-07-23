using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Project.API.Utils;
using System.Security.Claims;
using System.Text;

namespace Project.API.Authentication
{
    public class BearerAuthenticationOptions(JwtSettings jwtSettings)
    {
        public void ConfigureJwtAuthentictionOptions(JwtBearerOptions options)
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context => 
                {
                    var claims = context.Principal?.Identity as ClaimsIdentity;
                    if (claims != null)
                    {
                        var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        context.HttpContext.Items["userId"] = userId;
                    }
                    return Task.CompletedTask;
                }
            };
        }
    }
}
