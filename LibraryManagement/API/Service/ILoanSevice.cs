using LibraryManagement.API.Domain.Enums;
using LibraryManagement.API.DTO;

namespace LibraryManagement.API.Service;

public interface ILoanService
{
    Task<IReadOnlyList<LoanResponse>> GetLoansAsync(int? memberId, LoanStatus? status, CancellationToken ct);
    Task<LoanResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<LoanResponse> BorrowAsync(BorrowBookRequest request, CancellationToken ct);
    Task<LoanResponse> ReturnAsync(int loanId, CancellationToken ct);
}