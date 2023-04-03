using FIIPracticCars.Entities;
using FiiPracticFootball.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FiiPracticFootball
{
    public class FootballContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Match> Matches{ get; set; }
        public DbSet<Played> Played { get; set; }
        public DbSet<Season> Seasons{ get; set; }
        public DbSet<SeasonStats> SeasonStats { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public FootballContext(DbContextOptions options)
            : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectionString =
        //      "Server=localhost\\SQLEXPRESS;Database=FIIPracticFootball;" +
        //      "Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
        //    optionsBuilder.UseSqlServer(connectionString)
        //      .LogTo(Console.WriteLine, minimumLevel: LogLevel.None);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTeam>()
              .HasKey(uv => new { uv.UserId, uv.TeamId });

            modelBuilder.Entity<User>()
              .HasMany(u => u.Clubs)
              .WithMany(v => v.Users)
              .UsingEntity<UserTeam>();


            modelBuilder.Entity<Played>()
                .HasKey(p => new { p.MatchId, p.PlayerId});
            modelBuilder.Entity<SeasonStats>()
                .HasKey(s => new { s.SeasonId, s.ClubId });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments);

            modelBuilder.Entity<Match>()
                .HasMany(m => m.Comments);

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User);
            modelBuilder.Entity<Comment>()
               .HasOne(u => u.Match);
            modelBuilder.Entity<SeasonStats>()
                .HasOne(s => s.Club);

            modelBuilder.Entity<SeasonStats>()
                .HasOne(s => s.Season);
            modelBuilder.Entity<Club>()
                .HasOne(c => c.Country);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Club)
                .WithMany(c => c.Players)
                ;
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Country);

            modelBuilder.Entity<Season>()
                .HasOne(s => s.League)
                .WithMany(l => l.Seasons);

            modelBuilder.Entity<Match>()
                .HasOne(s => s.Season)
                .WithMany(s => s.Matches);

            modelBuilder.Entity<Match>()
                .HasMany(m => m.Players)
                .WithMany(p => p.Matches)
                .UsingEntity<Played>();
        }
    }
}
