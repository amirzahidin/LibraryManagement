using LibraryManagement.API.Domain.Enums;
using LibraryManagement.API.DTO;
using LibraryManagement.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controller;

[ApiController]
[Route("api/loans")]
[Produces("application/json")]
public class LoansController(ILoanService loanService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LoanResponse>>> GetAll([FromQuery] int? memberId, [FromQuery] LoanStatus? status, CancellationToken ct)
    {
        var result = await loanService.GetLoansAsync(memberId, status, ct);
        return Ok(result);
    }
                    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanResponse>> GetById(int id, CancellationToken ct)
    {
        var loan = await loanService.GetByIdAsync(id, ct);
        return Ok(loan);
    }
                   
    [HttpPost]
    public async Task<ActionResult<LoanResponse>> Borrow(BorrowBookRequest request, CancellationToken ct)
    {
        var loan = await loanService.BorrowAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    [HttpPost("{id:int}/return")]
    public async Task<ActionResult<LoanResponse>> Return(int id, CancellationToken ct)
    {
        var loan = await loanService.ReturnAsync(id, ct);
        return Ok(loan);
    }
}