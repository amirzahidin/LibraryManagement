using LibraryManagement.API.Data;
using LibraryManagement.API.Domain.Entity;
using LibraryManagement.API.Domain.Exceptions;
using LibraryManagement.API.DTO;
using LibraryManagement.API.Mapping;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Service;

public class MemberService(LibraryDbContext db) : IMemberService
{
    public async Task<IReadOnlyList<MemberResponse>> GetAllAsync(CancellationToken ct)
    {
        var members = await db.Members.AsNoTracking().OrderBy(m => m.FullName).ToListAsync(ct);
        return members.Select(m => m.ToResponse()).ToList();
    }
    public async Task<MemberResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var member = await db.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, ct);
        if(member==null)
        {
            throw new NotFoundException(nameof(Member), id);
        }
        return member.ToResponse();
    }
    public async Task<MemberResponse> CreateAsync(CreateMemberRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Members.AnyAsync(m => m.Email == email, ct))
        {
            throw new DomainException($"A member with email {email} already exists.");
        }
            
        var member = new Member(request.FullName, request.Email, request.MembershipType);
        db.Members.Add(member);
        await db.SaveChangesAsync(ct);
        return member.ToResponse();
    }
    public async Task<MemberResponse> UpdateAsync(int id, UpdateMemberRequest request, CancellationToken ct)
    {
        var member = await db.Members.FirstOrDefaultAsync(m => m.Id == id, ct);
        if(member==null)
        {
            throw new NotFoundException(nameof(Member), id);
        }
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Members.AnyAsync(m => m.Email == email && m.Id != id, ct))
        {
            throw new DomainException($"A member with email {email} already exists.");
        }
            
        member.UpdateDetails(request.FullName, request.Email, request.MembershipType);
        await db.SaveChangesAsync(ct);
        return member.ToResponse();
    }
    public async Task DeactivateAsync(int id, CancellationToken ct)
    {
        var member = await db.Members.FirstOrDefaultAsync(m => m.Id == id, ct);
        if(member==null)
        {
            throw new NotFoundException(nameof(Member), id);
        }
        if (await db.Loans.AnyAsync(l => l.MemberId == id && l.ReturnedAt == null, ct))
        {
            throw new DomainException("Member still has books that have not been returned.");
        }
           
        member.Deactivate();
        await db.SaveChangesAsync(ct);
    }
}