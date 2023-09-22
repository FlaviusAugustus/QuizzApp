using Microsoft.EntityFrameworkCore;
using QuizApi.Repository;

namespace QuizApi.Extensions;

public static class DbConfigExtensions
{
    public static IServiceCollection ConfigureDb(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<QuizContext>(options => 
        {
            options.UseSqlite(config.GetConnectionString("database"));
        });

        services.AddScoped<IRepositoryQuiz, QuizRepository>();
        services.AddScoped<IRepositoryUser, UserRepository>();
        return services;
    }
}