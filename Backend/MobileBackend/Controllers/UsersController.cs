using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                return await _context.DBUser.Include(x => x.login).Include("devices").Include("watchingNotificcations").ToListAsync();///Include("devices").
            } else {//update
                var user = await _context.DBUser.Include("devices").Include("watchingNotificcations").FirstOrDefaultAsync(x => x.UUID == new Guid(id));
                //_context.Entry(user).Reference(x => x.devices).Load();
                //_context.Entry(user).Reference(x => x.AcessableInstruments).Load();
                List<DBUser> list = new List<DBUser>() { user };
                if (user == null) {
                    return NotFound();
                }

                return list;
            }

        }


        [HttpGet]
        [Route("notifcDbuse")]
        public async Task<ActionResult<IEnumerable<NotificationDBUser>>> GetnotifcDbuse() {
            return await _context.NotificationDBUser.ToListAsync();
        }

        [HttpGet]
        [Route("devs")]
        public async Task<ActionResult<IEnumerable<device>>> Getdevs() {
            return await _context.device.ToListAsync();
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<DBUser>> createUser([FromBody] DBUser user) {
            // 
            // using (var context = new MobileBackendContext(null)) {
            //if (_context.DBUser.Any(e => e.UUID == user.UUID)) 
             Console.WriteLine(user);
            if (user.login.userName == null) {
                return BadRequest("Username required");
            }
            if (_context.DBUser.Any(e => e.login.userName == user.login.userName)) {
                return BadRequest("Username already Registered");
            }
            rfc2898pwd hasher = new rfc2898pwd();
            var salt = hasher.Pepper();
            var encriptedPWD = hasher.hash(user.login.pwd,salt);
            user.login.encrypted = encriptedPWD;
            user.login.salt = salt;
            var exsisting =_context.device.FirstOrDefault(x => x.DeviceID == user.devices.First().DeviceID);
            if (exsisting != null) {
                user.devices.Clear();
                user.devices.Add(exsisting);
            }
            user.pingAll();
            _context.DBUser.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", user);//new { id = user.ID },
        }


        [HttpPost]
        [Route("loin")]
        public async Task<ActionResult<DBUser>> loginUser([FromBody] DBUser user) {
            // 
            // using (var context = new MobileBackendContext(null)) {
            //if (_context.DBUser.Any(e => e.UUID == user.UUID)) 
            // Console.WriteLine(user);
            string username = user.login.userName;
            if (username == null) {
                return BadRequest("Username required");
            }
            var potential = _context.DBUser.Include(x => x.login).FirstOrDefault(e => e.login.userName == username);
            if (potential == null) {
                return BadRequest("Username not exist");
            }
            rfc2898pwd hasher = new rfc2898pwd();
            /*string encriptedPWD = "";
            try {
                encriptedPWD = hasher.deHash(potential.login.encrypted, user.login.pwd, potential.login.salt);
            }catch(Exception err) {
                Console.WriteLine(err);
            }*/
            var encriptedPWD = hasher.hash(user.login.pwd, potential.login.salt);
            if (!ArrayEquals(encriptedPWD, potential.login.encrypted)) {
                Console.WriteLine("salt: " + potential.login.salt);
                Console.WriteLine("stored: " + user.login.pwd);
                Console.WriteLine("new   : " + encriptedPWD);
                Console.WriteLine("old   : " + potential.login.encrypted);
                var uf8 = new System.Text.UTF8Encoding(false);
                Console.WriteLine("old   : " + System.Text.Encoding.Unicode.GetString(potential.login.encrypted) ); 
                    Console.WriteLine("old   : " + uf8.GetString(potential.login.encrypted));
                return BadRequest("password");
            }
            // user.pingAll();
            //_context.DBUser.Add(user);
            //await _context.SaveChangesAsync();
            user.login = null;
            return CreatedAtAction("GetUser", potential);//new { id = user.ID },
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
            var result = _context.DBUser.FirstOrDefault(x => x.UUID == user.UUID);
            user.pingAll();

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


        [HttpPost]
        [Route("ping")]
        public async Task<ActionResult<DBUser>> ping([FromBody] dynamic device) {

            Guid devID = new Guid((string)device.DeviceID);
            if (devID == null || devID == new Guid("00000000-0000-0000-0000-000000000000")) {
                return BadRequest();
            }
            var dev = _context.device.FirstOrDefault(x => x.DeviceID == devID);
            dev.ping();
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DBUser>> DeleteUser(Guid id) {
            var user = await _context.DBUser.Where(x => x.UUID == id).FirstOrDefaultAsync();
            if (user == null) {
                return NotFound();
            }
            _context.Entry(user).Collection(x => x.watchingNotificcations).Load();

            foreach(NotificationDBUser toRemove in user.watchingNotificcations) {
                _context.Notification.Include(x => x.RegisteredUsers).Where(x => x.NotifivationID == toRemove.NotifivationID).FirstOrDefault().RegisteredUsers.Remove(toRemove);
               // _context.NotificationDBUser.Remove(toRemove);
            }

            _context.DBUser.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }


        private bool UserExists(Guid id) {
            return _context.DBUser.Any(e => e.UUID == id);
        }

        public static bool ArrayEquals<T>(T[] a, T[] b) {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++) {
                if (!a[i].Equals(b[i]))
                    return false;
            }
            return true;
        }

    }

    public class rfc2898pwd {
        //wrapper for Rfc2898 to salt and hash passwords
        private int Iterations = 1000;
        public byte[] Pepper() {
            byte[] salt = new byte[8];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider()) {
                // Fill the array with a random value.
                rngCsp.GetBytes(salt);
            }
            return salt;
        }

        public byte[] hash(string pwd, byte[] salt ) {
            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(pwd, salt, Iterations);
            // Encrypt the data.
            byte[] rawPlaintext = System.Text.Encoding.Unicode.GetBytes(pwd);
            using (Aes aes = new AesManaged()) {
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;          // in bits
                aes.Key = k1.GetBytes(16);  // 16 bytes for 128 bit encryption
                aes.IV = new byte[128 / 8];   // AES needs a 16-byte IV
                byte[] cipherText = null;

                using (MemoryStream ms = new MemoryStream()) {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)) {
                        cs.Write(rawPlaintext, 0, rawPlaintext.Length);
                    }

                    cipherText = ms.ToArray();
                }
                return cipherText;
            }
        }

        public string deHash(byte[] encrypted, string pwd, byte[] salt) {
            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(pwd, salt, Iterations);
            // Encrypt the data.
            byte[] rawPlaintext = System.Text.Encoding.Unicode.GetBytes(pwd);
            using (Aes aes = new AesManaged()) {
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;          // in bits
                aes.Key = k1.GetBytes(16);  // 16 bytes for 128 bit encryption
                aes.IV = new byte[128 / 8];   // AES needs a 16-byte IV

                byte[] plainText = null;



                using (MemoryStream ms = new MemoryStream()) {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write)) {
                        cs.Write(encrypted, 0, encrypted.Length);
                    }

                    plainText = ms.ToArray();
                }
                string s = System.Text.Encoding.Unicode.GetString(plainText);
                return s;
            }
        }
    }
}