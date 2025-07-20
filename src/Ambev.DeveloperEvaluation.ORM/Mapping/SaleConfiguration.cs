using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    /// <summary>
    /// Configuration for the Sale entity.
    /// </summary>
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SaleNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SaleDate)
                .IsRequired();

            builder.Property(s => s.CustomerId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.BranchId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.BranchName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(s => s.IsCancelled)
                .IsRequired();

            builder.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey("SaleId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
