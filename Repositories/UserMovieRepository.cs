using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Movie;
using api.DTOs.User;
using api.DTOs.UserMovie;
using api.Interfaces;
using api.Models;
using api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class UserMovieRepository : IUserMovieRepository
    {
        private readonly ApplicationDBContext _context;
        public UserMovieRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<string> AddUserMovie(UserMovie usermovie)
        {
            await _context.UserMovies.AddAsync(usermovie);
            await _context.SaveChangesAsync();
            return "Sucessful";
        }

        public async Task<UpdateUserMovieDTO> ChangeMovieStatus(AppUser user, UserMovieMovie movie, int status)
        {
            var userMovieInstance = await _context.UserMovies.FirstOrDefaultAsync(item => item.AppUserId == user.Id && item.MovieId == movie.Id);
            var userMoves = await _context.UserMovies.ToListAsync();
            
            if (userMovieInstance == null)
            {
                return null;
            }

            userMovieInstance.Status = MovieStatus.Watched;
            await _context.SaveChangesAsync();

            return new UpdateUserMovieDTO
            {
                AppUserId = userMovieInstance.AppUserId,
                MovieId = userMovieInstance.MovieId,
                Status = userMovieInstance.Status
            };
        }

        public async Task<List<UserMovieMovie>> GetUserMovies(AppUser user)
        {
            return await _context.UserMovies
            .Where(u => u.AppUserId == user.Id)
            .Select(userMovie => new UserMovieMovie
            {
                RecommendationId = userMovie.Id,
                Title = userMovie.Movie.Title,
                OverView = userMovie.Movie.OverView,
                PosterPath = userMovie.Movie.PosterPath,
                Runtime = userMovie.Movie.Runtime,
                Id = userMovie.Movie.Id, // bruh this was excldued before,
                Status = userMovie.Status,
                RecommendedByUserName = userMovie.RecommendedBy != null ? userMovie.RecommendedBy.UserName : "Unknown",
                Reason = userMovie.Reason,
                RecipientRating = userMovie.RecipientRating,
                RecipientNotes = userMovie.RecipientNotes

            }).ToListAsync();
        }
    }
}