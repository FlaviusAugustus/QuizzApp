using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QuizApi.Configurations;

namespace QuizApi;

public class QuizContext : DbContext
{
    private readonly IMapper _mapper;
    public DbSet<FlashCardSet> Sets { get; set; }

    public QuizContext(DbContextOptions<QuizContext> options,
                        IMapper mapper) : base(options)
    {
        _mapper = mapper;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new FlashCardConfiguration().Configure(modelBuilder.Entity<FlashCard>());
        new FlashCardSetConfiguration().Configure(modelBuilder.Entity<FlashCardSet>());
        base.OnModelCreating(modelBuilder);
    }

    public async ValueTask<EntityEntry<FlashCardSet>> AddAsync(FlashCardSetDto dto) =>
        await Sets.AddAsync(_mapper.Map<FlashCardSetDto, FlashCardSet>(dto));

}