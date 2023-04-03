using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class Season
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        [ForeignKey("LeagueId")]
        public League League { get; set; }
        public string Edition { get; set; }
        public IList<Match> Matches { get; set; }
    }
}
