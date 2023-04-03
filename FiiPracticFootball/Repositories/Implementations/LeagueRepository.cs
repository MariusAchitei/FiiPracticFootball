using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiiPracticFootball.Repositories.Dtos;
using FiiPracticFootball.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiiPracticFootball.Repositories.Implementations
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly FootballContext _context;

        public LeagueRepository(FootballContext context)
        {
            _context = context;
        }
        public LeagueDto? getLeague(int leagueId)
        { 
            return _context.Leagues
                .Include(l=> l.Country)
                .Where(l=> l.Id == leagueId)
                .Select(l=> new LeagueDto(l))
                .FirstOrDefault();
        }
        public List<LeagueDto> getAllLeagues()
        {
            return _context.Leagues
                .Include(l=> l.Country)
                .Select(l=> new LeagueDto(l))
                .ToList();
        }

    }
}
