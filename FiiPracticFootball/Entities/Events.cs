using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class Events
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Type { get; set; }
        public int Time { get; set; }
    }
}
