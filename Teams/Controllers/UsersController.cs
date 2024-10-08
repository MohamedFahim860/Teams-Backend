﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Teams.Domain.DTOs;
using Teams.Domain.Interfaces.Services;
using Teams.Domain.Models;
using Teams.Persistence.Context;

namespace Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TeamsDbContext _context;
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, TeamsDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                Console.WriteLine(user.UserName);
                int savedObjectCount = await _userService.AddUser(user);
                _logger.LogInformation($"User Added Successfully");
                return Ok(savedObjectCount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while trying to add a User: {ex}");
                return BadRequest($"An exception occurred while trying to add an User: {ex}");

            }

            //return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[HttpGet("current")]
        //public IActionResult GetCurrentUser()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (userId != null)
        //    {
        //        var user = _context.Users.Find(int.Parse(userId));  // Fetch the user from the database
        //        return Ok(user);
        //    }

        //    return Unauthorized();

        //}

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Get the current user's ID from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//Populating the User Object: When a request is made with an authentication token (e.g., a JWT token in the Authorization header or a cookie), the authentication middleware verifies the token. If valid, it creates a ClaimsPrincipal object from the token’s claims and attaches it to HttpContext.User. The User object is now available during the lifetime of that request, and can be accessed in any controller or service as HttpContext.User.
            //This user property of HttpContext object is automatically populated when the authentication middleware validates the incoming request (for example, via JWT, cookies, or other authentication schemes).


            if (userId != null)
            {
                // Call the service method to fetch the user
                var user = await _userService.GetUserById(int.Parse(userId));

                if (user != null)
                {
                    return Ok(user);
                }

                return NotFound("User not found");
            }
            return NotFound("User not found");
        }

        [HttpPost("check-email")]
        public async Task<IActionResult> CheckEmailExists(CheckEmailRequestDto email)
        {
            var isTaken = await _userService.CheckEmailExists(email);
            return Ok(isTaken);
        }

        [HttpPost("check-username")]
        public async Task<IActionResult> CheckUsernameExists(CheckUsernameRequestDto username)
        {
            var isTaken = await _userService.CheckUsernameExists(username);
            return Ok(isTaken);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}

//A Task in C# represents an operation that may run asynchronously and complete at some point in the future. When you execute an asynchronous method, it returns a Task object,
//which serves as a placeholder for the future result.

//It's also important,if we create control functions as synchronous(without using async, task, await and all), the rest of the service and the repository functions also should be 
//synchronous, otherwise it would give serialization error which would be a 500 error, and also if we create the controller functions as asynchronous, all the other functions which it calls,
//in the service and the repository files are also should be asynchronous.