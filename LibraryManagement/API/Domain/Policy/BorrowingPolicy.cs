using LibraryManagement.API.Domain.Enums;

namespace LibraryManagement.API.Domain.Policy;

public abstract class BorrowingPolicy
{
    public abstract int MaxActiveLoans { get; }
    public abstract int LoanPeriodDays { get; }
    public abstract decimal FinePerDay { get; }
    public virtual decimal CalculateFine(int daysOverdue)
    {
        if (daysOverdue <= 0)
        {
            return 0m;
        }
        else
        {
            return daysOverdue * FinePerDay;
        }


    }
    public static BorrowingPolicy For(MembershipType type)
    {
        if (type == MembershipType.Standard)
        {
            return new StandardBorrowingPolicy();
        }

        if (type == MembershipType.Premium)
        {
            return new PremiumBorrowingPolicy();
        }

        throw new ArgumentOutOfRangeException(nameof(type),type,"Unknown membership type.");
    }
    public sealed class StandardBorrowingPolicy : BorrowingPolicy
    {
        public override int MaxActiveLoans { get { return 3; } }
        public override int LoanPeriodDays { get { return 14; } }
        public override decimal FinePerDay { get { return 1.00m; } }
    }
    public sealed class PremiumBorrowingPolicy : BorrowingPolicy
    {
        private const int GracePeriodDays = 3;
        public override int MaxActiveLoans { get { return 10; } }
        public override int LoanPeriodDays { get { return 30; } }
        public override decimal FinePerDay { get { return 0.50m; } }

        public override decimal CalculateFine(int daysOverdue)
        {
            int actualOverdueDays = daysOverdue - GracePeriodDays;

            return base.CalculateFine(actualOverdueDays);
        }
    }
}
