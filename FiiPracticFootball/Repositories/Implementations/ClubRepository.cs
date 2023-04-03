using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiiPracticFootball.Entities;
using FiiPracticFootball.Repositories.Dtos;
using FiiPracticFootball.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FiiPracticFootball.Repositories.Implementations
{
    public class ClubRepository : IClubRepository
    {
        private readonly FootballContext _context;

        public ClubRepository(FootballContext context)
        {
            _context = context;
        }
        public List<ClubDto> GetAll()
        {
            return _context.Clubs
                .Include(c=> c.Country)
                .Select(c => new ClubDto
                {
                    Id = c.Id,
                    CountryId = c.CountryId,
                    Name = c.Name,
                    Logo = c.Logo,
                    Country = c.Country,
                })
                .ToList();
        }
        public List<ClubDto> getClubsByCountry(int countryId)
        {
            return _context.Clubs
                .Where(c => c.CountryId == countryId)
                .Select(c => new ClubDto
                {
                    Id = c.Id,
                    CountryId = countryId,
                    Name = c.Name,
                    Logo = c.Logo,
                })
                .ToList();
        }
        public ClubDto getClub(int clubId)
        {
            if (clubId <= 0) throw new ArgumentOutOfRangeException(nameof(clubId));
            var club = _context.Clubs
                .Include(c=>c.Country)
                .Where(c=> c.Id==clubId)
                .FirstOrDefault();
            if (club == null) return null;
            return new ClubDto() {
                Id = club.Id,
                CountryId = club.CountryId,
                Name = club.Name,
                Logo = club.Logo,
                Country = club.Country,
            };
        }
        public List<SeasonStats> getClubStats(int clubId)
        {
            if (clubId <= 0) throw new ArgumentOutOfRangeException(nameof(clubId));
            var stats = _context.SeasonStats
                .Include(ss=> ss.Season)
                .ThenInclude(s=>s.League)
                .Where(ss=>ss.ClubId==clubId)
                .ToList();
            return stats;
        }

        public List<ClubDto> getSupportedClubs(int userId)
        {
            throw new NotImplementedException();
        }

        public void addToFavorte(int userId, int clubId)
        {
            if (clubId <1) throw new ArgumentOutOfRangeException(nameof(clubId));
            if (userId < 1) throw new ArgumentOutOfRangeException(nameof(userId));

            var test = _context.UserTeams
                .Where(ss => ss.UserId == userId && ss.TeamId == clubId)
                .FirstOrDefault();
            if (test != null)
                return;

            var userTeamEntity = new UserTeam()
            {
                TeamId = clubId,
                UserId = userId,
            };
            _context.UserTeams.Add(userTeamEntity);

        }

        public List<ClubDto> getUserFavorites(int userId)
        {
            //var clubIds = _context.Users
            //    .Include(u => u.Supports)
            //    .ThenInclude(c=>c.Country)
            //    .Select(c => new ClubDto
            //    {
            //        Id = c.Id,
            //        CountryId = c.CountryId,
            //        Name = c.Name,
            //        Logo = c.Logo,
            //        Country = c.Country,
            //    })
            //    .ToList;

            var user = _context.Users
              .Include(u => u.Clubs)
              .ThenInclude(c => c.Country)
              .Where(u => u.Id == userId)
              .FirstOrDefault();

            return user.Clubs.
                Select(c => new ClubDto
                {
                    Id = c.Id,
                    CountryId = c.CountryId,
                    Name = c.Name,
                    Logo = c.Logo,
                    Country = c.Country,
                })
                .ToList();
        }

        public void removeFavorite(int userId, int clubId)
        {
            var preference = _context.UserTeams
                 .Where(ut=>ut.UserId==userId && ut.TeamId==clubId)
                 .FirstOrDefault();
            _context.UserTeams.Remove(preference);
            return;
        }
    }
}
