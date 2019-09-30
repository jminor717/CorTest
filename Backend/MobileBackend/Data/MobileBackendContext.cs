using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MobileBackend.Models
{
    public class MobileBackendContext : DbContext, IBackendContext {
        public MobileBackendContext (DbContextOptions<MobileBackendContext> options): base(options) {
        }//

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            Guid instGuid = Guid.NewGuid();
            Instrument localdev = new Instrument { UUID = instGuid, Adress = "localhost", InstrumentID = -1,DisplayName="VSPXXXXX" };

            modelBuilder.Entity<Instrument>().HasData(localdev);
            //*  
            modelBuilder.Entity<Notification>(n => {
                n.HasData(new { NotifivationID = -1, InstrumentID = -1, NotificationName = "Alert", NotificationDescription = "Instrument will cease to function without user attention" });
                n.HasData(new { NotifivationID = -2, InstrumentID = -1, NotificationName = "Warning", NotificationDescription = "Instrument will not function at full capacity without attention" });
                n.HasData(new { NotifivationID = -3, InstrumentID = -1, NotificationName = "ErrorSample", NotificationDescription = "Batch will not complete without user attention" });
                n.HasData(new { NotifivationID = -4, InstrumentID = -1, NotificationName = "LowInventory", NotificationDescription = "Inventory item below 30% capacity" });
                n.HasData(new { NotifivationID = -5, InstrumentID = -1, NotificationName = "EmptyInventory", NotificationDescription = "Inventory item below  5% capacity"});
            });
            // n.OwnsOne(x => x.OriginInstrument).HasData(new Instrument { UUID = instGuid, Adress = "localhost", InstrumentID = -1 });
            //*/
            //        ,
            //       new Notification { NotifivationID = -4, NotificationName = "LowInventory", NotificationDescription = "Inventory item below 30% capacity", RegisteredUsers = new List<DBUser>() },
            //        new Notification { NotifivationID = -5, NotificationName = "EmptyInventory", NotificationDescription = "Inventory item below  5% capacity", RegisteredUsers = new List<DBUser>() }
            //   );
        }


        public DbSet<MobileBackend.Models.Notification> Notification { get; set; }

        public DbSet<MobileBackend.Models.RegisteredUsers> RegisteredUsers { get; set; }

        public DbSet<MobileBackend.Models.DBUser> DBUser { get; set; }

        public DbSet<MobileBackend.Models.device> device { get; set; }

        public DbSet<MobileBackend.Models.Instrument> Instrument { get; set; }

        public DbSet<MobileBackend.Models.RaisedNotification> RaisedNotification { get; set; }
        
        public DbSet<MobileBackend.Models.NotificationDBUser> NotificationDBUser { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity) {
            return base.Entry(entity);
        }
    }
}
