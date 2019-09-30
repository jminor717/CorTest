using SQLite;
using System;

namespace CORMobileApp.Database.Entities {
    public class persistantUserData{
        [PrimaryKey]
        public string registeredID { get; set; }
        public string token { get; set; }
        public Guid userID { get; set; }
        public Guid DeviceID { get; set; }
        public bool RemoteNeedsUpdated { get; set; }
        public string UserName { get; set; }
        
    }
}
