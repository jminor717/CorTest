
using CORMobileApp.Helpers;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CORMobileApp.Droid {
    class AndroidHttps : IHTTPs {
        public Task<string> Get(string url, object body) {
            using (var client = new WebClient()) {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                client.Headers.Add("Content-Type", "application/json");
                return client.UploadStringTaskAsync(url, json);//"{'id':22}"
            }
            /*
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            byte[] bytes = Encoding.ASCII.GetBytes(json);
            long pos = request.GetRequestStream().Position;
            request.GetRequestStream().Write(bytes, (int)pos, bytes.Length);
            using (WebResponse response = request.GetResponse()) {
                using (Stream stream = response.GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        return reader.ReadToEndAsync();//.GetAwaiter().GetResult();
                        //Console.Write(responseString);
                        //return responseString;
                        //Debug.WriteLine(responseString);
                    }
                }
            }

            */
        }

        public Task<string> Post(string url, object body) {
            using (var client = new WebClient()) {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                client.Headers.Add("Content-Type", "application/json");
                return client.UploadStringTaskAsync(url, json);//"{'id':22}"
            }
        }

    }
}