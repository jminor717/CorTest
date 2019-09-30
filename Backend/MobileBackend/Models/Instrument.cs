using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobileBackend.Models {
    public class Instrument {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstrumentID { get; set; }
        public Guid UUID { get; set; }
        public string Adress { get; set; }
        public string DisplayName { get; set; }

    }
}
