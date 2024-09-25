﻿using System;
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
        public async Task<IActionResult> AddMessage(Message message, int ReceiverId)
        {
            try
            {
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

    }
}