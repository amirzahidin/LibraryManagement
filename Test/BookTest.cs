using LibraryManagement.API.Domain.Entity;
using LibraryManagement.API.Domain.Exceptions;

namespace LibraryManagement.Test;

public class BookTests
{
    [Fact]
    public void CheckOut_WhenCopiesAvailable_DecreasesAvailableCopies()
    {
        var book = new Book("Clean	Code", "Robert	C.	Martin", "9780132350884", 2008, 2);
        book.CheckOut();
        Assert.Equal(1, book.CopiesAvailable);
    }
    [Fact]
    public void CheckOut_WhenNoCopiesLeft_ThrowsDomainException()
    {
        var book = new Book("Clean	Code", "Robert	C.	Martin", "9780132350884", 2008, 1);
        book.CheckOut();
        Assert.Throws<DomainException>(() => book.CheckOut());
    }

    [Fact]
    public void Constructor_WithZeroCopies_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() =>
        new Book("Clean	Code", "Robert	C.	Martin", "9780132350884", 2008, 0));
    }
}