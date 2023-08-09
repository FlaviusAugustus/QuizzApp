using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QuizApi.Configurations;

namespace QuizApi;

public class QuizContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public DbSet<Quiz> Quizzes { get; set; }

    public QuizContext(DbContextOptions<QuizContext> options, IConfiguration configuration,
                        IMapper mapper) : base(options)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new QuestionConfiguration().Configure(modelBuilder.Entity<Question>());
        new QuizConfiguration().Configure(modelBuilder.Entity<Quiz>());
        base.OnModelCreating(modelBuilder);
    }

    public void Add(QuizDto dto)
    {
        Quizzes.Add(_mapper.Map<QuizDto, Quiz>(dto));
    }
}