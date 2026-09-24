using LibraryManagement.API.DTO;

namespace LibraryManagement.API.Service;

public interface IBookService
{
    Task<PagedResult<BookResponse>> GetAllAsync(string? search, int page, int pageSize, CancellationToken ct);
    Task<BookResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken ct);
    Task<BookResponse> UpdateAsync(int id, UpdateBookRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}