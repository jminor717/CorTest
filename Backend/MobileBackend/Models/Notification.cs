using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobileBackend.Models {
    /// <summary>
    /// list of notifications users can subscribe to based on instrument and type of alert/ warning
    /// these are not actual notificatos from an instrument but a list of all notifications that can come from an instrument
    /// </summary>
    public class Notification {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotifivationID { get; set; }
        public int InstrumentID { get; set; }

        [ForeignKey("Instrument")]
        public virtual Instrument OriginInstrument { get; set; }
        public string NotificationName { get; set; }
        public string NotificationDescription { get; set; }

        //[ForeignKey("DBUser")]
        public virtual List<NotificationDBUser> RegisteredUsers { get; set; }

        [NotMapped]
        public bool isWatching { get; set; }
        //public IEnumerable<Guid> RegisteredUsers { get; set; }
    }
}
