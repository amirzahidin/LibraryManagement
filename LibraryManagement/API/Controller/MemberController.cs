using LibraryManagement.API.Domain.Enums;
using LibraryManagement.API.DTO;
using LibraryManagement.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controller;

[ApiController]
[Route("api/members")]
[Produces("application/json")]
public class MembersController(IMemberService memberService, ILoanService loanService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MemberResponse>>> GetAll(CancellationToken ct)
    {
        var result = await memberService.GetAllAsync(ct);
        return Ok(result);
    }
                    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberResponse>> GetById(int id, CancellationToken ct)
    {
        var member = await memberService.GetByIdAsync(id, ct);
        return Ok(member);
    }
                    
    [HttpPost]
    public async Task<ActionResult<MemberResponse>> Create(CreateMemberRequest request, CancellationToken ct)
    {
        var member = await memberService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MemberResponse>> Update(int id, UpdateMemberRequest request, CancellationToken ct)
    {
        var member = await memberService.UpdateAsync(id, request, ct);
        return Ok(member);
    }
                   
   
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
    {
        await memberService.DeactivateAsync(id, ct);
        return NoContent();
    }

    [HttpGet("{id:int}/loans")]
    public async Task<ActionResult<IReadOnlyList<LoanResponse>>> GetLoans(int id, [FromQuery] LoanStatus? status, CancellationToken ct)
    {
        await memberService.GetByIdAsync(id, ct);   //	returns	404	if	the	member	doesn't	exist
        return Ok(await loanService.GetLoansAsync(id, status, ct));
    }
}