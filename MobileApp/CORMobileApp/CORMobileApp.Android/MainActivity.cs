using Android.App;
using Android.Content.PM;
using Android.OS;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Android.Util;
using Android.Gms.Common;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace CORMobileApp.Droid
{
    [Activity(Label = "COR Status", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        private static readonly HttpClient client = new HttpClient();

        public const string TAG = "MainActivity";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        private static bool AlwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors) {
            return true;
        }
        protected override void OnCreate(Bundle bundle) {
            
            //notifications = new Notifications(this, NotificationSettings.HubName, NotificationSettings.HubListenConnectionString);

            /*
            ServicePointManager.Expect100Continue = true;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Ssl3;
            // ServicePointManager.ServerCertificateValidationCallback = delegate {
            //      return true;
            // };
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            // ServicePointManager.ServerCertificateValidationCallback +=(sender, cert, chain, sslPolicyErrors) => { return true; };
            //ServicePointManager.ServerCertificateValidationCallback +=(sender, cert, chain, sslPolicyErrors) => true;

            //*/

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AlwaysGoodCertificate);
            try {

                /*
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://10.0.2.2:3303/avalabelInstruments");
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().GetAwaiter().GetResult())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var responseString = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    Console.Write(responseString);
                    //Debug.WriteLine(responseString);
                }

                //HttpClient httpClient = new HttpClient();
               // var result = httpClient.GetAsync("https://10.0.2.2:3303/avalabelInstruments").Result;


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://10.0.2.2:3303/avalabelInstruments");
                req.Method = "GET";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;


                // Skip validation of SSL/TLS certificate
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };


                WebResponse respon = req.GetResponse();
                Stream res = respon.GetResponseStream();

                string ret = "";
                byte[] buffer = new byte[1048];
                int read = 0;
                while ((read = res.Read(buffer, 0, buffer.Length)) > 0) {
                    Console.Write(Encoding.ASCII.GetString(buffer, 0, read));
                    ret += Encoding.ASCII.GetString(buffer, 0, read);
                }





                /*
                using (var client = new HttpClient(new HttpClientHandler { }))
                {
                    var responseString = client.GetAsync("https://10.0.2.2:7000/signalr//negotiate?clientProtocol=1.5&_=1567610154102").GetAwaiter().GetResult();
                    //Debug.WriteLine(responseString);

                }

                                var values = new Dictionary<string, string> {
                    { "thing1", "hello" },
                    { "thing2", "world" }
                };

                var content = new FormUrlEncodedContent(values);

                var responsey = client.PostAsync("https://10.0.2.2:3303/notifictions/register", content).GetAwaiter().GetResult();

                var responseStringy = responsey.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.Write(responseStringy);

                //*/




            }
            catch (Exception err) {
                //System.Net.Sockets.SocketException     System.IO.IOException
                var x = 2;

            }

            App.InitFS(new AndroidFileSystem());
            //App.Initmsger(new AndroidNotifications());
            App.Initmsger(new AndroidMessagingService());
            App.msger.setContext(this);
            App.InitHttps(new AndroidHttps());
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            if (Intent.Extras != null) {
                foreach (var key in Intent.Extras.KeySet()) {
                    if (key != null) {
                        var value = Intent.Extras.GetString(key);
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    }
                }
            }

            IsPlayServicesAvailable();
            CreateNotificationChannel();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }


        public bool IsPlayServicesAvailable() {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success) {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else {
                    Log.Debug(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(TAG, "Google Play Services is available.");
            return true;
        }


        private void CreateNotificationChannel() {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O) {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = CHANNEL_ID;
            var channelDescription = string.Empty;
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default) {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}