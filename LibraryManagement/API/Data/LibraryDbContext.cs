using LibraryManagement.API.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Data;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get {return Set<Book>(); } }
    public DbSet<Member> Members { get {return Set<Member>(); } }
    public DbSet<Loan> Loans { get {return Set<Loan>(); } }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
    }
}