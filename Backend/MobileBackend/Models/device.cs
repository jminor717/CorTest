using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobileBackend.Models {
    public class device {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Platform { get; set; }
        public string notificationHubRegistration { get; set; }
        public Guid DeviceID { get; set; }
        public virtual DateTime LastContact { get; set; }

        public bool isAlive() {
            return DateTime.Now - this.LastContact < TimeSpan.FromDays(4);
        }

        public void ping() {
            this.LastContact = DateTime.Now;
        }
    }
}
