using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobileBackend.Models {
    public class RaisedNotification {

        [Key]
        public Guid NotificationId { get; set; }
        public DateTimeOffset When { get; set; }

        [ForeignKey("Instrument")]
        public virtual Instrument originatingInstrument { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string InstrumentDisplayName { get; set; }
        public string DetailDescription { get; set; }
        public string notificationType { get; set; }
    }
}
