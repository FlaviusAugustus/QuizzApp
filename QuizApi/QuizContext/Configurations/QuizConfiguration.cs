using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuizApi.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder
            .HasMany(quiz => quiz.Questions)
            .WithOne(question => question.Quiz)
            .HasForeignKey(question => question.QuizId);
        builder
            .HasKey(quiz => quiz.Id);
        builder
            .Ignore(quiz => quiz.QuestionCount);
    }
}