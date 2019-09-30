using CORMobileApp.Models;
using SQLite;
using System.Collections.Generic;

namespace CORMobileApp.Database
{
    public class Instrument : BaseDataObject
    {
        [PrimaryKey, AutoIncrement]
        public int InternalInstrumentID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Login { get; set; }
        /// <summary>
        /// number of finished batches still in the instrument
        /// </summary>
        public int CompletedBatches { get; set; }
        public int UnfinisedBatches { get; set; }

        public string Password { get; set; }

        [Ignore]
        public ICollection<dynamic> batches { get; set; }

        [Ignore]
        public ICollection<dynamic> notifications { get; set; }

        [Ignore]
        public dynamic InstrumentData { get; set; }
    }
}
