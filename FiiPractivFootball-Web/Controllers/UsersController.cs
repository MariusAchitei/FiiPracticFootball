using FiiPracticFootball.Repositories;
using FiiPracticFootball.Repositories.Dtos;
using FiiPracticFootball;
using FiiPractivFootball_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FiiPracticFootball.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;

namespace FIIPracticFootball.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IFootballUnitOfWork _footballUnitOfWork;

        public UsersController(IUserRepository userRepository, IFootballUnitOfWork carsUnitOfWork)
        {
            _userRepository = userRepository;
            _footballUnitOfWork = carsUnitOfWork;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var userDtos = _userRepository.GetAll();

            var userViewModels = userDtos.Select(u => new UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
            });

            return View(userViewModels);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromForm] CreateUserViewModel createUserViewModel)
        {
            if (createUserViewModel == null)
            {
                return RedirectToAction("Error", new { message = "CreateUserViewModel is null!" });
            }

            if (!ModelState.IsValid)
            {
                return View(createUserViewModel);
            }

            var userDto = new UserDto
            {
                FirstName = createUserViewModel.FirstName,
                LastName = createUserViewModel.LastName,
                Email = createUserViewModel.Email,
                PasswordHash = createUserViewModel.Password
            };

            _userRepository.CreateUser(userDto);
            _footballUnitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit([FromRoute] int id)
        {
            if (id < 1)
            {
                return RedirectToAction("Error", new { message = "User Id is negative!" });
            }

            var userDto = _userRepository.GetUser(id);
            if (userDto == null)
            {
                return RedirectToAction("Error", new { message = "User not found!" });
            }

            var updateUserViewModel = new UpdateUserViewModel
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
            };

            return View(updateUserViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit([FromForm] UpdateUserViewModel updateUserViewModel)
        {
            if (updateUserViewModel == null)
            {
                return RedirectToAction("Error", new { message = "UpdateUserViewModel is null!" });
            }

            if (!ModelState.IsValid)
            {
                return View(updateUserViewModel);
            }

            var userDto = _userRepository.GetUser(updateUserViewModel.Id);
            if (userDto == null)
            {
                return RedirectToAction("Error", new { message = "User not found!" });
            }

            userDto.FirstName = updateUserViewModel.FirstName;
            userDto.LastName = updateUserViewModel.LastName;

            _userRepository.UpdateUser(userDto);
            _footballUnitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (id < 1)
            {
                return RedirectToAction("Error", new { message = "User Id is negative!" });
            }

            var userDto = _userRepository.GetUser(id);
            if (userDto == null)
            {
                return RedirectToAction("Error", new { message = "User not found!" });
            }

            var userViewModel = new UserViewModel
            {
                Id = id,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
            };

            return View(userViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed([FromRoute] int id)
        {
            if (id < 1)
            {
                return RedirectToAction("Error", new { message = "User Id is negative!" });
            }

            _userRepository.DeleteUser(id);
            _footballUnitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Details([FromRoute] int id)
        {
            if (id < 1)
            {
                return RedirectToAction("Error", new { message = "User Id is negative!" });
            }

            var userDto = _userRepository.GetUser(id);
            if (userDto == null)
            {
                return RedirectToAction("Error", new { message = "User not found!" });
            }

            var userViewModel = new UserViewModel
            {
                Id = id,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
            };

            return View(userViewModel);
        }

        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            });
        }
    }
}
