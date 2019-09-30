using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MobileBackend.AzureControlers;
using MobileBackend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace MobileBackend.corMonitoring
{
    public class CorMonitorService : IScheduledTask {
        private readonly MobileBackendContext _context;
        //public bool keepAlive = true;
        //private Thread monitorThread;
        private int i = 0;
        private List<Instrument> instruments = new List<Instrument>();
        private AzureNotifications azureComunications = new AzureNotifications("Endpoint=sb://cor-test.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=n/QrPMw/COwX7rBtd7/+lWewVcmI9oO9/4lBsn3MLYs=", "COR-test-notifications");
        public CorMonitorService(MobileBackendContext context) {
            _context = context;
            BakaSeed();
        }

        public string Schedule => "* * * * *";

        public int exicuteEverySeconds => 60;

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            if (i % 30 == 0) {//minimise context usage only update instrument list once every 5 min
                instruments = _context.Instrument.ToList();
            }
            i++;
            try {
                foreach (Instrument sturment in instruments) {
                    await getInstrumentData(sturment);
                }
            }
            catch (Exception err) {
                Console.WriteLine("network fail:  "+err );
            }
        }

        private async Task<bool> getInstrumentData(Instrument source) {
            string data = string.Empty;
            string filter = "$filter=(NotificationType/EventType%20eq%20BD.HT.Common.Notifications.EventTypes%27Warning%27%20and%20%20ClearedByUser%20eq%20null)%20or%20(NotificationType/EventType%20eq%20BD.HT.Common.Notifications.EventTypes%27Alert%27%20and%20%20ClearedByInstrument%20eq%20null)";
            string expeand = "$expand=NotificationType,NotificationParameters,Instrument,Site,InstrumentConfigurationDevice";
            string url = @"https://" + source.Adress + ":44360/api/Notifications" + "?" + filter + "&" + expeand;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                data = reader.ReadToEnd();
                parseNotifications(data, source);
            }

            return true;
        }

        private void parseNotifications(string data, Instrument source) {
            dynamic json = JsonConvert.DeserializeObject(data);
            JArray arr = json.value;
            foreach (dynamic notification in arr) {
                string incomingGuid = notification.NotificationId;
                if (!_context.RaisedNotification.Any(e => e.NotificationId == new Guid(incomingGuid))) {
                    RaisedNotification incommming = new RaisedNotification() {
                        Name= (string)notification.NotificationType.Name,
                        Description= (string)notification.NotificationType.Description,
                        DetailDescription = (string)notification.NotificationType.DetailDescription,
                        InstrumentDisplayName = 
                            (string)notification.Instrument.Name +" "+ (string)notification.Instrument.InstrumentSide
                            +" ("+ (string)notification.InstrumentSerialNumber+") - " + (string)notification.NotificationTypeId,
                        NotificationId = new Guid((string)notification.NotificationId),
                        originatingInstrument = source,
                        When = (DateTimeOffset)notification.When,
                        notificationType = (string)notification.NotificationType.EventType
                    };
                    azureComunications.sendToUsersbyID(whoGetsIt(incommming), incommming, source);
                   // _context.RaisedNotification.Add(incommming);

                    Console.WriteLine((object)notification.NotificationId);
                }
            }
        }

        private List<device> whoGetsIt(RaisedNotification incoming) {//.ThenInclude(post => post.Author)    .Include(x => x.RegisteredUsers.Select(u => u.devices))   .ThenInclude(DBUser => DBUser.device)
            List<device> outgoing = new List<device>();
            var posible = _context.Notification.Include(notif => notif.RegisteredUsers).Where(x => x.InstrumentID == incoming.originatingInstrument.InstrumentID);
            Notification only = posible.Where(x => x.NotificationName == incoming.notificationType).FirstOrDefault();

            // _context.Entry(only).Collection(x => x.RegisteredUsers).Load();
            // _context.Entry(only).Collection(x => x.RegisteredUsers.AsQueryable().Select(y => y.devices)).Load();////////////////////////////////////////
            //_context.Entry(only).Reference(x => x.RegisteredUsers).Load();
            //only.RegisteredUsers.AsQueryable().Select(x => x.devices).Load();


            List<NotificationDBUser> users = only.RegisteredUsers;
            foreach (NotificationDBUser notif_usr in users) {
                //_context.Entry(notif_usr).Reference(x => x.DBUser).Load();
                var usr = _context.DBUser.Where(x => x.UUID == notif_usr.UserUUID).FirstOrDefault();
                    _context.Entry(usr).Collection(x => x.devices).Load();
                var alive = usr.getAlive();
                    outgoing.AddRange(alive);
            }
            return outgoing;
        }


        //entity framework seed baka, me fix here
        private void BakaSeed() {
            List<Notification> notifications = new List<Notification>();
            Instrument defaultinst = _context.Instrument.FirstOrDefault(x => x.InstrumentID == -1);
            notifications = _context.Notification.Include("OriginInstrument").ToList();
            foreach (Notification noti in notifications) {
                if (noti.OriginInstrument == null) {
                    noti.OriginInstrument = defaultinst;
                }
            }
            _context.SaveChanges();
        }
    }
}