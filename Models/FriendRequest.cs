using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Enums;

namespace api.Models
{
    [Table("FriendRequest")]
    public class FriendRequest
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}