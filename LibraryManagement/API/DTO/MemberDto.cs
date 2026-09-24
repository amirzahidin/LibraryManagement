using System.ComponentModel.DataAnnotations;
using LibraryManagement.API.Domain.Enums;

namespace LibraryManagement.API.DTO;

public record CreateMemberRequest([Required, StringLength(150)] string FullName,
                                  [Required, EmailAddress, StringLength(200)] string Email,
                                  MembershipType MembershipType);
public record UpdateMemberRequest([Required, StringLength(150)] string FullName,
                                  [Required, EmailAddress, StringLength(200)] string Email,
                                  MembershipType MembershipType);
public record MemberResponse(int Id, string FullName, string Email, MembershipType MembershipType, bool IsActive);