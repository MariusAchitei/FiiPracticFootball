using FiiPracticFootball;
using Microsoft.EntityFrameworkCore;
using FiiPracticFootball.Repositories.Interfaces;
using FiiPracticFootball.Repositories.Implementations;
using FIIPracticCars.Services;
using FIIPracticFootball.Services;
using FiiPracticFootball.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<FootballContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FIIPracticFootball;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True")
    .LogTo(Console.WriteLine, minimumLevel: LogLevel.Information))
    .AddScoped<IFootballUnitOfWork, FootballUnitOfWork>()
    .AddScoped<IMatchRepository, MatchRepository>()
    .AddScoped<ISeasonRepository, SeasonRepository>()
    .AddScoped<IClubRepository, ClubRepository>()
    .AddScoped<ILeagueRepository, LeagueRepository>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IPlayerRepository, PlayerRepository>()
    .AddScoped<ICryptographyService, CryptographyService>()
    .BuildServiceProvider();

builder.Services.AddControllersWithViews();

builder.Services
  .AddAuthentication(AuthFootballConstants.UserSchema)
  .AddCookie(AuthFootballConstants.UserSchema);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
