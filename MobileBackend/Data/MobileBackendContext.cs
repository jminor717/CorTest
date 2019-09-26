using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MobileBackend.Models;

namespace MobileBackend.Models
{
    public class MobileBackendContext : DbContext
    {
        public MobileBackendContext (DbContextOptions<MobileBackendContext> options)
            : base(options)
        {
        }//

        public DbSet<MobileBackend.Models.Notification> Notification { get; set; }

        public DbSet<MobileBackend.Models.RegisteredUsers> RegisteredUsers { get; set; }

        public DbSet<MobileBackend.Models.DBUser> DBUser { get; set; }

        public DbSet<MobileBackend.Models.device> device { get; set; }

        public DbSet<MobileBackend.Models.Instrument> Instrument { get; set; }

        public DbSet<MobileBackend.Models.RaisedNotification> RaisedNotification { get; set; }
    }
}
