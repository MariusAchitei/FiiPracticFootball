using FiiPracticFootball;
using FiiPractivFootball_Web.Models.Season;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FiiPracticFootball.Entities;
using FiiPracticFootball.Repositories.Interfaces;
using FiiPracticFootball.Repositories.Dtos;
using Microsoft.AspNetCore.Authorization;
using FiiPracticFootball.Repositories;

namespace FiiPractivFootball_Web.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IFootballUnitOfWork _footballUnitOfWork;
        private readonly IMatchRepository _matchRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUserRepository _userRepository;

        public SeasonController(ISeasonRepository seasonRepository,
            IMatchRepository matchRepository,
            IFootballUnitOfWork footballUnitOfWork,
            ILeagueRepository leagueRepository,
            IClubRepository clubRepository,
            IUserRepository userRepository)
        {
            _seasonRepository = seasonRepository;
            _footballUnitOfWork = footballUnitOfWork;
            _matchRepository = matchRepository;
            _leagueRepository = leagueRepository;
            _clubRepository = clubRepository;
            _userRepository = userRepository;
        }


        // GET: SeasonController
        public ActionResult Index()
        {
            var seasonDtos = _seasonRepository.GetAll();
            return View(seasonDtos);
        }

        // GET: SeasonController/Details/5
        public ActionResult Details([FromRoute] int Id)
        {
            if (Id < 1)
            {
                return RedirectToAction("Error", new { message = "Season Id is negative!" });
            }
            var userDto = _userRepository.GetUserByEmail(User.Identity.Name);
            ViewBag.user = userDto;
            var seasonDto = _seasonRepository.GetSeason(Id);
            var leagueDto = _leagueRepository.getLeague(seasonDto.LeagueId);
            
            //var seasonDetailsView = new SeasonDetailsView(seasonDto);
            SeasonModelView model = new SeasonModelView()
            {
                Id = Id,
                LeagueId = leagueDto.Id,
                LeagueName = leagueDto.Name,
                CountryName = leagueDto.Country.Name,
                Flag = leagueDto.Country.Flag,
                Edition = seasonDto.Edition,
            };

            if (seasonDto == null)
            {
                return RedirectToAction("Error", new { message = "Season not found!" });
            }
            return View(model);
        }
        public ActionResult ShowMatches(int id)
        {
            var userDto = _userRepository.GetUserByEmail(User.Identity.Name);
            ViewBag.user = userDto;
            var matches = _matchRepository.searchBySeason(id);
            ViewBag.matches = matches;
            return View();
        }

        public ActionResult ShowTable(int id)
        {
            ViewBag.SeasonStats = _seasonRepository.getSeasonStats(id);
            return View();
        }

        // GET: SeasonController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
 
            var leagues = _leagueRepository.getAllLeagues();
            var clubs = _clubRepository.GetAll();
            ViewBag.leagues = leagues;
            ViewBag.clubs = clubs;
            return View();
        }

        // POST: SeasonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Create([FromForm] SeasonCreate newSeasonData)
        {
            try
            {
                var league = _leagueRepository.getLeague(newSeasonData.LeagueId);
                SeasonDto season= new SeasonDto(newSeasonData.LeagueId, newSeasonData.Edition);
                _seasonRepository.CreateSeason(season, new List<ClubDto>(_clubRepository.getClubsByCountry(league.CountryId)));
                _footballUnitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SeasonController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SeasonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SeasonController/Delete/
        [Authorize(Roles = "Admin")]
        // 
        public ActionResult Delete(int id)
        {
            if (id < 1)
            {
                return RedirectToAction("Error", new { message = "season Id is negative!" });
            }

            var seasonDto = _seasonRepository.GetSeason(id);
            if (seasonDto == null)
            {
                return RedirectToAction("Error", new { message = "season not found not found!" });
            }
            var leagueDto = _leagueRepository.getLeague(seasonDto.LeagueId);
            SeasonModelView model = new SeasonModelView() {
                Id = id,
                LeagueId = leagueDto.Id,
                LeagueName = leagueDto.Name,
                CountryName = leagueDto.Country.Name,
                Flag = leagueDto.Country.Flag,
                Edition = seasonDto.Edition
            };

            return View(model);
        }

        // POST: SeasonController/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _seasonRepository.Delete(id);
                _footballUnitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
