using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManager.Domain.Entities;

namespace TestManager.Infrastructure.Configurations;

public class TestSuiteConfiguration : IEntityTypeConfiguration<TestSuite>
{
    public void Configure(EntityTypeBuilder<TestSuite> builder)
    {
        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasMany(ts => ts.TestCases)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
