using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QuizApi.Configurations;
using QuizApi.Repository;

namespace QuizApi;

public class QuizContext : IdentityDbContext<User,IdentityRole<Guid>, Guid>
{
    private DbSet<FlashCardSet> Sets { get; set; }

    public QuizContext(DbContextOptions<QuizContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlashCardConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}