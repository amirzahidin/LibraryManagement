using LibraryManagement.API.Data;
using LibraryManagement.API.Domain.Entity;
using LibraryManagement.API.Domain.Enums;
using LibraryManagement.API.Domain.Exceptions;
using LibraryManagement.API.Domain.Policy;
using LibraryManagement.API.DTO;
using LibraryManagement.API.Mapping;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Service;

public class LoanService(LibraryDbContext db, TimeProvider timeProvider) : ILoanService
{
    private DateTime Now { get { return timeProvider.GetLocalNow().LocalDateTime;}}
    public async Task<IReadOnlyList<LoanResponse>> GetLoansAsync(int? memberId, LoanStatus? status, CancellationToken ct)
    {
        var now = Now;
        var query = db.Loans.AsNoTracking()
                            .Include(l => l.Book)
                            .Include(l => l.Member)
                            .AsQueryable();
        if (memberId.HasValue)
        {
            query = query.Where(l => l.MemberId == memberId.Value);
        }

        if (status == LoanStatus.Returned)
        {
            query = query.Where(l => l.ReturnedAt != null);
        }
        else if (status == LoanStatus.Overdue)
        {
            query = query.Where(l => l.ReturnedAt == null && l.DueDate < now);
        }
        else if (status == LoanStatus.Active)
        {
            query = query.Where(l => l.ReturnedAt == null && l.DueDate >= now);
        }
        var loans = await query.OrderByDescending(l => l.BorrowedAt).ToListAsync(ct);
        return loans.Select(l => l.ToResponse(now)).ToList();
    }
    public async Task<LoanResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var loan = await db.Loans.AsNoTracking()
                                 .Include(l => l.Book)
                                 .Include(l => l.Member)
                                 .FirstOrDefaultAsync(l => l.Id == id, ct);
        if(loan == null)
        {
            throw new NotFoundException(nameof(Loan), id);
        }
        return loan.ToResponse(Now);
    }
    public async Task<LoanResponse> BorrowAsync(BorrowBookRequest request, CancellationToken ct)
    {
        var now = Now;
        var member = await db.Members.FirstOrDefaultAsync(m => m.Id == request.MemberId, ct);
        if(member == null)
        {
            throw new NotFoundException(nameof(Member), request.MemberId);
        }
        if (!member.IsActive)
        {
            throw new DomainException("Inactive members cannot borrow books.");
        }

        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == request.BookId, ct);
        if(book == null)
        {
            throw new NotFoundException(nameof(Book), request.BookId);
        }
        var policy = BorrowingPolicy.For(member.MembershipType);
        var activeLoans = await db.Loans
                        .Where(l => l.MemberId == member.Id && l.ReturnedAt == null)
                        .ToListAsync(ct);
        if (activeLoans.Any(l => l.IsOverdue(now)))
        {
            throw new DomainException("Member has overdue books and cannot borrow more.");
        }
            
        if (activeLoans.Count >= policy.MaxActiveLoans)
        {
            throw new DomainException($"{member.MembershipType} members can borrow at most {policy.MaxActiveLoans} books.");
        }
            
        if (activeLoans.Any(l => l.BookId == book.Id))
        {
            throw new DomainException("Member has already borrowed this book.");
        }
            
        book.CheckOut();
        var loan = new Loan(book, member, policy, now);
        db.Loans.Add(loan);

        await db.SaveChangesAsync(ct);
        return loan.ToResponse(now);
    }
    public async Task<LoanResponse> ReturnAsync(int loanId, CancellationToken ct)
    {
        var now = Now;
        var loan = await db.Loans.Include(l => l.Book)
                                 .Include(l => l.Member)
                                 .FirstOrDefaultAsync(l => l.Id == loanId, ct);
        if(loan == null)
        {
            throw new NotFoundException(nameof(Loan), loanId);
        }
        var policy = BorrowingPolicy.For(loan.Member.MembershipType);
        loan.MarkReturned(now, policy);
        loan.Book.CheckIn();
        await db.SaveChangesAsync(ct);
        return loan.ToResponse(now);
    }
}