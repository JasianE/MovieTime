using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.FriendRequest
{
    public class FriendRequestDTO
    {
        public int Id { get; set; }
        [Required]
        public string SenderUserName { get; set; }
        [Required]
        public string ReceiverUserName { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}