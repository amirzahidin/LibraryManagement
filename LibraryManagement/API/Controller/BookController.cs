using LibraryManagement.API.DTO;
using LibraryManagement.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controller;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<BookResponse>>> GetAll(string? search,int page = 1,int pageSize = 20,CancellationToken ct = default)
    {
        var result = await bookService.GetAllAsync(search,page,pageSize,ct);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponse>> GetById(int id,CancellationToken ct)
    {
        var book = await bookService.GetByIdAsync(id, ct);

        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> Create(CreateBookRequest request,CancellationToken ct)
    {
        var book = await bookService.CreateAsync(request, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = book.Id },
            book);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookResponse>> Update(int id, UpdateBookRequest request, CancellationToken ct)
    {
        var book = await bookService.UpdateAsync(id, request, ct);
        return book;
    }
   
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await bookService.DeleteAsync(id, ct);
        return NoContent();
    }
}
