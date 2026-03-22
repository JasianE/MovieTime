using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Enums;

namespace api.DTOs.UserMovie
{
    public class UpdateRecommendationDTO
    {
        public int? RecipientRating { get; set; }
        public string? RecipientNotes { get; set; }
        public MovieStatus? Status { get; set; }
    }
}