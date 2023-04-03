using FiiPracticFootball.Entities;
using FiiPracticFootball.Repositories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Repositories.Interfaces
{
    public interface ISeasonRepository
    {
        int CreateSeason(SeasonDto seasonDto, List<ClubDto> clubs);
        SeasonDto? GetSeason(int seasonId);
        public List<SeasonDto> GetAll();

        public List<SeasonStats> getSeasonStats(int seasonId);
        //public List<SeasonStats> getClubStats(int seasonId);


        public void Delete(int seasonId);
        //void DeleteSeason(int matchId);
    }
}
