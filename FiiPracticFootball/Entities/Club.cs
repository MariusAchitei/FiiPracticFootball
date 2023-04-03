using FIIPracticCars.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class Club
    {
        public int Id { get; set; }

        public int? Value = 0; 
        public string Name { get; set; }
        public string Logo { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
        public IList<Player> Players { get; set; }
        public List<User> Users { get; set; }
    }
}
