using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuizApi.Configurations;

public class FlashCardSetConfiguration : IEntityTypeConfiguration<FlashCardSet>
{
    public void Configure(EntityTypeBuilder<FlashCardSet> builder)
    {
        builder
            .HasMany(set => set.FlashCards)
            .WithOne(flashcard => flashcard.Set)
            .HasForeignKey(flashcard => flashcard.SetId);
        builder
            .HasKey(set => set.Id);
        builder
            .Ignore(set => set.FlashCardCount);
        builder
            .Property(set => set.CreatedAt)
            .IsRequired(); 
    }
}