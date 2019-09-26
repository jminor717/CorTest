using System;
using System.Collections.Generic;
using App2.Helpers;
using WindowsAzure.Messaging;
using Android.Util;
using Android.OS;
using Firebase.Iid;
using Android.Content;

namespace App2.Droid {
    class AndroidNotifications : INotificationControl {
        //private FirebaseInstanceId fcm;
        NotificationHub _hub;
        

        public void setHub(object hub) {
            _hub = (NotificationHub)hub;
        }

        public void registerForTags(List<string> tags) {

            var noti = App.Database.getCurrentNotificationID();
            //msger.SendRegistrationToServer(noti.token,tags);
            // msger.AddTagsToWatch(tags);

        }

        public void unRegisterFromTags(List<string> tags) {
            throw new NotImplementedException();
        }

    }
}