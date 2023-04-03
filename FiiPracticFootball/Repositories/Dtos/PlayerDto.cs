using FiiPracticFootball.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Repositories.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Value { get; set; }
        public string? Photo { get; set; }
        public int Number { get; set; }

        public int Rating { get; set; }

        public int ClubId { get; set; }

        public Club? Club { get; set; }
        public IList<Match> Matches { get; set; }
    }
}
