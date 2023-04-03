using FiiPracticFootball.Entities;
using FiiPracticFootball.Repositories.Dtos;
using FiiPracticFootball.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Repositories.Implementations
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly FootballContext _context;

        public PlayerRepository(FootballContext context)
        {
            _context = context;
        }
        public List<PlayerDto> GetPlayersByClub(int clubId)
        {
            var playersDto = _context.Players
                .Include(p => p.Country)
                .Where(p => p.ClubId == clubId)
                .Select(p => new PlayerDto()
                {
                    Id = p.ClubId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    CountryId = p.CountryId,
                    Country = p.Country,
                    BirthDate = p.BirthDate,
                    Value = p.Value,
                    Photo = p.Photo,
                    Number = p.Number,
                    Rating = p.Rating,
                    ClubId = p.ClubId,
                })
                .ToList();
            return playersDto;
        }
    }
}
