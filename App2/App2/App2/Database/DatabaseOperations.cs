
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using App2.Database;
using System.Diagnostics;
using App2.Database.Entities;
using System;

namespace App2 {
    public class DatabaseOperations {
        readonly SQLiteAsyncConnection database;

        public DatabaseOperations(string dbPath) {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Batch>().Wait();
            database.CreateTableAsync<Instrument>().Wait();
            database.CreateTableAsync<SettingsStorage>().Wait();
            database.CreateTableAsync<persistantUserData>().Wait();
        }

        public Task<List<Instrument>> GetInstrumentsAsync() {
            return database.Table<Instrument>().ToListAsync();
        }

        public Instrument GetInstrumentAsync(int id) {
            Instrument temp = database.Table<Instrument>().Where(i => i.InternalInstrumentID == id).FirstOrDefaultAsync().Result;
            // ICollection<dynamic> batches = database.Table<Batch>().Where(x => x.InternalInstrumentID == temp.InternalInstrumentID).ToListAsync().Result;
            // temp.batches = batches;
            return temp;
        }

        public Instrument GetInstrumentAsync(string name) {
            Instrument temp = database.Table<Instrument>().Where(i => i.Name == name).FirstOrDefaultAsync().Result;
            // ICollection<dynamic> batches = database.Table<Batch>().Where(x => x.InternalInstrumentID == temp.InternalInstrumentID).ToListAsync().Result;
            // temp.batches = batches;
            return temp;
        }

        public int SaveInstrumentAsync(Instrument item) {
            /*
            List<Instrument> ins = database.QueryAsync<Instrument>("SELECT 1 FROM [Instrument] ").GetAwaiter().GetResult();
            foreach (Instrument iin in ins)
            {
                Debug.WriteLine(iin.Name+" "+iin.UUID+" "+iin.ID);
            }//*/
            try {
                if (item.InternalInstrumentID != 0) {
                    //await 
                    return database.UpdateAsync(item).GetAwaiter().GetResult();
                } else {
                    //await
                    return database.InsertAsync(item).GetAwaiter().GetResult();
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e);
                return 0;
            }
        }

        public Task<int> DeleteInstrumentAsync(Instrument item) {
            return database.DeleteAsync(item);
        }

        public Task<int> AddBatches(ICollection<Batch> batches) {
            return database.InsertAllAsync(batches);
        }

        public Task<int> UpdateBatches(ICollection<Batch> batches) {
            return database.UpdateAllAsync(batches);
        }

        public List<string> GetNames() {
            List<Instrument> names = database.QueryAsync<Instrument>("SELECT Name FROM [Instrument] ").GetAwaiter().GetResult();
            List<string> strs = new List<string>();
            foreach (Instrument instrumentname in names) {
                strs.Add(instrumentname.Name);
            }
            return strs;
        }

        public SettingsStorage GetSettings() {
            return database.Table<SettingsStorage>().FirstOrDefaultAsync().Result;
        }

        public persistantUserData getCurrentNotificationID() {
            return database.Table<persistantUserData>().FirstOrDefaultAsync().Result;
        }

        public async void updateCurrrentNotificationID(string id, string token, Guid Deviceid) {
            persistantUserData notie = new persistantUserData { registeredID = id, token = token, DeviceID = Deviceid, RemoteNeedsUpdated = true };
            try {
                var current = getCurrentNotificationID();
                notie.userID = current.userID;
                notie.UserName = current.UserName;
            }
            catch {
                Debug.WriteLine("no prior user found in db");
            }
            
            try {
                await database.Table<persistantUserData>().DeleteAsync(x => x != null);
            }
            catch (Exception e) { Debug.WriteLine(e); }
            await database.InsertAsync(notie);
        }

        public async Task<int> remoteAlerted(Guid userid) {
            var noti = await database.Table<persistantUserData>().FirstOrDefaultAsync();
            noti.RemoteNeedsUpdated = false;
            noti.userID = userid;
            return await database.UpdateAsync(noti);
        }
    }
}
