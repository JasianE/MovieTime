using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Movie;
using api.DTOs.UserMovie;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;
namespace api.Controllers
{
    [Route("api/usermovie")]
    [ApiController]
    public class UserMovieController : ControllerBase // because of controller base we have a user object w/ 3 props
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMovieRepository _movieRepo;
        private readonly IUserMovieRepository _userMovieRepo;

        public UserMovieController(UserManager<AppUser> userManager, IMovieRepository movieRepo, IUserMovieRepository userMovieRepo)
        {
            _userManager = userManager;
            _movieRepo = movieRepo;
            _userMovieRepo = userMovieRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserMovies()
        {
            string username = User.GetUserName();
            AppUser appUser = await _userManager.FindByNameAsync(username);
            var movies = await _userMovieRepo.GetUserMovies(appUser);
            return Ok(movies);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserMovie([FromBody] CreateUserMovieDTO movieDto) //name from other user
        {
            //Add in a recommended by in this field
            AppUser? appUser = await _userManager.FindByNameAsync(movieDto.UserName);
            Movie? movie = await _movieRepo.GetMovieByName(movieDto.MovieName);
            if (movie == null)
            {
                Movie? newMovie = await _movieRepo.AddMovieToDB(movieDto.MovieName);
                if (newMovie == null)
                {
                    return BadRequest("Movie does not exist in DB.");
                }
                else
                {
                    movie = newMovie;
                }
            }
            if (appUser == null)
            {
                return BadRequest("User does not exist.");
            }

            var userMovies = await _userMovieRepo.GetUserMovies(appUser);

            if (userMovies.Any(e => e.Title.ToLower() == movieDto.MovieName.ToLower()))
            {
                return BadRequest("User already has this movie added.");
            }

            UserMovie newUserMovie = new UserMovie
            {
                AppUserId = appUser.Id,
                MovieId = movie.Id
            };

            string result = await _userMovieRepo.AddUserMovie(newUserMovie);

            if (newUserMovie == null)
            {
                return BadRequest("Could not be created");
            }
            else
            {
                return Ok(new { message = result });
            }
        }
        [HttpPut("update/{MovieTitle}")]
        [Authorize]

        public async Task<IActionResult> ChangeMovieStatus([FromRoute] string MovieTitle)
        {
            string userName = User.GetUserName(); // we get the username from the user object that exists due to controller base logged in
            AppUser? appUser = await _userManager.FindByNameAsync(userName);

            //Query through all of the appUsers usermovies, and find the one with the same movie title
            //if null return not found, if found then update it and call the movie repo
            var userMovies = await _userMovieRepo.GetUserMovies(appUser);
            UserMovieMovie? itemOfInterest = userMovies.FirstOrDefault(item => item.Title.ToLower() == MovieTitle.ToLower());

            if (itemOfInterest == null)
            {
                return BadRequest("Movie not found.");
            }
            else
            {
                var result = await _userMovieRepo.ChangeMovieStatus(appUser, itemOfInterest, 1);
                return Ok(result);
            }
        }
    }
}