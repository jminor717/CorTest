using Android.Util;
using Firebase.Messaging;
using Android.Support.V4.App;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using App2.Droid.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Android.OS;

namespace App2.Droid {
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class AndroidMessagingService : FirebaseMessagingService {
        const string TAG = "MyFirebaseMsgService";
        NotificationHub hub;
        int notificationIndex = 0;
        List<ShownNotification> _notifications = new List<ShownNotification>();

        //public string ID { get { return PushHandlerService.RegistrationID; } }
        public override void OnMessageReceived(RemoteMessage message) {
            Log.Debug(TAG, "From: " + message.From);
            if (message.GetNotification() != null) {
                //These is how most messages will be received
                Log.Debug(TAG, message.GetNotification().Title +" ;;Notification Message Body: " + message.GetNotification().Body);
                SendNotification(message.GetNotification().Body, message.GetNotification().Title);
            } else {
                //Only used for debugging payloads sent from the Azure portal
                SendNotification(message.Data.Values.First(),"dev test");

            }
        }

        void SendNotification(string messageBody, string title) {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID);
            Log.Debug(TAG, "  :   " + messageBody);

            string notiType = shittyParse(messageBody, "notiType:") ?? "APP: failed to read notification";
            string body = shittyParse(messageBody, "body:") ?? "APP: failed to read notification";
            string displayName = shittyParse(messageBody, "name:");
            string guid = shittyParse(messageBody, "UUID:") ?? "00000000-0000-0000-0000-000000000000";
            Guid instrumentID = new Guid(guid);

            var incoming = new ShownNotification {
                instrumentID = instrumentID,
                instrumentDisplayName = displayName,
                types = new List<notificationDisplay>() {
                    new notificationDisplay {
                        notifType =notiType,
                        bodys = new List<string>() { body }
                    }
                }
            };

            var previos = incoming.InstrumentIsIN(_notifications);
            var notificationManager = NotificationManager.FromContext(this);

            if (previos != null) {
                var display = previos.getByType(notiType);
                display.bodys.Add(body);
                Console.WriteLine(_notifications);
                notificationManager.Notify(previos.index, previos.buildThis(notificationBuilder, pendingIntent));
            } else {
                incoming.index = notificationIndex++;
                _notifications.Add(incoming);
                notificationManager.Notify(incoming.index, incoming.buildThis(notificationBuilder, pendingIntent) );
            }
        }

        /// <summary>
        /// returns the string between --tag-- and the next  --,--  will return null if it encounters an error
        /// </summary>
        /// <param name="inpit"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string shittyParse(string inpit,string tag) {
            try {
                return inpit.Substring(inpit.IndexOf(tag) + tag.Length, inpit.IndexOf(",", (inpit.IndexOf(tag)) + ",".Length) - (inpit.IndexOf(tag) + tag.Length));
            }
            catch {
                return null;
            }
        }

        public override void OnNewToken(string token) {
            Log.Debug(TAG, "FCM token: ################" + token);
            SendRegistrationToServer(token );
        }

        public void SendRegistrationToServer(string token) {
            // Register with Notification Hubs
            hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, this);
            App.msger.setHub(hub);
            Guid DeviceID = Guid.NewGuid();
            var tags = new List<string>() { "device:" + DeviceID };

            //Log.Debug(TAG, "FCM tags: ******************" + tags.Count );
            // hub.Register(token, tags.ToArray());
            App.Database.updateCurrrentNotificationID("", token, DeviceID);

            var regID = hub.Register(token, tags.ToArray()).RegistrationId;

            Log.Debug(TAG, $"Successful registration of ID {regID} {regID.GetType()}");
            App.Database.updateCurrrentNotificationID(regID,token, DeviceID);
           // persistantUserData notie;
           // updateCurrrentNotificationID()
        }
    }


    public class ShownNotification {
        public Guid instrumentID;
        public string instrumentDisplayName;
        public List<notificationDisplay> types;
        public int index;

        public Notification buildThis(NotificationCompat.Builder notificationBuilder, PendingIntent pendingIntent) {
            if (this.types.Count == 1) {
                if (this.types.First().bodys.Count == 1) {
                    var display = this.types.First();
                    notificationBuilder.SetContentTitle(display.notifType + " " + this.instrumentDisplayName)
                        .SetSmallIcon(Resource.Drawable.ic_launcher)
                        .SetContentText(display.bodys.First())
                        .SetAutoCancel(true)
                        .SetShowWhen(false)
                        .SetContentIntent(pendingIntent);
                }
            } else {
                Notification.BigTextStyle textStyle = new Notification.BigTextStyle();

                string longTextMessage = "I went up one pair of stairs.";
                longTextMessage += " / Just like me. ";
                textStyle.BigText(longTextMessage);

                textStyle.SetSummaryText("The summary text goes here.");

                notificationBuilder.BigContentView.SetTextViewText(1, longTextMessage);//(textStyle);
            }



            return notificationBuilder.Build();
        }

        public notificationDisplay getByType(string notifType) {
            foreach(notificationDisplay display in this.types) {
                if (display.notifType == notifType) { return display; }
            }
            var next = new notificationDisplay() { notifType = notifType , bodys = new List<string>()};
            this.types.Add(next);
            return next;
        }

        public ShownNotification InstrumentIsIN(List<ShownNotification> notifications) {
            foreach(ShownNotification previous in notifications) {
                if(previous.instrumentID == this.instrumentID) { return previous; }
            }
            return null;
        }
    }
    public class notificationDisplay {
        public string notifType;
        public List<string> bodys;
    }

}