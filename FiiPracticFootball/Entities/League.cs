using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class League
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        public IList<Season> Seasons { get; set; }
    }
}
