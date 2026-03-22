using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.FriendRequest;
using api.DTOs.User;
using api.Extensions;
using api.Models;
using api.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FriendController(ApplicationDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> GetFriends()
        {
            string userName = User.GetUserName();
            AppUser currentUser = await _userManager.FindByNameAsync(userName);

            var friendIds = await _context.FriendRequests
                .Where(request => request.Status == FriendRequestStatus.Accepted &&
                    (request.SenderId == currentUser.Id || request.ReceiverId == currentUser.Id))
                .Select(request => request.SenderId == currentUser.Id ? request.ReceiverId : request.SenderId)
                .Distinct()
                .ToListAsync();

            var friends = await _userManager.Users
                .Where(user => friendIds.Contains(user.Id))
                .Select(user => new UserDTO
                {
                    UserName = user.UserName,
                    ID = user.Id
                })
                .ToListAsync();

            return Ok(friends);
        }

        [HttpGet("requests/incoming")]
        [Authorize]
        public async Task<IActionResult> GetIncomingRequests()
        {
            string userName = User.GetUserName();
            AppUser currentUser = await _userManager.FindByNameAsync(userName);

            var requests = await _context.FriendRequests
                .Where(request => request.ReceiverId == currentUser.Id && request.Status == FriendRequestStatus.Pending)
                .Include(request => request.Sender)
                .Select(request => new FriendRequestDTO
                {
                    Id = request.Id,
                    SenderUserName = request.Sender.UserName,
                    ReceiverUserName = userName,
                    Status = request.Status.ToString(),
                    CreatedAt = request.CreatedAt
                })
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("requests/outgoing")]
        [Authorize]
        public async Task<IActionResult> GetOutgoingRequests()
        {
            string userName = User.GetUserName();
            AppUser currentUser = await _userManager.FindByNameAsync(userName);

            var requests = await _context.FriendRequests
                .Where(request => request.SenderId == currentUser.Id && request.Status == FriendRequestStatus.Pending)
                .Include(request => request.Receiver)
                .Select(request => new FriendRequestDTO
                {
                    Id = request.Id,
                    SenderUserName = userName,
                    ReceiverUserName = request.Receiver.UserName,
                    Status = request.Status.ToString(),
                    CreatedAt = request.CreatedAt
                })
                .ToListAsync();

            return Ok(requests);
        }

        [HttpPost("requests")]
        [Authorize]
        public async Task<IActionResult> SendFriendRequest([FromBody] CreateFriendRequestDTO requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userName = User.GetUserName();
            AppUser currentUser = await _userManager.FindByNameAsync(userName);
            AppUser receiver = await _userManager.FindByNameAsync(requestDto.UserName);

            if (receiver == null)
            {
                return NotFound("User not found.");
            }

            if (receiver.Id == currentUser.Id)
            {
                return BadRequest("You cannot send a friend request to yourself.");
            }

            bool existingRequest = await _context.FriendRequests.AnyAsync(request =>
                (request.SenderId == currentUser.Id && request.ReceiverId == receiver.Id) ||
                (request.SenderId == receiver.Id && request.ReceiverId == currentUser.Id));

            if (existingRequest)
            {
                return BadRequest("A friend request already exists between you and this user.");
            }

            var friendRequest = new FriendRequest
            {
                SenderId = currentUser.Id,
                ReceiverId = receiver.Id,
                Status = FriendRequestStatus.Pending
            };

            await _context.FriendRequests.AddAsync(friendRequest);
            await _context.SaveChangesAsync();

            return Ok(new FriendRequestDTO
            {
                Id = friendRequest.Id,
                SenderUserName = userName,
                ReceiverUserName = receiver.UserName,
                Status = friendRequest.Status.ToString(),
                CreatedAt = friendRequest.CreatedAt
            });
        }

        [HttpPut("requests/{id}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptRequest([FromRoute] int id)
        {
            string userName = User.GetUserName();
            AppUser currentUser = await _userManager.FindByNameAsync(userName);

            var request = await _context.FriendRequests
                .Include(item => item.Sender)
                .Include(item => item.Receiver)
                .FirstOrDefaultAsync(item => item.Id == id && item.ReceiverId == currentUser.Id);

            if (request == null)
            {
                return NotFound("Friend request not found.");
            }

            request.Status = FriendRequestStatus.Accepted;
            await _context.SaveChangesAsync();

            return Ok(new FriendRequestDTO
            {
                Id = request.Id,
                SenderUserName = request.Sender.UserName,
                ReceiverUserName = request.Receiver.UserName,
                Status = request.Status.ToString(),
                CreatedAt = request.CreatedAt
            });
        }

        [HttpPut("requests/{id}/decline")]
        [Authorize]
        public async Task<IActionResult> DeclineRequest([FromRoute] int id)
        {
            string userName = User.GetUserName();
            AppUser currentUser = await _userManager.FindByNameAsync(userName);

            var request = await _context.FriendRequests
                .Include(item => item.Sender)
                .Include(item => item.Receiver)
                .FirstOrDefaultAsync(item => item.Id == id && item.ReceiverId == currentUser.Id);

            if (request == null)
            {
                return NotFound("Friend request not found.");
            }

            request.Status = FriendRequestStatus.Declined;
            await _context.SaveChangesAsync();

            return Ok(new FriendRequestDTO
            {
                Id = request.Id,
                SenderUserName = request.Sender.UserName,
                ReceiverUserName = request.Receiver.UserName,
                Status = request.Status.ToString(),
                CreatedAt = request.CreatedAt
            });
        }
    }
}