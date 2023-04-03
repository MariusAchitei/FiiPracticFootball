using FiiPracticFootball.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Repositories.Dtos
{
    public class LeagueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public CountryDto Country { get; set; }  
        public IList<Season> Seasons { get; set; }
        public LeagueDto(League l) {
            Id = l.Id;
            Name = l.Name;
            CountryId = l.CountryId;
            Country = new CountryDto(l.Country);
        }
    }
}
