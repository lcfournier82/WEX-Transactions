using WEX.TransactionAPI.Application;
using WEX.TransactionAPI.Infrastructure;
using WEX.TransactionAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using wex_transactions_api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services from other layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Web layer services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the custom exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Apply migrations on startup
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PurchaseDbContext>();
    dbContext.Database.Migrate();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating the database.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable the custom exception handler
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();

// Make Program public for the Test project
public partial class Program { }
