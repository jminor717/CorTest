using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobileBackend.Models {
    public class login {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int key { get; set; }
        public string userName { get; set; }
        [NotMapped]
        public string pwd { get; set; }
        //[MaxLength(16)]
        public byte[] encrypted { get; set; }
        //[MaxLength(16)]
        public byte[] salt { get; set; }
    }
}
