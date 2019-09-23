using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileBackend.Models;

namespace MobileBackend.Controllers {
    [Route("api/Users")]
    [ApiController]
    public class UsersController : DefaultController {
        private readonly MobileBackendContext _context;

        public UsersController(MobileBackendContext context) {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DBUser>>> GetUser([FromBody] string id) {
            if (id == null || id == "" || id == "00000000-0000-0000-0000-000000000000") {//create
                return await _context.DBUser.ToListAsync();
            } else {//update
                var user = await _context.DBUser.Include("devices").FirstOrDefaultAsync(x => x.UUID == new Guid(id));
                //_context.Entry(user).Reference(x => x.devices).Load();
                //_context.Entry(user).Reference(x => x.AcessableInstruments).Load();
                List<DBUser> list = new List<DBUser>() { user };
                if (user == null) {
                    return NotFound();
                }

                return list;
            }

        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<DBUser>> createUser([FromBody] DBUser user) {
            // 
            // using (var context = new MobileBackendContext(null)) {
            //if (_context.DBUser.Any(e => e.UUID == user.UUID)) 
            // Console.WriteLine(user);
            if (user.userName == null) {
                return BadRequest("Username required");
            }
            if (_context.DBUser.Any(e => e.userName == user.userName)) {
                return BadRequest("Username already Registered");
            }
            _context.DBUser.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", user);//new { id = user.ID },
        }


        [HttpPost]
        [Route("updateregistrationID")]
        public async Task<ActionResult<DBUser>> updateregistration([FromBody] DBUser user) {
            var _user = await _context.DBUser.FirstOrDefaultAsync(e => e.UUID == user.UUID);
            //ocaisonaly notification hub tokens will expire and when they do the backeend database will need the new device id they registered

            // _user.
            // _context.Update()

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", user);//new { id = user.ID },
        }

        [HttpPost]
        [Route("addDevice")]
        public async Task<ActionResult<DBUser>> AddDevice([FromBody] DBUser user) {
            // 
            if (user.UUID == null || user.devices == null || user == null) {
                return BadRequest();
            }
            Console.WriteLine(user.devices);
            var result = _context.DBUser.SingleOrDefault(x => x.UUID == user.UUID);
            if (result != null) {
                if (result.devices == null) {
                    result.devices = user.devices;
                } else {
                    result.devices.AddRange(user.devices);
                }
                /*
                Console.WriteLine(result.devices.GetType());
                Console.WriteLine(typeof(List<device>));
                if (result.devices.GetType()== typeof(List<device>)) {
                    result.devices.AddRange(user.devices);
                } else {
                    result.devices = user.devices;
                }//*/
            } else {
                return NotFound();
            }



            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                throw;
            }

            return NoContent();
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DBUser>> DeleteUser(int id) {
            var user = await _context.DBUser.FindAsync(id);
            if (user == null) {
                return NotFound();
            }

            _context.DBUser.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpGet]
        [Route("test/{userId}")]
        public async Task<ActionResult<DBUser>> testNotify(string userId) {
            var user = await _context.DBUser.Include("devices").FirstOrDefaultAsync(x => x.UUID == new Guid(userId));
            //_context.Entry(user).Reference(x => x.devices).Load();
            //_context.Entry(user).Reference(x => x.AcessableInstruments).Load();

            // if (user == null) {
            //     return NotFound();
            //}

           // List<string> user = new List<string>();
           // user.Add("device:" + DeviceID);

            azureComunications.sendToUsersbyID(user.devices.ToArray(), "", "");

            return NoContent();
        }


        private bool UserExists(Guid id) {
            return _context.DBUser.Any(e => e.UUID == id);
        }
    }
}