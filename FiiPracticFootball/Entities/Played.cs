using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class Played
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Status { get; set; }
    }
}
