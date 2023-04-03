using FiiPracticFootball.Entities;
using FiiPracticFootball.Repositories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Repositories.Interfaces
{
    public interface IClubRepository
    {
        public List<ClubDto> GetAll();
        public List<ClubDto> getClubsByCountry(int countryId);
        public List<SeasonStats> getClubStats(int clubId);

        public ClubDto getClub(int clubId);
        public List<ClubDto> getSupportedClubs(int userId);
        public void addToFavorte(int userId, int clubId);
        public List<ClubDto> getUserFavorites(int userId);
        public void removeFavorite(int userId, int clubId);
    }
}
