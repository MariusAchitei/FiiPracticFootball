using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class SeasonStats
    {
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season Season { get; set; }
        public int ClubId { get; set; }
        [ForeignKey("ClubId")]
        public Club Club { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Tie { get; set; }
        public int Scored { get; set; }
        public int Conceded { get; set; }

    }
}
