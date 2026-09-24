using LibraryManagement.API.Domain.Enums;
using LibraryManagement.API.Domain.Common;
using LibraryManagement.API.Domain.Exceptions;
using LibraryManagement.API.Domain.Entity;
using LibraryManagement.API.Domain.Policy;

namespace LibraryManagement.API.Domain.Entity;

public class Loan : BaseEntity
{
    public int BookId { get; private set; }
    public Book Book { get; private set; } = null!;
    public int MemberId { get; private set; }
    public Member Member { get; private set; } = null!;
    public DateTime BorrowedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnedAt { get; private set; }
    public decimal FineAmount { get; private set; }
    public bool IsReturned { get { return ReturnedAt.HasValue; } }

    private Loan() { } 
    public Loan(Book book, Member member, BorrowingPolicy policy, DateTime now)
    {
        Book = book;
        BookId = book.Id;
        Member = member;
        MemberId = member.Id;
        BorrowedAt = now;
        DueDate = now.AddDays(policy.LoanPeriodDays);
    }
    public bool IsOverdue(DateTime now)
    {
        if (!IsReturned && now > DueDate)
        {
            return true;
        }

        return false;
    }
    public LoanStatus GetStatus(DateTime now)
    {
        if (IsReturned)
        {
            return LoanStatus.Returned;
        }

        if (IsOverdue(now))
        {
            return LoanStatus.Overdue;
        }

        return LoanStatus.Active;
    }
    public void MarkReturned(DateTime now, BorrowingPolicy policy)
    {
        if (IsReturned)
        {
            throw new DomainException("This loan has already been returned.");
        }
        ReturnedAt = now;
        var daysLate = (int)Math.Ceiling((now - DueDate).TotalDays);
        FineAmount = policy.CalculateFine(daysLate);
        MarkUpdated();
    }
}