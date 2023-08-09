using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuizApi.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder
            .HasOne(question => question.Quiz)
            .WithMany(quiz => quiz.Questions)
            .HasForeignKey(question => question.QuizId);
            
        builder
            .HasKey(question => question.Id);
        builder
            .Property(question => question.Content)
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(question => question.CorrectAnwser)
            .IsRequired();
    }
}