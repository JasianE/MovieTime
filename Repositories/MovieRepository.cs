using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using api.Repositories.Helpers;
using api.Helpers;

namespace api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDBContext _context;
        public MovieRepository(ApplicationDBContext context)
        {
            _context = context; // abstract and allow dependency injection to do its thang
        }
        public async Task<Movie?> AddMovieToDB(string movieName)
        {
            Movie? CheckIfExists = await _context.Movies.FirstOrDefaultAsync(item => item.Title == movieName); // don't allow duplicate movies, but this is a race condition (currently if i spam frontend it don't work)
            if (CheckIfExists == null)
            {
                //Create request from TMDB
                var options = new RestClientOptions($"https://api.themoviedb.org/3/search/movie?query={movieName}&include_adult=true&language=en-US&page=1");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhMzE1YjkzNDQzZDFlNzEwYTYzMDRiYzY5NTRkZjcwNyIsIm5iZiI6MTc1NDIzNDAwNC4zNTEsInN1YiI6IjY4OGY3Yzk0MmRhNWFlZjY1ZmI3ZjMzZSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.m1vQKqndjZC-0_uaxkHTzVdfbDodjmRv0Q5k5EnTFNY");
                var response = await client.GetAsync(request);
                MovieThing theMovie; // define movie outside of scope for return

                if (response.IsSuccessful && response.Content is not null)
                {
                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var result = JsonSerializer.Deserialize<MovieResponse>(response.Content, jsonOptions);
                    /*foreach (var movie in result.Results)
                    {
                        //Add a new feature to have the user select which one it is that they want, but for now
                        
                    }*/
                    //Deserializes movie and adds to DB
                    theMovie = result.Results[0];
                    Movie newMovie = new Movie
                    {
                        Title = theMovie.Original_Title,
                        OverView = theMovie.Overview,
                        PosterPath = theMovie.Poster_Path,
                        Runtime = "609",
                    };
                    try
                    {
                        await _context.Movies.AddAsync(newMovie);
                        await _context.SaveChangesAsync();
                        return newMovie;
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine("Hey im here!");
                        return null; //throw in an error that says already exists
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public Task<Movie> AddMovieToSeperateUser()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Movie>> GetAll(QueryObject query)
        {
            var movies = _context.Movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                movies = movies.Where(movie => movie.Title.Contains(query.Title));
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await movies.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public Task<Movie> GetMovie()
        {
            throw new NotImplementedException();
        }

        public async Task<Movie> GetMovieByName(string title)
        {
            return await _context.Movies.FirstOrDefaultAsync(item => item.Title == title);
        }
    }
}