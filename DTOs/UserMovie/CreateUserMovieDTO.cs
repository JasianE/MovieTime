using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.UserMovie
{
    public class CreateUserMovieDTO
    {
        public string MovieName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Reason { get; set; }
    }
}