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
        public string userName { get; set; }

        [ForeignKey("device")]
        public virtual List<device> devices { get; set; }

        [ForeignKey("Instrument")]
        public virtual List<Instrument> AcessableInstruments { get; set; }
        //public IEnumerable<int> AcessableInstrumentIDS { get; set; }

    }
}
