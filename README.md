# Library Management System API

A RESTful Web API for managing books, members and loans, built with ASP.NET Core and Entity Framework Core using object-oriented design.

## Tech stack

- .NET 10, ASP.NET Core Web API (controllers)
- Entity Framework Core with SQLite (zero-setup local database)
- xUnit for unit tests
- Swagger (Swashbuckle) for API documentation


## Business rules

| Rule | Standard | Premium |
| --- | --- | --- |
| Maximum active loans | 3 | 10 |
| Loan period | 14 days | 30 days |
| Late fine | RM1.00 per day | RM0.50 per day after a 3-day grace period |

- A book can only be borrowed when a copy is available.
- Members with overdue books cannot borrow more.
- A member cannot borrow the same book twice at the same time.
- Inactive members cannot borrow.
- A book with loan history cannot be deleted.
- Total copies cannot be reduced below the number currently borrowed.
- Members are deactivated (soft delete) instead of deleted, to keep loan history. A member with unreturned books cannot be deactivated.


## Tests

11 unit tests cover the core business logic:

- **BookTests:** checking out copies, running out of copies, rejecting zero copies.
- **LoanTests:** on-time and late returns, fine calculation, returning twice, overdue status.
- **BorrowingPolicyTests:** Standard fines, Premium grace period.

