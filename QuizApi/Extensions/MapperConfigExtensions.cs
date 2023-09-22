using AutoMapper;

namespace QuizApi.Extensions;

public static class MapperConfigExtensions
{
    public static IServiceCollection ConfigureMapper(this IServiceCollection services)
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.CreateMap<FlashCardDto, FlashCard>();
            config.CreateMap<FlashCardSetDto, FlashCardSet>();
        });

        var mapper = configuration.CreateMapper();
        
        services.AddSingleton(mapper);
        return services;
    }
}