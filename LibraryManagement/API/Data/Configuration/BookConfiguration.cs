using LibraryManagement.API.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.API.Data.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", t => t.HasCheckConstraint("CK_Books_Copies", "CopiesAvailable >= 0 AND CopiesAvailable <= TotalCopies"));
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Author).IsRequired().HasMaxLength(150);
        builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
        builder.HasIndex(b => b.ISBN).IsUnique();
        
        builder.Property(b => b.CopiesAvailable).IsConcurrencyToken();
    }
}

