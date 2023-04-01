////using System.Text.RegularExpressions;
//using FiiPracticFootball.Repositories.Dtos;
//using FiiPracticFootball.Entities;

//namespace FiiPractivFootball_Web.Models.Season
//{
//    public class SeasonDetailsView
//    {
//        //private SeasonDto? seasonDto;
//        public List<Match> Matches;

//        public int Id { get; set; }
//        public int LeagueId { get; set; }
//        public string Edition { get; set; }
//        //public List<Match> Matches  {get; set;}
//        public string LeagueName { get; set; }
//        public SeasonDetailsView(SeasonDto season, List<Match> matches) {
//            Id = season.Id;
//            LeagueName = season.LeagueName;
//            LeagueId = season.LeagueId;
//            Edition = season.Edition;
//            Matches = matches;
//        }

//        //public SeasonDetailsView(SeasonDto? seasonDto, List<FiiPracticFootball.Entities.Match> matches)
//        //{
//        //}

//        //public SeasonDetailsView(SeasonDto? seasonDto, List<FiiPracticFootball.Entities.Match> matches)
//        //{
//        //    this.seasonDto = seasonDto;
//        //    this.matches = matches;
//        //}
//    }
//}