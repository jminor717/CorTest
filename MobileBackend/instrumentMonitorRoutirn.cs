using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MobileBackend {
    public class instrumentMonitorRoutirn {
        public bool keepAlive = true;

        public async void runer() {
            while (keepAlive) {
                try {
                    await getInstrumentData("localhost");
                }
                catch {
                    Console.WriteLine("network fail:  ");
                }
                Thread.Sleep(10000);
            }
        }

        private async Task<bool> getInstrumentData(string adress) {
            string data = string.Empty;
            string filter = "$filter=(NotificationType/EventType%20eq%20BD.HT.Common.Notifications.EventTypes%27Warning%27%20and%20%20ClearedByUser%20eq%20null)%20or%20(NotificationType/EventType%20eq%20BD.HT.Common.Notifications.EventTypes%27Alert%27%20and%20%20ClearedByInstrument%20eq%20null)";
            string expeand = "$expand=NotificationType,NotificationParameters,Instrument,Site,InstrumentConfigurationDevice";
            string url = @"https://"+ adress + ":44360/api/Notifications" + "?" + filter + "&" + expeand;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                data = reader.ReadToEnd();
            }
            var json = JsonConvert.DeserializeObject(data);
            Console.WriteLine(json);
            return true;
        }
    }
}
