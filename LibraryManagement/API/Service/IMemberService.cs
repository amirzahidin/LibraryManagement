using LibraryManagement.API.DTO;

namespace LibraryManagement.API.Service;

public interface IMemberService
{
    Task<IReadOnlyList<MemberResponse>> GetAllAsync(CancellationToken ct);
    Task<MemberResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<MemberResponse> CreateAsync(CreateMemberRequest request, CancellationToken ct);
    Task<MemberResponse> UpdateAsync(int id, UpdateMemberRequest request, CancellationToken ct);
    Task DeactivateAsync(int id, CancellationToken ct);
}