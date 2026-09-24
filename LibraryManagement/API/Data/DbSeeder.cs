using LibraryManagement.API.Domain.Entity;
using LibraryManagement.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(LibraryDbContext db)
    {
        if (await db.Books.AnyAsync()) return;
        db.Books.AddRange(
                        new Book("Clean Code", "Robert C. Martin", "9780132350884", 2008, 3),
                        new Book("The Pragmatic Programmer", "Andrew Hunt & David Thomas", "9780135957059", 2019, 2),
                        new Book("Designing Data-Intensive Applications", "Martin Kleppmann", "9781449373320", 2017, 1));
        db.Members.AddRange(
                        new Member("Ali Ahmad", "ali@example.com", MembershipType.Standard),
                        new Member("Siti Aminah", "siti@example.com", MembershipType.Premium));
        await db.SaveChangesAsync();
    }
}