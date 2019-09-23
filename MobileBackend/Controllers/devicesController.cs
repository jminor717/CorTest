/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileBackend.Models;

namespace MobileBackend.Controllers {
    [Route("api/devices")]
    [ApiController]
    public class devicesController : DefaultController {
        private readonly MobileBackendContext _context;

        public devicesController(MobileBackendContext context) {
            _context = context;
        }

        // GET: api/devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<device>>> Getdevice() {
            return await _context.device.ToListAsync();
        }

        // GET: api/devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<device>> Getdevice(int id) {
            var device = await _context.device.FindAsync(id);

            if (device == null) {
                return NotFound();
            }
            return device;



        }

        // PUT: api/devices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putdevice(int id, device device) {
            if (id != device.ID) {
                return BadRequest();
            }

            _context.Entry(device).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!deviceExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/devices
        [HttpPost]
        public async Task<ActionResult<device>> Postdevice(device device) {
            _context.device.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getdevice", new { id = device.ID }, device);
        }

        // DELETE: api/devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<device>> Deletedevice(int id) {
            var device = await _context.device.FindAsync(id);
            if (device == null) {
                return NotFound();
            }

            _context.device.Remove(device);
            await _context.SaveChangesAsync();

            return device;
        }

        private bool deviceExists(int id) {
            return _context.device.Any(e => e.ID == id);
        }
    }
}
*/