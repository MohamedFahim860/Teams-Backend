using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using NuGet.Protocol.Plugins;
using Teams.Domain.DTOs;
using Teams.Domain.Interfaces.Services;
using Teams.Domain.Models;
using Teams.Persistence.Context;
using Teams.Services.Services;

namespace Teams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly TeamsDbContext _context;
        private readonly ILogger<UsersController> _logger;
        private readonly IMessageService _messageService;

        public MessagesController(TeamsDbContext context, ILogger<UsersController> logger, IMessageService messageService)
        {
            _context = context;
            _logger = logger;
            _messageService = messageService;
        }

        // Add User Message
        [HttpPost("send")]
        public async Task<IActionResult> AddMessage(AddMessageDto text)
        {
            try
            {
                var ReceiverId = text.ReceiverId;
                var message = new Message
                {
                    MessageText = text.MessageText
                };

                Console.WriteLine(message.MessageText);
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//Getting the userId from the authentication token
                message.UserId = int.Parse(userId);

                var sendMessage = new SendMessageDto
                {
                    message = message,
                    ReceiverId = ReceiverId
                };

                int savedObjectCount = await _messageService.AddMessage(sendMessage);
                _logger.LogInformation($"Message Added Successfully");
                return Ok(savedObjectCount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while trying to add a Message: {ex}");
                return BadRequest($"An exception occurred while trying to add a Message: {ex}");

            }
        }

        // API to get all messages between logged-in user and another user
        [HttpGet("chat/{receiverId}")]
        public async Task<IActionResult> GetMessagesWithUser(int receiverId)
        {
            try
            {
                // Get the logged-in user's ID from the JWT token
                var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Call the service method to get all messages between the two users
                var messages = await _messageService.GetMessagesBetweenUsers(senderId, receiverId);

                // Return the list of messages
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching messages: {ex.Message}");
                return BadRequest("Failed to get messages.");
            }
        }

        [HttpDelete("delete/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            try
            {
                var result = await _messageService.DeleteMessage(messageId);

                if (result)
                {
                    _logger.LogInformation($"Message with ID {messageId} deleted successfully.");
                    return Ok($"Message with ID {messageId} deleted successfully.");
                }
                else
                {
                    _logger.LogWarning($"Message with ID {messageId} not found.");
                    return NotFound($"Message with ID {messageId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to delete message with ID {messageId}: {ex.Message}");
                return StatusCode(500, "Internal server error while trying to delete the message.");
            }
        }

    }
}
