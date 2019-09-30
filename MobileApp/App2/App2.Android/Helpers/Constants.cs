using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App2.Droid.Helpers {
    public static class Constants {
        public static string ListenConnectionString { get; set; }  = "Endpoint=sb://cor-test.servicebus.windows.net/;SharedAccessKeyName=secondListen;SharedAccessKey=uya9XveTvC9OxcTJaomB9K7MrIH73hlx9gSB4hEBUbA=";
        public static string NotificationHubName { get; set; } = "COR-test-notifications";
        public static string NotificationChannelName { get; set; } = "XamarinNotifyChannel";
        public static string DebugTag { get; set; } = "XamarinNotify";
        public static string[] SubscriptionTags { get; set; } = { "default" };
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";
    }
}