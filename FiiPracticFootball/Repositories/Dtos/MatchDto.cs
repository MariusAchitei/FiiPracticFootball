using FiiPracticFootball.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Repositories.Dtos
{
    public class MatchDto
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int HostId { get; set; }
        public int VisitId { get; set; }
        public DateTime? Date { get; set; }
        public string? Stadium { get; set; }
        public int? HostScore { get; set; }
        public int? VisitScore { get; set; }
        public bool? Status { get; set; }
        public int Round { get; set; }

        public MatchDto(int SeasonId,
            int HostId, int VisitId, DateTime? Date,
            int? HostScore, int? VisitScore) {

            this.HostId = HostId;
            this.VisitId = VisitId;
            this.SeasonId = SeasonId;
            this.Date = Date;
            this.HostScore = HostScore;
            this.VisitScore = VisitScore;
            this.Round = 0;
        }
        public MatchDto(int SeasonId, int HostId, int VisitId, int Round, DateTime Date)
        {
            this.HostId = HostId;
            this.VisitId = VisitId;
            this.SeasonId = SeasonId;
            this.Date = Date;
            this.Round = Round;
        }
        public MatchDto(int SeasonId, int HostId, int VisitId, int Round)
        {
            this.HostId = HostId;
            this.VisitId = VisitId;
            this.SeasonId = SeasonId;
            this.Date = DateTime.Now;
            this.Round = Round;
        }
    }
}
