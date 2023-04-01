using FiiPracticFootball.Repositories.Dtos;

namespace FiiPractivFootball_Web.Models.Season
{
    public class SeasonModelView
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public string CountryName { get; set; }
        public string Flag { get; set; }
        public string Edition { get; set; }
        public List<MatchDto> Matches { get; set; }
    }
}
