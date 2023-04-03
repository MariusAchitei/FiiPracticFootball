using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiiPracticFootball.Entities;

namespace FiiPracticFootball.Repositories.Dtos
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public CountryDto(Country country) {
            Id = country.Id;
            Name = country.Name;
            Flag = country.Flag;
        }
    }
}
