using FiiPracticFootball.Entities;

namespace FiiPractivFootball_Web.Models.Club
{
    public class ClubViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryFlag { get; set; }
    }
}
