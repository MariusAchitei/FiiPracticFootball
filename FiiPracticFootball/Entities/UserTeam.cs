using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Entities
{
    public class UserTeam
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Club Club { get; set; }
    }
}
