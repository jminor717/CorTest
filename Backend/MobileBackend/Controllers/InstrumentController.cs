using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBackend.Models;

namespace MobileBackend.Controllers
{
    [Route("api/Instrument")]
    [ApiController]
    public class InstrumentController : DefaultController {
        private readonly MobileBackendContext _context;

        public InstrumentController(MobileBackendContext context) {
            _context = context;
        }
        // GET: api/devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instrument>>> Getdevice() {
            return _context.Instrument.ToList();
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Instrument>> AddInstrument([FromBody] Instrument inst) {
            Instrument toAdd = Adder.createInstrument(_context, Guid.NewGuid(),inst.Adress,inst.DisplayName); //new Instrument { DisplayName = inst.DisplayName, Adress = inst.Adress, UUID = Guid.NewGuid() };
            //Adder.createInstrument();
            //_context.Instrument.Add(toAdd);
            return toAdd;
        }

        // DELETE: api/devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Instrument>> Deletedevice(int id) {
            var device = await _context.Instrument.FindAsync(id);
            if (device == null) {
                return NotFound();
            }
            var motificatioons = _context.Notification.Where(x => x.InstrumentID == id).ToList();

            _context.Instrument.Remove(device);
            _context.Notification.RemoveRange(motificatioons);
            await _context.SaveChangesAsync();

            return device;
        }
    }
}