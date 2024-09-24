using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teams.Domain.Models;
using Teams.Persistence.Context;

namespace Teams.Controllers
{
    public class MessagesController : Controller
    {
        private readonly TeamsDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public MessagesController(TeamsDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: User Messages
        [HttpGet]
        public async Task<IActionResult> GetUserMessages(User user)
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










        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var teamsDbContext = _context.Message.Include(m => m.Channel).Include(m => m.User);
            return View(await teamsDbContext.ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Channel)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            ViewData["ChannelId"] = new SelectList(_context.Channels, "ChannelId", "ChannelName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MessageId,MessageText,SentAt,ChannelId,UserId")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChannelId"] = new SelectList(_context.Channels, "ChannelId", "ChannelName", message.ChannelId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", message.UserId);
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["ChannelId"] = new SelectList(_context.Channels, "ChannelId", "ChannelName", message.ChannelId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", message.UserId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageId,MessageText,SentAt,ChannelId,UserId")] Message message)
        {
            if (id != message.MessageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.MessageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChannelId"] = new SelectList(_context.Channels, "ChannelId", "ChannelName", message.ChannelId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", message.UserId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Channel)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Message.FindAsync(id);
            if (message != null)
            {
                _context.Message.Remove(message);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.MessageId == id);
        }
    }
}
