using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileBackend.Models;

namespace MobileBackend.Controllers {
    [Route("api/Notifications")]
    [ApiController]
    public class NotificationsController : DefaultController {
        private readonly MobileBackendContext _context;

        public NotificationsController(MobileBackendContext context) {
            _context = context;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotification() {
            return await _context.Notification.ToListAsync();
        }

        // GET: api/Notifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id) {
            var notification = await _context.Notification.FindAsync(id);

            if (notification == null) {
                return NotFound();
            }

            return notification;
        }

        // PUT: api/Notifications/5
       // [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(int id, Notification notification) {
            if (id != notification.ID) {
                return BadRequest();
            }

            _context.Entry(notification).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!NotificationExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notifications
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification notification) {
            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotification", new { id = notification.ID }, notification);
        }
        /*
        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Notification>> DeleteNotification(int id) {
            var notification = await _context.Notification.FindAsync(id);
            if (notification == null) {
                return NotFound();
            }

            _context.Notification.Remove(notification);
            await _context.SaveChangesAsync();

            return notification;
        }
        */
        private bool NotificationExists(int id) {
            return _context.Notification.Any(e => e.ID == id);
        }
    }
}

