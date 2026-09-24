using LibraryManagement.API.Data;
using LibraryManagement.API.Middleware;
using LibraryManagement.API.Service;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

//	Controller
builder.Services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//	Database
builder.Services.AddDbContext<LibraryDbContext>(options =>options.UseSqlite(builder.Configuration.GetConnectionString("LibraryDb")));

//	Dependency injection
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ILoanService, LoanService>();

//	Error handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
await db.Database.MigrateAsync();
await DbSeeder.SeedAsync(db);
}

app.MapControllers();
app.Run();

public partial class Program { }    