using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuizApi.Settings;

namespace QuizApi.Extensions;

public static class JWTAuthenticationExtensions
{
    public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opts =>
            {
                opts.RequireHttpsMetadata = false;
                opts.SaveToken = false;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = config["JWTConfig:Issuer"],
                    ValidAudience = config["JWTConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTConfig:Key"]))

                };
            });
        services.Configure<JWT>(config.GetSection(JWT.JWTConfig));
        
        return services;
    }
}