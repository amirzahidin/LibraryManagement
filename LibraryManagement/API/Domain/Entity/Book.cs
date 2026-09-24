using LibraryManagement.API.Domain.Common;
using LibraryManagement.API.Domain.Exceptions;

namespace LibraryManagement.API.Domain.Entity;

public class Book : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Author { get; private set; } = null!;
    public string ISBN { get; private set; } = null!;
    public int PublishedYear { get; private set; }
    public int TotalCopies { get; private set; }
    public int CopiesAvailable { get; private set; }

    public bool IsAvailable
    {
        get
        {
            return CopiesAvailable > 0;
        }
    }

    private Book() { }
    public Book(string title, string author, string isbn, int publishedYear, int totalCopies)
    {
        SetDetails(title, author, isbn, publishedYear);

        if(totalCopies < 1)
        {
            throw new DomainException("Total copies must be at least 1.");
        }
        TotalCopies = totalCopies;
        CopiesAvailable = totalCopies;

    }
    public void UpdateDetails(string title, string author, string isbn, int publishedYear)
    {
        SetDetails(title, author, isbn, publishedYear);
        MarkUpdated();
    }
    public void UpdateTotalCopies(int newTotal)
    {
        var borrowed = TotalCopies - CopiesAvailable;
        if (newTotal < borrowed)
        {
            throw new DomainException($"Total copies cannot be less than {borrowed} (currently borrowed).");
        }
           
        TotalCopies = newTotal;
        CopiesAvailable = newTotal - borrowed;
        MarkUpdated();
    }
    public void CheckOut()
    {
        if (!IsAvailable)
        {
            throw new DomainException($"'{Title}' has no available copies.");
        }
        CopiesAvailable--;
        MarkUpdated();
    }
    public void CheckIn()
    {
        if (CopiesAvailable >= TotalCopies)
        {
            throw new DomainException($"All copies of '{Title}' are already in the library.");
        }
        CopiesAvailable++;
        MarkUpdated();
    }
    private void SetDetails(string title, string author, string isbn, int publishedYear)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Title is required.");
        }
        if (string.IsNullOrWhiteSpace(author)) 
        {
            throw new DomainException("Author is required.");
        }
        if (string.IsNullOrWhiteSpace(isbn)) 
        {
            throw new DomainException("ISBN is required.");
        }
        if (publishedYear < 1000 || publishedYear > DateTime.Now.Year) 
        {
            throw new DomainException("Published year is invalid.");
        }
        Title = title.Trim();
        Author = author.Trim();
        ISBN = isbn.Trim();
        PublishedYear = publishedYear;
    }
}
