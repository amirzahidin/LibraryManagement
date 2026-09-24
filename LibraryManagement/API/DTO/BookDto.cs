using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTO;

public record CreateBookRequest([Required, StringLength(200)] string Title,
                                [Required, StringLength(150)] string Author,
                                [Required, StringLength(20)] string ISBN,
                                [Range(1000, 2100)] int PublishedYear,
                                [Range(1, 1000)] int TotalCopies);
public record UpdateBookRequest([Required, StringLength(200)] string Title,
                                [Required, StringLength(150)] string Author,
                                [Required, StringLength(20)] string ISBN,
                                [Range(1000, 2100)] int PublishedYear,
                                [Range(1, 1000)] int TotalCopies);
public record BookResponse(int Id, string Title, string Author, string ISBN,int PublishedYear, int TotalCopies, int CopiesAvailable);