using LibraryManagement.API.Domain.Entity;
using LibraryManagement.API.DTO;

namespace LibraryManagement.API.Mapping;

public static class MappingExtensions
{
    public static BookResponse ToResponse(this Book b) {
        return new BookResponse(b.Id, b.Title, b.Author, b.ISBN, b.PublishedYear, b.TotalCopies, b.CopiesAvailable);
    }
                  
    public static MemberResponse ToResponse(this Member m) {
        return new MemberResponse(m.Id, m.FullName, m.Email, m.MembershipType, m.IsActive);
    }
    public static LoanResponse ToResponse(this Loan l, DateTime now) {
        return new LoanResponse(l.Id, l.BookId, l.Book.Title, l.MemberId, l.Member.FullName,
                                l.BorrowedAt, l.DueDate, l.ReturnedAt, l.FineAmount, l.GetStatus(now));
    }
}