using FiiPracticFootball.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball.Repositories.Dtos
{
    public class ClubDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public int CountryId { get; set; }
        public Country? Country { get; set; }
    }
}
