using LibraryManagement.API.Domain.Entity;
using LibraryManagement.API.Domain.Enums;
using LibraryManagement.API.Domain.Exceptions;
using LibraryManagement.API.Domain.Policy;
namespace LibraryManagement.Tests;

public class LoanTests
{
    private static readonly DateTime BorrowDate = new(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
    private static Loan CreateLoan(MembershipType type)
    {
        var book = new Book("Clean Code", "Robert C. Martin", "9780132350884", 2008, 1);
        var member = new Member("Ali Ahmad", "ali@example.com", type);
        return new Loan(book, member, BorrowingPolicy.For(type), BorrowDate);
    }
    [Fact]
    public void MarkReturned_ThreeDaysLate_StandardMember_ChargesFine()
    {
        var policy = BorrowingPolicy.For(MembershipType.Standard);
        var loan = CreateLoan(MembershipType.Standard);
        loan.MarkReturned(loan.DueDate.AddDays(3), policy);
        Assert.Equal(3.00m, loan.FineAmount);
        Assert.Equal(LoanStatus.Returned, loan.GetStatus(loan.DueDate.AddDays(3)));
    }
    [Fact]
    public void MarkReturned_OnTime_NoFine()
    {
        var policy = BorrowingPolicy.For(MembershipType.Standard);
        var loan = CreateLoan(MembershipType.Standard);
        loan.MarkReturned(loan.DueDate.AddDays(-1), policy);
        Assert.Equal(0m, loan.FineAmount);
    }
    [Fact]
    public void MarkReturned_Twice_ThrowsDomainException()
    {
        var policy = BorrowingPolicy.For(MembershipType.Standard);
        var loan = CreateLoan(MembershipType.Standard);
        loan.MarkReturned(loan.DueDate, policy);
        Assert.Throws<DomainException>(() => loan.MarkReturned(loan.DueDate, policy));
    }
    [Fact]
    public void GetStatus_AfterDueDate_IsOverdue()
    {
        var loan = CreateLoan(MembershipType.Standard);
        Assert.Equal(LoanStatus.Overdue, loan.GetStatus(loan.DueDate.AddDays(1)));
    }
}