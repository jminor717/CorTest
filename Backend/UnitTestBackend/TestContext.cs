using Microsoft.EntityFrameworkCore;
using MobileBackend.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace UnitTestBackend {
    class TestContext {
        public TestContext() {
            this.Notification = new TestDbSet<Notification>();
            this.RegisteredUsers = new TestDbSet<RegisteredUsers>();
            this.DBUser = new TestDbSet<DBUser>();
            this.device = new TestDbSet<device>();
            this.Instrument = new TestDbSet<Instrument>();
            this.RaisedNotification = new TestDbSet<RaisedNotification>();
            this.NotificationDBUser = new TestDbSet<NotificationDBUser>();
        }

        public DbSet<MobileBackend.Models.Notification> Notification { get; set; }

        public DbSet<MobileBackend.Models.RegisteredUsers> RegisteredUsers { get; set; }

        public DbSet<MobileBackend.Models.DBUser> DBUser { get; set; }

        public DbSet<MobileBackend.Models.device> device { get; set; }

        public DbSet<MobileBackend.Models.Instrument> Instrument { get; set; }

        public DbSet<MobileBackend.Models.RaisedNotification> RaisedNotification { get; set; }

        public DbSet<MobileBackend.Models.NotificationDBUser> NotificationDBUser { get; set; }

        public int SaveChanges() {
            return 0;
        }


        public void Dispose() { }
    }


    class TestDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T>
    where T : class {
        ObservableCollection<T> _data;
        IQueryable _query;

        public TestDbSet() {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }
        /*
        public override T Add(T item) {
            _data.Add(item);
            return item;
        }

        public override T Remove(T item) {
            _data.Remove(item);
            return item;
        }

        public override T Attach(T item) {
            _data.Add(item);
            return item;
        }

        public override T Create() {
            return Activator.CreateInstance<T>();
        }

        public override TDerivedEntity Create<TDerivedEntity>() {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override ObservableCollection<T> Local {
            get { return new ObservableCollection<T>(_data); }
        }
        */
        Type IQueryable.ElementType {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return _data.GetEnumerator();
        }
    }
}
