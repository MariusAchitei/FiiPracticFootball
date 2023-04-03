using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public string? LeagueName = "";
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season Season { get; set; }
        public int HostId { get; set; }
        [ForeignKey("HostId")]
        public Club Host { get; set; }
        public int VisitId { get; set; }
        [ForeignKey("VisitId")]
        public Club Visit { get; set; }
        public DateTime? Date { get; set; }
        public string? Stadium { get; set; }
        public int? HostScore { get; set; }
        public int? VisitScore { get; set; }
        public bool? Status { get; set; }
        public int Round { get; set; }

        public IList<Player> Players { get; set; }
        public List<Comment> Comments { get; set; }


    }
}
