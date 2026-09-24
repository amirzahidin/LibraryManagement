using LibraryManagement.API.Domain.Enums;
using LibraryManagement.API.Domain.Policy;

namespace LibraryManagement.Test;

public class BorrowingPolicyTests
{
    [Theory]
    [InlineData(0, 0.0)]
    [InlineData(5, 5.0)]
    public void Standard_CalculateFine_ChargesPerDay(int daysOverdue, double expected)
    {
        var policy = BorrowingPolicy.For(MembershipType.Standard);
        Assert.Equal((decimal)expected, policy.CalculateFine(daysOverdue));
    }
    [Theory]
    [InlineData(2, 0.0)]          
    [InlineData(5, 1.0)]            
    public void Premium_CalculateFine_AppliesGracePeriod(int daysOverdue, double expected)
    {
        var policy = BorrowingPolicy.For(MembershipType.Premium);
        Assert.Equal((decimal)expected, policy.CalculateFine(daysOverdue));
    }
}