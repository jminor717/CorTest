using Microsoft.Azure.NotificationHubs;
using MobileBackend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MobileBackend.AzureControlers {



    public class FCMnotificationTemplate {
        public data data=new data();
    }
    public class data {
        public string message = "";
       // public string Title = "";
    }

    public class AzureNotifications {
        private static NotificationHubClient notificationhub;

        private const string FcmSampleNotificationContent = "{\"data\":{\"message\":\"Notification Hub test notification from SDK sample\"}}";
        private const string AppleSampleNotificationContent = "{\"aps\":{\"alert\":\"Notification Hub test notification from SDK sample\"}}";
        //{"data":{"message":"{ title:test notification,body:test Body,}"}}
        public AzureNotifications(string conectionstring, string hubname) {
            notificationhub = NotificationHubClient.CreateClientFromConnectionString(conectionstring, hubname);
        }

        public async void sendToUsersbyID( List<device> devices, RaisedNotification incoming, Instrument source ) {
            FCMnotificationTemplate androidnoty = new FCMnotificationTemplate();
            androidnoty.data.message = "{ notiType:" + incoming.notificationType + ",body:" + incoming.Name + ",UUID:" + source.UUID.ToString() + ",name:" + source.DisplayName + ",}";//title + " ;; " + 
            //androidnoty.data2.title = title;
            string androidString = JsonConvert.SerializeObject(androidnoty);//"{\"data\":{\"message\":\"{ \"title\":" + title + ",\"body\":" + body + "}\"}}";
            List<string> androidDevices = new List<string>();
            List<string> iOSDevices = new List<string>();
            foreach (device dev in devices) {
                if (dev.Platform == "Android") {
                    androidDevices.Add("device:" + dev.notificationHubRegistration);
                } else if (dev.Platform == "iOS") {
                    iOSDevices.Add("device:" + dev.notificationHubRegistration);
                }

            }
            if (androidDevices.Count > 0) {///////////////MAX  20 tags
              // var outcomeFcmByTag = await notificationhub.SendFcmNativeNotificationAsync(androidString, androidDevices);//JsonConvert.SerializeObject(androidnoty)
                // var fcmTagOutcomeDetails = await WaitForThePushStatusAsync("FCM Tags", notificationhub, outcomeFcmByTag);
                //PrintPushOutcome("FCM Tags", fcmTagOutcomeDetails, fcmTagOutcomeDetails.FcmOutcomeCounts);
            }
            if (iOSDevices.Count > 0) {
                var outcomeApnsByTag = await notificationhub.SendAppleNativeNotificationAsync(AppleSampleNotificationContent, iOSDevices);
                // var apnsTagOutcomeDetails = await WaitForThePushStatusAsync("APNS Tags", notificationhub, outcomeApnsByTag);
                //PrintPushOutcome("APNS Tags", apnsTagOutcomeDetails, apnsTagOutcomeDetails.ApnsOutcomeCounts);
            }
        }

        private static async Task<NotificationDetails> WaitForThePushStatusAsync(string pnsType, NotificationHubClient nhClient, NotificationOutcome notificationOutcome) {
            var notificationId = notificationOutcome.NotificationId;
            var state = NotificationOutcomeState.Enqueued;
            var count = 0;
            NotificationDetails outcomeDetails = null;
            while ((state == NotificationOutcomeState.Enqueued || state == NotificationOutcomeState.Processing) && ++count < 10) {
                try {
                    Console.WriteLine($"{pnsType} status: {state}");
                    outcomeDetails = await nhClient.GetNotificationOutcomeDetailsAsync(notificationId);
                    state = outcomeDetails.State;
                }
                catch (Exception) {
                    // It's possible for the notification to not yet be enqueued, so we may have to swallow an exception
                    // until it's ready to give us a new state.
                }
                Thread.Sleep(1000);
            }
            return outcomeDetails;
        }
    }
}
