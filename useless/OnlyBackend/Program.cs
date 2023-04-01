using FiiPracticFootball.Repositories;
using FiiPracticFootball;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FootballContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FIIPracticFootball;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True")
    .LogTo(Console.WriteLine, minimumLevel: LogLevel.Information))
    .AddScoped<IFootballUnitOfWork, FootballUnitOfWork>()
    .AddScoped<IMatchRepository, MatchRepository>()
    .AddScoped<ISeasonRepository, SeasonRepository>()
    .AddScoped<IClubRepository, ClubRepository>()
    .BuildServiceProvider();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
