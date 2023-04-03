using FiiPracticFootball.Entities;
using FiiPracticFootball.Repositories.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiiPracticFootball.Repositories.Interfaces;


namespace FiiPracticFootball.Repositories.Implementations
{
    public class MatchRepository : IMatchRepository
    {
        private readonly FootballContext _context;

        public MatchRepository(FootballContext context)
        {
            _context = context;
        }
        public void updateMatch(Match matchDto)
        {
            if (matchDto == null) throw new ArgumentNullException(nameof(matchDto));
            if (matchDto.Id < 1) throw new ArgumentException($"{nameof(matchDto.Id)} cannot be null or empty.");
            if (matchDto.Round < 1) throw new ArgumentException($"{nameof(matchDto.Round)} cannot be null or empty.");
            if (matchDto.Date == null) throw new ArgumentException($"{nameof(matchDto.Date)} cannot be null or empty.");

            var matchToUpdate = _context.Matches.Find(matchDto.Id);
            if (matchToUpdate == null)
            {
                throw new Exception("The user has not been found");
            }

            matchToUpdate.Date = matchDto.Date;
            matchToUpdate.Stadium = matchDto.Stadium;
            if (matchToUpdate.Status == true)
                return;
            if (matchDto.HostScore == null || matchDto.VisitScore == null)
                return;

            matchToUpdate.Status = true;
            matchToUpdate.HostScore = matchDto.HostScore;
            matchToUpdate.VisitScore = matchDto.VisitScore;

            AddMatchResult(matchDto.SeasonId, matchDto.HostId, matchDto.HostScore, matchDto.VisitScore);
            AddMatchResult(matchDto.SeasonId, matchDto.VisitId, matchDto.VisitScore, matchDto.HostScore);

        }
        public void AddMatchResult(int seasonId, int teamId, int? teamScore, int? opponentScore)
        {
            var teamStats = _context.SeasonStats
                .Where(ss => ss.ClubId == teamId && ss.SeasonId == seasonId)
                .FirstOrDefault();
            if (teamStats == null) { return; }
            teamStats.Scored += teamScore ?? default;
            teamStats.Conceded += opponentScore ?? default;
            teamStats.Played++;
            if (teamScore > opponentScore)
                teamStats.Won++;
            if (teamScore < opponentScore)
                teamStats.Lost++;
            if (teamScore == opponentScore)
                teamStats.Tie++;
        }
        public int CreateMatch(MatchDto matchDto, List<Player> players)
        {
            if (matchDto == null) throw new ArgumentNullException(nameof(matchDto));
            if (matchDto.HostId < 1) throw new ArgumentException($"{nameof(matchDto.HostId)} cannot be null or empty.");
            if (matchDto.VisitId < 1) throw new ArgumentException($"{nameof(matchDto.VisitId)} cannot be null or empty.");
            if (matchDto.SeasonId < 1) throw new ArgumentException($"{nameof(matchDto.SeasonId)} cannot be null or empty.");

            var matchEntity = new Match
            {
                VisitId = matchDto.VisitId,
                HostId = matchDto.HostId,
                SeasonId = matchDto.SeasonId,
                Date = matchDto.Date,
                HostScore = matchDto.HostScore,
                VisitScore = matchDto.VisitScore,
                Status = false,
                Round = matchDto.Round,
            };

            _context.Matches.Add(matchEntity);
            foreach (var player in players)
            {
                var playedEntity = new Played
                {
                    PlayerId = player.Id,
                    MatchId = matchEntity.Id,
                    Status = 0
                };
                _context.Played.Add(playedEntity);
            }
            if (matchEntity.HostScore != null && matchEntity.VisitScore != null)
            {
                matchEntity.Status = true;
                AddMatchResult(matchDto.SeasonId, matchDto.HostId, matchDto.HostScore, matchDto.VisitScore);
                AddMatchResult(matchDto.SeasonId, matchDto.VisitId, matchDto.VisitScore, matchDto.HostScore);
            }
            return matchEntity.Id;
        }
        public Match? SearchById(int matchId)
        {
            if (matchId < 0)
                return null;
            return _context.Matches
                .Include(m => m.Host)
                .Include(m => m.Visit)
                .Where(m => m.Id == matchId)
                .FirstOrDefault();
        }
        public List<Match> searchBySeason(int seasonId)
        {
            return _context.Matches
                .Include(m => m.Host)
                .Include(m => m.Visit)
                //.Select(m-> new MatchDto
                //{
                //    Id = m.Id,
                //    SeasonId = seasonId,
                //    HostId = m.HostId,
                //    HostId

                //})
                .Where(m => m.SeasonId == seasonId)
                .OrderBy(m => m.Round)
                .ToList();
        }
        public void DeleteMatch(int matchId)
        {

        }
        public void addComment(int userId, int matchId, string message)
        {
            var CommentEntity = new Comment()
            {
                UserId = userId,
                MatchId = matchId,
                Message = message,
                Date = DateTime.UtcNow
            };
            _context.Comments.Add(CommentEntity);
        }
        public List<Comment> getComments(int matchId)
        {
            return _context.Comments
                .Include(c => c.User)
                .Where(c => c.MatchId == matchId)
                .ToList();
        }
        public void removeComment(int commentId, UserDto userDto)
        {
            if (commentId < 0) { return; }
            if (userDto == null) { return; }
            var commentEntity = _context.Comments
                .Where(c => c.Id == commentId)
                .FirstOrDefault();
            if (commentEntity == null) return;

            if (commentEntity.UserId == userDto.Id || userDto.Admin == true)
            {
                _context.Comments.Remove(commentEntity);
            }
        }
        public void simulate(int matchId)
        {
            Random random = new Random();

            if (matchId < 0) { return; }
            var matchToUpdate = SearchById(matchId);
            if (matchToUpdate == null) return;
            if (matchToUpdate.Status == true) return;
            matchToUpdate.HostScore = random.Next(0, 5);
            matchToUpdate.VisitScore = random.Next(0, 5);
            matchToUpdate.Status = true;
            AddMatchResult(matchToUpdate.SeasonId, matchToUpdate.HostId, matchToUpdate.HostScore, matchToUpdate.VisitScore);
            AddMatchResult(matchToUpdate.SeasonId, matchToUpdate.VisitId, matchToUpdate.VisitScore, matchToUpdate.HostScore);
        }
    }
}
