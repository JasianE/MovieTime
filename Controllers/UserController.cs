
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Movie;
using api.DTOs.User;
using api.Interfaces;
using api.Models;
using api.Repositories;
using api.Data;
using api.Models.Enums;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserMovieRepository _userMovieRepo;
        private readonly ApplicationDBContext _context;
        public UserController(UserManager<AppUser> userManager, IUserMovieRepository userMovieRepo, ApplicationDBContext context)
        {
            _userManager = userManager;
            _userMovieRepo = userMovieRepo;
            _context = context;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            //Add pagination

            List<AppUser> users = await _userManager.Users.ToListAsync();
            var DTOs = users.Select(user => new UserDTO
            {
                UserName = user.UserName,
                ID = user.Id
            });

            return Ok(DTOs);
        }

        [HttpGet("username")]
        [Authorize]

        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            AppUser? appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null)
            {
                return NotFound("User does not exist");
            }
            else
            {
                UserDTO appUserDTO = new UserDTO
                {
                    UserName = appUser.UserName,
                    ID = appUser.Id
                };
                return Ok(appUserDTO);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByID(string id)
        { //only once you get the id can you use this, which will also return the movies the user has, for sepearte page (maybe convoluted)
            AppUser? appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return NotFound("User does not exist");
            }
            string currentUserName = User.GetUserName();
            AppUser currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser.Id != appUser.Id)
            {
                bool areFriends = await _context.FriendRequests.AnyAsync(request =>
                    request.Status == FriendRequestStatus.Accepted &&
                    ((request.SenderId == currentUser.Id && request.ReceiverId == appUser.Id) ||
                     (request.SenderId == appUser.Id && request.ReceiverId == currentUser.Id)));

                if (!areFriends)
                {
                    return StatusCode(403, "You can only view friends.");
                }
            }

            List<UserMovieMovie> moviesOfUser = await _userMovieRepo.GetUserMovies(appUser);
            UserWithUserMovies appUserDTO = new UserWithUserMovies
            {
                UserName = appUser.UserName,
                ID = appUser.Id,
                UserMovies = moviesOfUser
            };
            return Ok(appUserDTO);
        }
    }
}