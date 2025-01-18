using FlightAlertSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();
builder.Services.AddDbContext<FlightAlertContext>(options =>
    options.UseInMemoryDatabase("FlightAlertDB")); // Use an in-memory database for now

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

// Enable Swagger for API testing in Development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware to redirect HTTP to HTTPS
app.UseHttpsRedirection();

app.UseAuthorization();

// Map the controllers
app.MapControllers();

app.Run();