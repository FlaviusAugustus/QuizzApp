using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QuizApi.Configurations;

namespace QuizApi;

public class QuizContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public DbSet<FlashCardSet> Sets { get; set; }

    public QuizContext(DbContextOptions<QuizContext> options, IConfiguration configuration,
                        IMapper mapper) : base(options)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new FlashCardConfiguration().Configure(modelBuilder.Entity<FlashCard>());
        new FlashCardSetConfiguration().Configure(modelBuilder.Entity<FlashCardSet>());
        base.OnModelCreating(modelBuilder);
    }

    public void Add(FlashCardSetDto dto)
    {
        Sets.Add(_mapper.Map<FlashCardSetDto, FlashCardSet>(dto));
    }
}