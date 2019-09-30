using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileBackend.Models {
    public class DBUser {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       // public int ID { get; set; }
        public Guid UUID { get; set; }
        public login login { get; set; }

        [ForeignKey("device")]
        public virtual List<device> devices { get; set; }

        [ForeignKey("Instrument")]
        public virtual List<Instrument> AcessableInstruments { get; set; }

        //[ForeignKey("Notification")]
        public virtual List<NotificationDBUser> watchingNotificcations { get; set; }
        


        public bool anyAlive() {
            foreach(device device in this.devices) {
                if (device.isAlive()) return true;
            }
            return false;
        }

       public void pingAll() {
            foreach (device device in this.devices) {
                device.ping();
            }
        }

        public List<device> getAlive() {
            List<device> alive = new List<device>();
            foreach(device device in this.devices) {
                if (device.isAlive()) alive.Add(device);
            }
            return alive;
        }
        //public IEnumerable<int> AcessableInstrumentIDS { get; set; }

    }
}
