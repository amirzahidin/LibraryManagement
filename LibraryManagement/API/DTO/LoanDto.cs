using System.ComponentModel.DataAnnotations;
using LibraryManagement.API.Domain.Enums;

namespace LibraryManagement.API.DTO;

public record BorrowBookRequest([Range(1, int.MaxValue)] int MemberId,
                                [Range(1, int.MaxValue)] int BookId);
public record LoanResponse(int Id, int BookId, string BookTitle, int MemberId, string MemberName,
                           DateTime BorrowedAt, DateTime DueDate, DateTime? ReturnedAt,
                           decimal FineAmount, LoanStatus Status);