namespace LibraryManagement.API.DTO;

public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);