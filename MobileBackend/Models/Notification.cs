using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobileBackend.Models {
    /// <summary>
    /// list of notifications users can subscribe to based on instrument and type of alert/ warning
    /// </summary>
    public class Notification {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int InstrumentID { get; set; }
        public Guid InstumentGuid { get; set; }
        public int NotificationID { get; set; }
        public string NotificationName { get; set; }
        public string NotificationDescription { get; set; }

        [ForeignKey("User")]
        public virtual ICollection<DBUser> RegisteredUsers { get; set; }

        //public IEnumerable<Guid> RegisteredUsers { get; set; }
    }
}
