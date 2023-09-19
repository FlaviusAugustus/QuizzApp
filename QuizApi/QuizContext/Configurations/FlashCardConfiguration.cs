using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuizApi.Configurations;

public class FlashCardConfiguration : IEntityTypeConfiguration<FlashCard>
{
    public void Configure(EntityTypeBuilder<FlashCard> builder)
    {
        builder
            .HasOne(flashcard => flashcard.Set)
            .WithMany(set => set.FlashCards)
            .HasForeignKey(flashcard => flashcard.SetId);
            
        builder
            .HasKey(flashcard => flashcard.Id);
        builder
            .Property(flashcard => flashcard.Question)
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(flashcard => flashcard.Answer)
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(flashcard => flashcard.CreatedAt)
            .IsRequired();
    }
}