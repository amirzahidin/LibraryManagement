using LibraryManagement.API.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.API.Data.Configuration;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.FineAmount).HasPrecision(10, 2);
        builder.HasOne(l => l.Book)
                        .WithMany()
                        .HasForeignKey(l => l.BookId)
                        .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(l => l.Member)
                        .WithMany()
                        .HasForeignKey(l => l.MemberId)
                        .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(l => new { l.MemberId, l.ReturnedAt });
    }
}