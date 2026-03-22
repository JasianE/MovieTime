using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Enums;

namespace api.DTOs.UserMovie
{
    public class RecommendationDetailDTO
    {
        public int RecommendationId { get; set; }
        public string Title { get; set; }
        public string OverView { get; set; }
        public string PosterPath { get; set; }
        public string Runtime { get; set; }
        public MovieStatus Status { get; set; }
        public string RecommendedByUserName { get; set; }
        public string? Reason { get; set; }
        public int? RecipientRating { get; set; }
        public string? RecipientNotes { get; set; }
    }
}