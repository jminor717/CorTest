using MobileBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileBackend.corMonitoring {
    public class InstrumentAdder {
        public InstrumentAdder() { }

        public void addNotifications(MobileBackendContext context, Instrument toAdd) {

            var temp = context.Notification.Where(x => x.NotifivationID < 0).ToList();
            List<Notification> notificationTypes = new List<Notification>();

            foreach (Notification noti in temp) {
                Notification tump = (Notification)context.Entry(noti).CurrentValues.ToObject();
                tump.NotifivationID = new int();
                tump.InstrumentID = toAdd.InstrumentID;
                tump.OriginInstrument = toAdd;
                notificationTypes.Add(tump);
            }
            context.Notification.AddRange(notificationTypes);
            context.SaveChanges();
        }

        public Instrument createInstrument(MobileBackendContext context,Guid InstrumentGuid, string adress, string displayName) {
            Instrument toAdd = new Instrument { DisplayName = displayName, Adress = adress, UUID = InstrumentGuid };
            context.Instrument.Add(toAdd);
            context.SaveChanges();

            addNotifications(context, toAdd);
            return toAdd;
        }

    }
}
