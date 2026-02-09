using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManager.Domain.Entities;

namespace TestManager.Infrastructure.Configurations;

public class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
{
    public void Configure(EntityTypeBuilder<TestCase> builder)
    {
        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(tc => tc.Steps)
               .IsRequired();

        builder.Property(tc => tc.ExpectedResult)
               .IsRequired();
    }
}
