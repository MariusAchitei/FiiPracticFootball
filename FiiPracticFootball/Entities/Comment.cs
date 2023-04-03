using FIIPracticCars.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int MatchId { get; set; }
        [ForeignKey("UserId")]
        public Match Match { get; set; }    

        public DateTime Date{ get; set; }
    }
}
