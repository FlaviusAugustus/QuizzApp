using QuizApi.Exceptions.Filters;

namespace QuizApi.Extensions;

public static class ControllerConfigExtensions
{
    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(config =>
        {
            config.Filters.Add<ParsingExceptionFilter>();
            
        });
        return services;
    }
}