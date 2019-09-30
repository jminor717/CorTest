using System;
using System.Collections.Generic;
using CORMobileApp.Helpers;
using WindowsAzure.Messaging;
using Android.Util;
using Android.OS;
using Firebase.Iid;
using Android.Content;
using System.Threading.Tasks;

namespace CORMobileApp.Droid {
    class AndroidNotifications : INotificationControl {
        //private FirebaseInstanceId fcm;
        NotificationHub _hub;
        

        public void setHub(object hub) {
            _hub = (NotificationHub)hub;
        }

        public Task registerForTagsAsync(List<string> tags, string token) {

            var noti = App.Database.getCurrentNotificationID();
            //msger.SendRegistrationToServer(noti.token,tags);
            // msger.AddTagsToWatch(tags);
            return null;
        }

        public void unRegisterFromTags(List<string> tags) {
            throw new NotImplementedException();
        }

        public void setContext(Context context) {
            throw new NotImplementedException();
        }
    }
}