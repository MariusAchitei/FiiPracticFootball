using FiiPracticFootball.Repositories.Interfaces;
using FiiPracticFootball;
using Microsoft.AspNetCore.Mvc;
using FiiPractivFootball_Web.Models.Club;
using FiiPracticFootball.Repositories.Implementations;
using FiiPracticFootball.Repositories;

namespace FiiPractivFootball_Web.Controllers
{
    public class ClubController : Controller
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IFootballUnitOfWork _footballUnitOfWork;
        private readonly IMatchRepository _matchRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;

        public ClubController(ISeasonRepository seasonRepository, IMatchRepository matchRepository, IFootballUnitOfWork footballUnitOfWork, ILeagueRepository leagueRepository, IClubRepository clubRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
        {
            _seasonRepository = seasonRepository;
            _footballUnitOfWork = footballUnitOfWork;
            _matchRepository = matchRepository;
            _leagueRepository = leagueRepository;
            _clubRepository = clubRepository;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            var ClubsDto = _clubRepository.GetAll();
            var ClubsViewModel = ClubsDto.Select(c => new ClubViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Logo = c.Logo,
                CountryId = c.CountryId,
                CountryName = c.Country.Name,
                CountryFlag = c.Country.Flag
            });
            return View(ClubsViewModel);
        }
        public ActionResult Details([FromRoute] int id)
        {
            var clubDto = _clubRepository.getClub(id);
            var ClubsViewModel = new ClubViewModel()
            {
                Id = clubDto.Id,
                Name = clubDto.Name,
                Logo = clubDto.Logo,
                CountryId = clubDto.CountryId,
                CountryName = clubDto.Country.Name,
                CountryFlag = clubDto.Country.Flag
            };
            ViewBag.players = _playerRepository.GetPlayersByClub(id);
            ViewBag.Stats = _clubRepository.getClubStats(id);
            return View(ClubsViewModel);
        }
        [HttpGet]
        public ActionResult AddFavorite([FromRoute] int id)
        {
            var userDto = _userRepository.GetUserByEmail(User.Identity.Name);
            _clubRepository.addToFavorte(userDto.Id, id);
            _footballUnitOfWork.SaveChanges();
            //return RedirectToAction("/Account");
            return RedirectToAction("Index", "Account");
        }
        public ActionResult RemoveFavorite([FromRoute] int id)
        {
            var userDto = _userRepository.GetUserByEmail(User.Identity.Name);
            _clubRepository.removeFavorite(userDto.Id, id);
            _footballUnitOfWork.SaveChanges();
            //return RedirectToAction("/Account");
            return RedirectToAction("Index", "Account");
        }


    }
}
