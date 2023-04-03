using FiiPracticFootball.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiiPracticFootball.Repositories.Dtos;

namespace FiiPracticFootball.Repositories.Interfaces
{
    public interface ILeagueRepository
    {
        List<LeagueDto> getAllLeagues();
        LeagueDto? getLeague(int leagueId);
    }
}
