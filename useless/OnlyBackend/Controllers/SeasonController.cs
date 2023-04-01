using Microsoft.AspNetCore.Mvc;
using FiiPracticFootball;
using FiiPracticFootball.Repositories;
using FiiPractivFootball_Web.Models.Season;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FiiPracticFootball.Entities;

namespace OnlyBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeasonController : ControllerBase
    {
        private readonly ILogger<SeasonController> _logger;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IFootballUnitOfWork _footballUnitOfWork;
        private readonly IMatchRepository _matchRepository;

        public SeasonController(ISeasonRepository seasonRepository, IMatchRepository matchRepository, IFootballUnitOfWork footballUnitOfWork)
        {
            _seasonRepository = seasonRepository;
            _footballUnitOfWork = footballUnitOfWork;
            _matchRepository = matchRepository;
        }
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        //public SeasonController(ILogger<SeasonController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet(Name = "GetSeasonController")]
        public IEnumerable<SeasonController> Get()
        {
            return 
        }
    }
}