using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
namespace QuizApi.Extensions;

public static class SwaggerConfigExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opts =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Description = "Enter only the jwt",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT", 
                
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            
            //opts.SwaggerDoc("V1", new OpenApiInfo { Title = "Quiz Api" , Version = "V1"});
            opts.AddSecurityDefinition("Bearer", jwtSecurityScheme);

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, new List<string>() }
            });
        });
    }
}