namespace FiiPractivFootball_Web.Models.Match
{
    public class MatchEditModel
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Stadium { get; set; }
        public int? HostScore { get; set; }
        public int? VisitScore { get; set; }
        public int Round { get; set; }

    }
}
