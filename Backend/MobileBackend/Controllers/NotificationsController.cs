using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileBackend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MobileBackend.Controllers {
    [Route("api/Notifications")]
    [ApiController]
    public class NotificationsController : DefaultController {
        private readonly MobileBackendContext _context;

        public NotificationsController(MobileBackendContext context) {
            _context = context;
        }

        // GET: api/Notifications
        [HttpPost, HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotification([FromBody] dynamic userid) {
            Guid userUUID = new Guid((string)userid.UUID);
            var user = _context.DBUser.Include("watchingNotificcations").Where(x => x.UUID == userUUID).FirstOrDefault();
            var notifications = await _context.Notification.Include("OriginInstrument").Include(x => x.RegisteredUsers).ToListAsync();
            if (user == null) { return notifications; }

            foreach (Notification notif in notifications) {
                notif.isWatching = false;
                foreach (NotificationDBUser notifreg in notif.RegisteredUsers) {
                    if (user.UUID == notifreg.UserUUID) { notif.isWatching = true; break; }
                }
            }
            // var temp = notifications.AsQueryable().Where(x => x.RegisteredUsers.AsQueryable().Where(y => y.UserUUID == user.UUID).Any()).ToList();

            return notifications; //.Include("RegisteredUsers")
            /*var temp =
            dynamic ret;
            try {
                ret = JsonConvert.SerializeObject(temp);
            }catch (Exception err) {
                throw err;
            }
            return ret;*/
        }

        [HttpGet]
        [Route("RaisedNotification")]
        public async Task<ActionResult<IEnumerable<RaisedNotification>>> GetrzedNotification() {
            return await _context.RaisedNotification.ToListAsync();
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

        [HttpPost]
        [Route("watch")]///not used
        public async Task<ActionResult<Notification>> watchNotification([FromBody] dynamic toWatch) {
            int notificationID = toWatch.notifivationID;
            var notification = _context.Notification.Include(x => x.RegisteredUsers).Where(x => x.NotifivationID == notificationID).FirstOrDefault();
            string usrguid = toWatch.userGuid;
            var user = _context.DBUser.Include("watchingNotificcations").Where(x => x.UUID == Guid.Parse(usrguid)).FirstOrDefault();

            if (notification == null || user == null) {
                return NotFound();
            }

            var user_notif = new NotificationDBUser() {
                //DBUser = user,
                UserUUID = user.UUID,
                // Notification = notification,
                NotifivationID = notification.NotifivationID
            };

            if (user.watchingNotificcations.AsQueryable().Any(x => x.NotifivationID == notification.NotifivationID)) {// user already registered for this notification
                return notification;
            }

            try {
                notification.RegisteredUsers.Add(user_notif);
                user.watchingNotificcations.Add(user_notif);
                _context.SaveChanges();
            }
            catch (Exception err) {
                Console.WriteLine(err);
            }
            //

            return notification;
        }

        [HttpPost]
        [Route("UpDateWatchList")]
        public async Task<ActionResult<Notification>> upDateWatchList([FromBody] dynamic Userchanges) {
            // int notificationID = toWatch.notifivationID;
            //  
            string usrguid = Userchanges.userGuid;
            var user = _context.DBUser.Include("watchingNotificcations").Where(x => x.UUID == Guid.Parse(usrguid)).FirstOrDefault();
            if(user == null) {
                return BadRequest("user not registered");
            }
            JArray changes = Userchanges.changes;
            if (changes.Count == 0) { return NoContent(); }
            HashSet<int> changedNotifications = new HashSet<int>();
            foreach (dynamic change in changes) {
                changedNotifications.Add((int)change.ID);
            }
            var notifications = _context.Notification.Include(x => x.RegisteredUsers).Where(x => changedNotifications.Contains(x.NotifivationID)).AsQueryable();
            foreach (dynamic change in changes) {
                int id = change.ID;
                bool watch = change.watchState;
                var notific = notifications.Where(x => x.NotifivationID == id).FirstOrDefault();
                var user_notif = new NotificationDBUser() {
                    UserUUID = user.UUID,
                    NotifivationID = notific.NotifivationID
                };
                if (watch) {
                    notific.RegisteredUsers.Add(user_notif);
                    user.watchingNotificcations.Add(user_notif);
                } else {
                    var rev = notific.RegisteredUsers.Where(x => x.UserUUID == user_notif.UserUUID).FirstOrDefault();
                    notific.RegisteredUsers.Remove(rev);
                    user.watchingNotificcations.Remove(rev);
                    //_context.NotificationDBUser.Remove(rev);
                }
            }
            _context.SaveChanges();

            return NoContent();
        }


        private bool NotificationExists(int id) {
            return _context.Notification.Any(e => e.NotifivationID == id);
        }
    }
}

