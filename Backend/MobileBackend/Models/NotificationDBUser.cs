using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobileBackend.Models {
    public class NotificationDBUser {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Guid UserUUID { get; set; }
        //public DBUser DBUser { get; set; }
        public int NotifivationID { get; set; }
        //public Notification Notification { get; set; }
    }
}
