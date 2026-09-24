using LibraryManagement.API.Data;
using LibraryManagement.API.Domain.Entity;
using LibraryManagement.API.Domain.Exceptions;
using LibraryManagement.API.DTO;
using LibraryManagement.API.Mapping;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Service;

public class BookService(LibraryDbContext db) : IBookService
{
    public async Task<PagedResult<BookResponse>> GetAllAsync(string? search, int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var query = db.Books.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(b =>
                            EF.Functions.Like(b.Title, pattern) ||
                            EF.Functions.Like(b.Author, pattern) ||
                            EF.Functions.Like(b.ISBN, pattern));
        }
        var total = await query.CountAsync(ct);
        var books = await query
                        .OrderBy(b => b.Title)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(ct);
        return new PagedResult<BookResponse>(books.Select(b => b.ToResponse()).ToList(), page, pageSize, total);
    }
    public async Task<BookResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var book = await db.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, ct);

        if (book == null)
        {
            throw new NotFoundException(nameof(Book), id);
        }
        return book.ToResponse();
    }
    public async Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken ct)
    {
        if (await db.Books.AnyAsync(b => b.ISBN == request.ISBN.Trim(), ct))
        {
            throw new DomainException($"A book with ISBN {request.ISBN} already exists.");
        }
            
        var book = new Book(request.Title, request.Author, request.ISBN,request.PublishedYear, request.TotalCopies);
        db.Books.Add(book);
        await db.SaveChangesAsync(ct);
        return book.ToResponse();
    }
    public async Task<BookResponse> UpdateAsync(int id, UpdateBookRequest request, CancellationToken ct)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (book == null)
        {
            throw new NotFoundException(nameof(Book), id);
        }
                                                   
        if (await db.Books.AnyAsync(b => b.ISBN == request.ISBN.Trim() && b.Id != id, ct))
        {
            throw new DomainException($"A book with ISBN {request.ISBN} already exists.");
        }
           
        book.UpdateDetails(request.Title, request.Author, request.ISBN, request.PublishedYear);
        book.UpdateTotalCopies(request.TotalCopies);
        await db.SaveChangesAsync(ct);
        return book.ToResponse();
    }
    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (book == null)
        {
            throw new NotFoundException(nameof(Book), id);
        }
                                                   
        if (await db.Loans.AnyAsync(l => l.BookId == id, ct))
        {
            throw new DomainException("Cannot delete a book that has loan history.");
        }

        db.Books.Remove(book);
        await db.SaveChangesAsync(ct);
    }
}