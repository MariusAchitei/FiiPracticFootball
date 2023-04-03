using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiiPracticFootball.Entities;
using FiiPracticFootball.Repositories.Dtos;

namespace FiiPracticFootball.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        int CreateMatch(MatchDto matchDto, List<Player> players);
        Match? SearchById(int matchId);
        List<Match> searchBySeason(int seasonId);
        void DeleteMatch(int matchId);

        void updateMatch(Match matchDto);
        void AddMatchResult(int seasonId, int teamId, int? teamScore, int? opponentScore);
        void addComment(int userId, int matchId, string message);
        List<Comment> getComments(int matchId);
        public void removeComment(int commentId, UserDto userDto);
        public void simulate(int matchId);

    }
}
