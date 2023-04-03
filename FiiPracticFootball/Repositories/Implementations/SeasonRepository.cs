using FiiPracticFootball.Entities;
using FiiPracticFootball.Repositories.Dtos;
using FiiPracticFootball.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FiiPracticFootball.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace FiiPracticFootball.Repositories.Implementations
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly FootballContext _context;
        private readonly MatchRepository _matchRepository;

        public SeasonRepository(FootballContext context)
        {
            _context = context;
            _matchRepository = new MatchRepository(context);
        }

        public DateTime getRoundDateTime(int round)
        {
            Random random = new Random();
            int[] matchHours = new int[] { 18, 19, 20, 21, 22, 17 };
            int randomHour = matchHours[random.Next(0, matchHours.Length)];
            TimeSpan matchTime = new TimeSpan(randomHour, 0, 0);
            DateTime matchDateTime = DateTime.Today.AddDays(round*7) + matchTime;
            return matchDateTime;
        }
        public void ScheduleMatch(int seasonId, int team1, int team2, int round, int teamsNr)
        {
            DateTime matchDate = getRoundDateTime(round);
            _matchRepository.CreateMatch(
                        new MatchDto(seasonId, team1, team2, round, matchDate),
                        new List<Player>());
            //_matchRepository.CreateMatch(
            //            new MatchDto(seasonId, team2, team1, teamsNr + round - 1),
            //            new List<Player>());
        }
        public int CreateSeason(SeasonDto seasonDto, List<ClubDto> clubs)
        {
            if (seasonDto == null) throw new ArgumentNullException(nameof(seasonDto));
            if (seasonDto.LeagueId < 1) throw new ArgumentException($"{nameof(seasonDto.LeagueId)} cannot be null or empty.");
            if (string.IsNullOrEmpty(seasonDto.Edition)) throw new ArgumentException($"{nameof(seasonDto.Edition)} cannot be null or empty.");
            if (clubs.Count % 2 == 1 || clubs.Count < 2) throw new ArgumentException($"{nameof(clubs)} must be even and bigger than 2");

            var seasonEntity = new Season
            {
                LeagueId = seasonDto.LeagueId,
                Edition = seasonDto.Edition
            };
            _context.Seasons.Add(seasonEntity);
            _context.SaveChanges();

            foreach (ClubDto club in clubs)
            {
                var seasonStatsEntity = new SeasonStats
                {
                    ClubId = club.Id,
                    SeasonId = seasonEntity.Id,
                    Played = 0,
                    Won = 0,
                    Lost = 0,
                    Tie = 0,
                    Scored = 0,
                    Conceded = 0,
                };
                _context.SeasonStats.Add(seasonStatsEntity);
            }
            int n = clubs.Count;

            for (int i = 0; i < n - 1; i++)
            {
                int j, round = 1;
                if (i == 0) j = 0;
                else j = n - 1 - i;
                for (int k = 0; k < n - 1; k++, j--, round++)
                {
                    if (j == i)
                    {
                        ScheduleMatch(seasonEntity.Id, clubs.ElementAt(i).Id, clubs.ElementAt(n - 1).Id, round, n);
                        ScheduleMatch(seasonEntity.Id, clubs.ElementAt(n - 1).Id, clubs.ElementAt(i).Id, round + n - 1, n);
                    }
                    else
                        if (i < j) ScheduleMatch(seasonEntity.Id, clubs.ElementAt(i).Id, clubs.ElementAt(j).Id, round, n);
                    else ScheduleMatch(seasonEntity.Id, clubs.ElementAt(i).Id, clubs.ElementAt(j).Id, round + n - 1, n);
                    if (j == 0) j = n - 1;
                }
            }
            return seasonEntity.Id;
        }
        public List<SeasonDto> GetAll()
        {
            return _context.Seasons
                .Select(s => new SeasonDto
                {
                    LeagueId = s.LeagueId,
                    Id = s.Id,
                    Edition = s.Edition,
                }).ToList();
        }
        public SeasonDto? GetSeason(int seasonId)
        {
            if (seasonId <= 0) throw new ArgumentOutOfRangeException(nameof(seasonId));

            var seasonEntity = _context.Seasons
                .Where(s => s.Id == seasonId)
                .FirstOrDefault();

            if (seasonEntity == null) return null;

            return new SeasonDto(seasonEntity);
        }
        public void Delete(int seasonId)
        {
            if (seasonId <= 0) throw new ArgumentOutOfRangeException(nameof(seasonId));

            var seasonToDelete = _context.Seasons.Find(seasonId);
            var matchesToDelete = _context.Matches
                .Where(m=>m.SeasonId == seasonId)
                .ToList();
            var statsToDelete = _context.SeasonStats
                .Where(ss => ss.SeasonId == seasonId)
                .ToList();
            foreach ( var matchToDelete in matchesToDelete)
            {
                var commentsToDelete = _context.Comments
                    .Where(c => c.MatchId==matchToDelete.Id)
                    .ToList();
                foreach( var commentToDelete in commentsToDelete)
                    _context.Comments.Remove(commentToDelete);
                _context.Matches.Remove(matchToDelete);
            }
            foreach ( var statToDelete in statsToDelete) { 
                _context.SeasonStats.Remove(statToDelete);
            }

            if (seasonToDelete != null)
            {
                _context.Seasons.Remove(seasonToDelete);
            }
        }
        public List<SeasonStats> getSeasonStats(int seasonId)
        {
            return _context.SeasonStats
                .Include(ss=> ss.Club)
                .Where(ss=> ss.SeasonId == seasonId)
                .OrderByDescending(ss=>ss.Won*3 + ss.Tie)
                .OrderByDescending(ss => ss.Scored-ss.Conceded)
                .ToList();
        }
    }
}
