using FiiPracticFootball.Repositories.Interfaces;
using FiiPracticFootball;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FiiPracticFootball.Repositories.Dtos;
using Microsoft.CodeAnalysis.Differencing;
using FiiPractivFootball_Web.Models.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using FiiPracticFootball.Repositories;
using FiiPractivFootball_Web.Models.Comment;

namespace FiiPractivFootball_Web.Controllers
{
    public class MatchController : Controller
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IFootballUnitOfWork _footballUnitOfWork;
        private readonly IMatchRepository _matchRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUserRepository _userRepository;

        public MatchController(ISeasonRepository seasonRepository,
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
        // GET: MatchController
        public ActionResult Index()
        {
            //var MatchesDtos = _matchRepository.GetAll();
            return View();
        }

        // GET: MatchController/Details/5
        public ActionResult Details(int id)
        {
            var userDto = _userRepository.GetUserByEmail(User.Identity.Name);
            ViewBag.user = userDto;
            var model = _matchRepository.SearchById(id);
            ViewBag.Comments = _matchRepository.getComments(id);
            return View(model);
        }


        // GET: MatchController/Create
        [Authorize(Roles = "Admin")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: MatchController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(IFormCollection collection)
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


        // GET: MatchController/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id)
        {
            var matchDto = _matchRepository.SearchById(id);
            var editMatchData = new MatchEditModel
            {
                Id = id,
                Date = matchDto.Date,
                Stadium = matchDto.Stadium,
                HostScore = matchDto.HostScore,
                VisitScore = matchDto.VisitScore,
                Round = matchDto.Round
            };
            return View(editMatchData);
        }

        // POST: MatchController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit([FromForm] MatchEditModel editMatchData)
        {
            try
            {
                if (editMatchData == null)
                {
                    return RedirectToAction("Error", new { message = "UpdateUserViewModel is null!" });
                }

                if (!ModelState.IsValid)
                {
                    return View(editMatchData);
                }
                int id = editMatchData.Id;
                var matchDto = _matchRepository.SearchById(id);
                if(matchDto == null)
                    return RedirectToAction("Error", new { message = "User not found!" });

                matchDto.Date = editMatchData?.Date;
                matchDto.Stadium = editMatchData?.Stadium;
                matchDto.HostScore = editMatchData?.HostScore;
                matchDto.VisitScore = editMatchData?.VisitScore;
                matchDto.Round = editMatchData.Round;
                matchDto.Id = id;

                _matchRepository.updateMatch(matchDto);
                _footballUnitOfWork.SaveChanges();
                //String redirect = "Details" + id.ToString();

                return RedirectToAction("Details", new { id });
            }
            catch
            {
                return View(editMatchData);
            }
        }

        // GET: MatchController/Delete/5
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MatchController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id, IFormCollection collection)
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

        [HttpGet]
        public ActionResult AddComment([FromRoute] int id) {
            ViewBag.matchId=id;
            return View();
        }
        [HttpPost]
        public ActionResult AddComment([FromForm] CommentCreate newComment)
        {
            try
            {
                var userDto = _userRepository.GetUserByEmail(User.Identity.Name);
                _matchRepository.addComment(userDto.Id, newComment.MatchId, newComment.Message);
                _footballUnitOfWork.SaveChanges();
                return RedirectToAction("Details", "Match", new {id=newComment.MatchId});
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult DeleteComment([FromQuery] int commentId, [FromQuery] int matchId)
        {
            var userDto = _userRepository.GetUserByEmail(User.Identity.Name);

            _matchRepository.removeComment(commentId, userDto);
            _footballUnitOfWork.SaveChanges();
            return RedirectToAction("Details", "match", new {id = matchId});
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Simulate([FromQuery] int matchId)
        {
            _matchRepository.simulate(matchId);
            _footballUnitOfWork.SaveChanges();
            return RedirectToAction("Details", "match", new { id = matchId });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult SimulateAll([FromQuery] int seasonId)
        {
            var matches = _matchRepository.searchBySeason(seasonId);
            foreach (var match in matches)
                if(match.Status==false)
                    _matchRepository.simulate(match.Id); 
            _footballUnitOfWork.SaveChanges();
            return RedirectToAction("ShowMatches", "Season", new { id = seasonId });
        }
    }
}
