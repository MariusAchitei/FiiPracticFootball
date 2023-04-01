using FiiPracticFootball;
using FiiPracticFootball.Repositories;
using FiiPracticFootball.Repositories.Dtos;
using FiiPracticFootball.Repositories.Interfaces;
using FiiPracticFootball.Web.Models.Account;
using FiiPracticFootball.Web.Models.Users;
using FIIPracticFootball.Services;
using FiiPractivFootball_Web.Models.Club;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace FiiPracticFootball.Web.Controllers
{
    public class AccountController : Controller
  {
    private readonly ICryptographyService _cryptographyService;
    private readonly IUserRepository _userRepository;
    private readonly IFootballUnitOfWork _footballUnitOfWork;
        private readonly IClubRepository _clubRepository;

    public AccountController(
      ICryptographyService cryptographyService
      , IUserRepository userRepository
      , IFootballUnitOfWork footballUnitOfWork
        ,IClubRepository clubRepository)
    {
      _cryptographyService = cryptographyService;
      _userRepository = userRepository;
      _footballUnitOfWork = footballUnitOfWork;
            _clubRepository = clubRepository;
    }
    public IActionResult Index()
    {
        var userDto = _userRepository.GetUserByEmail(User.Identity.Name);
        var ClubsDto = _clubRepository.getUserFavorites(userDto.Id);
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
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }
      if (HttpContext.User.Identity?.IsAuthenticated == true)
      {
        return RedirectToAction("Index");
      }
      var user = _userRepository.GetUserByEmail(model.Email);
      if(user == null)
      {
        Response.StatusCode = (int) HttpStatusCode.NotFound;
        ViewBag.ErrorMessage = "Could not find user";
        return View();
      }

      var hash = _cryptographyService.HashPassword(model.Password, user.PasswordSalt);
      if(user.PasswordHash != hash.Hash)
      {
        Response.StatusCode = (int)HttpStatusCode.NotFound;
        ViewBag.ErrorMessage = "Could not find user";
        return View();
      }

      await SignInAsync(model.Email, user.Admin);

      return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
      if (HttpContext.User.Identity?.IsAuthenticated == false)
      {
        return RedirectToAction(nameof(Index));
      }
      await HttpContext!.SignOutAsync();
      return RedirectToAction(nameof(Login));
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(CreateUserViewModel registerModel)
    {
      var hash = _cryptographyService.HashPasswordWithSaltGeneration(registerModel.Password);
      _userRepository.CreateUser(new UserDto
      {
        FirstName = registerModel.FirstName,
        LastName = registerModel.LastName,
        Email = registerModel.Email,
        PasswordHash = hash.Hash,
        PasswordSalt = hash.Salt
      });
      _footballUnitOfWork.SaveChanges();

      await SignInAsync(registerModel.Email, false);
      return RedirectToAction(nameof(Index));
    }

    private async Task SignInAsync(string email, bool isAdmin)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
        };
        var identity = new ClaimsIdentity(claims, AuthFootballConstants.UserSchema);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);
}
    }
}
