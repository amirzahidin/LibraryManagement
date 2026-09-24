using LibraryManagement.API.Domain.Common;
using LibraryManagement.API.Domain.Enums;
using LibraryManagement.API.Domain.Exceptions;

namespace LibraryManagement.API.Domain.Entity;

public class Member : BaseEntity
{
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public MembershipType MembershipType { get; private set; }
    public bool IsActive { get; private set; }
    private Member() { }
    public Member(string fullName, string email, MembershipType membershipType)
    {
        SetDetails(fullName, email, membershipType);
        IsActive = true;
    }
    public void UpdateDetails(string fullName, string email, MembershipType membershipType)
    {
        SetDetails(fullName, email, membershipType);
        MarkUpdated();
    }
    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Member is already inactive.");
        }
        IsActive = false;
        MarkUpdated();
    }
    private void SetDetails(string fullName, string email, MembershipType membershipType)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new DomainException("Full name is required.");
        }
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new DomainException("Valid email is required.");
        }
        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        MembershipType = membershipType;
    }
}
