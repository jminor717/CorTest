using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CORMobileApp.Database
{
    public class Batch
    {
        [PrimaryKey, AutoIncrement]
        public int BatchInternalID { get; set; }

        public string BatchExternalID { get; set; }
        public int NumTubes { get; set; }
        public string CurrentStep { get; set; }
        public string State { get; set; }

        public int InternalInstrumentID { get; set; }
        // public Instrument Instrument { get; set; }


        public ICollection<Batch> GenerateRandomBatches(int InstrumentID,int numBatches)
        {
            Random rand = new Random();
            ICollection<Batch> batches = new List<Batch>();
            int tempint;
            for (int i = 0; i< numBatches; i++)
            {
                Batch temp = new Batch { InternalInstrumentID= InstrumentID };
                temp.NumTubes = rand.Next(30);
                tempint = rand.Next(3);
                if (tempint == 0) { temp.State = "waiting"; }
                if (tempint == 1) { temp.State = "processing"; }
                if (tempint == 2) { temp.State = "completed"; }
                //Debug.WriteLine(temp.State+"  "+tempint);
                temp.BatchExternalID= Guid.NewGuid().ToString();
                batches.Add(temp);
            }
            return batches;
        }
    }
}
