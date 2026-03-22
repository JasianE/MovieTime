using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Enums;

namespace api.Models
{
    [Table("UserMovie")]
    public class UserMovie
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } //for some reason dotnet id be a string by defualt
        public int MovieId { get; set; }
        public MovieStatus Status { get; set; } = MovieStatus.ToWatch;
        public string? RecommendedByUserId { get; set; }
        public string? Reason { get; set; }
        public int? RecipientRating { get; set; }
        public string? RecipientNotes { get; set; }

        //Nav properties
        public Movie Movie { get; set; }
        public AppUser AppUser { get; set; }
        public AppUser? RecommendedBy { get; set; }

    }
}