using FIIPracticCars.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiiPractivFootball_Web.Models.Comment
{
    public class CommentCreate
    {
        public string Message { get; set; }
        public int MatchId { get; set; }
        //public int MatchId { get; set; }

        //public DateTime Date { get; set; }

    }
}
