using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MobileBackend.Models {
    public interface IBackendContext : IDisposable {
        DbSet<MobileBackend.Models.Notification> Notification { get; set; }

        DbSet<MobileBackend.Models.RegisteredUsers> RegisteredUsers { get; set; }

        DbSet<MobileBackend.Models.DBUser> DBUser { get; set; }

        DbSet<MobileBackend.Models.device> device { get; set; }

        DbSet<MobileBackend.Models.Instrument> Instrument { get; set; }

        DbSet<MobileBackend.Models.RaisedNotification> RaisedNotification { get; set; }

        DbSet<MobileBackend.Models.NotificationDBUser> NotificationDBUser { get; set; }
        int SaveChanges();

        Task<int> SaveChangesAsync( CancellationToken cancellationToken = default);

         EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
