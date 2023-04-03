using FiiPracticFootball.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Repositories.Dtos
{
    public class SeasonDto
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public LeagueDto? League { get; set; }
        public string Edition { get; set; }
        public string LeagueName { get; set; }

        public SeasonDto(int LeagueId, string Edition)
        {
            this.LeagueId = LeagueId;
            this.Edition = Edition;
        }

        public SeasonDto(Season s)
        {
            LeagueId = s.LeagueId;
            Edition = s.Edition;
            //League = new LeagueDto(s.League);
        }
        public SeasonDto() { }
    }
}
