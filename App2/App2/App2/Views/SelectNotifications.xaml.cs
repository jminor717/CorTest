using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectNotifications : ContentPage {
        public SelectNotifications() {
            _states = new List<watchingState>();
            InitializeComponent();
            //scrole.Content = new Label { };
            renderNotifications();
        }

        protected override void OnAppearing() {//after render called
            base.OnAppearing();
           // _states = new List<watchingState>();
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            sendStates();
        }

        private async void sendStates() {
            var mee = App.Database.getCurrentNotificationID();
            List<dynamic> changes = new List<dynamic>();
            foreach(watchingState state in _states) {
                if (state.start != state.finish) {
                    Debug.WriteLine(state.notifivationID);
                    changes.Add(new {
                        ID = state.notifivationID,
                        watchState = state.finish
                    });
                }
            }
            dynamic body = new {
                deviceGuid = mee.DeviceID.ToString(),
                userGuid = mee.userID.ToString(),
                changes = changes
            };
            await App.Https.Post(App.backend + "/api/Notifications/UpDateWatchList", body);

        }

        private List<watchingState> _states;


        private async void renderNotifications() {
            JArray notifications;
            try {
                notifications = await getAvalNotific();
            }
            catch (Exception err) {
                await DisplayAlert("network error", "could not reach server please try again with an active network conection", "OK");
                await Navigation.PopAsync();
                return;
            }
            List<LocalInstrument> instruments = sortInstruments(notifications);
            Thickness padding = new Thickness();
            Device.OnPlatform(() => { padding = new Thickness(10, 10, 10, 10); }, () => { padding = new Thickness(10, 10, 10, 10); });
            Content.BackgroundColor = Color.FromHex("#4c637b");
            scrole.Padding = padding;
            var stack = new StackLayout { };
            foreach (LocalInstrument inst in instruments) {
                var perinstrument = new StackLayout {
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Children = { new Label() { Text = inst.displayName } }
                };
                foreach (Notification notif in inst.notifications) {
                    perinstrument.Children.Add(new BoxView() { Color = Color.Black, WidthRequest = 100, HeightRequest = 2 });
                    perinstrument.Children.Add(makeNotifSwitch(notif));
                }
                stack.Children.Add(new Frame {
                    BackgroundColor = Color.White,
                    Padding = padding,
                    Content = perinstrument
                });
            }
            scrole.Content = stack;
        }

        private List<LocalInstrument> sortInstruments(JArray notifications) {
            List<LocalInstrument> instruments = new List<LocalInstrument>();
            foreach (dynamic notification in notifications) {
                int index = indexOfdynamic(instruments, (int)notification.instrumentID);
                if (index == -1) {
                    instruments.Add(new LocalInstrument() {
                        instrumentID = (int)notification.instrumentID,
                        displayName = (string)notification.originInstrument.displayName,
                        notifications = new List<Notification>() {
                            new Notification() {
                                notifivationID =(int)notification.notifivationID,
                                notificationName =(string)notification.notificationName,
                                notificationDescription =(string)notification.notificationDescription,
                                isWatching = (bool)notification.isWatching
                            }
                        }
                    });
                } else {
                    instruments[index].notifications.Add(new Notification() {
                        notifivationID = (int)notification.notifivationID,
                        notificationName = (string)notification.notificationName,
                        notificationDescription = (string)notification.notificationDescription,
                        isWatching = (bool)notification.isWatching
                    });
                }
            }
            return instruments;
        }

        private int indexOfdynamic(List<LocalInstrument> instruments, int id) {
            if (instruments.Count == 0) { return -1; }
            int index = 0;
            foreach (LocalInstrument instru in instruments) {
                if (instru.instrumentID == id) { return index; }
                index++;
            }
            return -1;
        }

        private View makeNotifSwitch(Notification notification) {
            var state = new watchingState() {
                start = notification.isWatching,
                finish = notification.isWatching,
                notifivationID = notification.notifivationID
            };
            _states.Add(state);
            var btn = new ButtonWinfo() {
                // Text = "watch",
                IsToggled = notification.isWatching,
                HorizontalOptions = LayoutOptions.End,
                _notification = notification,
                _thisState = state
            };
            btn.Toggled += tog;
            //btn.Clicked += watch(notification.notifivationID);
            return new StackLayout {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Children = {
                    new Label() { Text=notification.notificationName},
                    btn
                }
            };
        }

        private async void tog(object sender, EventArgs e) {
            var temp = (sender as ButtonWinfo);
            if (temp != null) {
                temp._thisState.findThis(_states).finish = temp.IsToggled;
            }
        }

        private async void addToWatchList(Notification notification) {
            var mee = App.Database.getCurrentNotificationID();
            if (mee == null) { return; }//user is not registered for notifications
            var body = new {
                notifivationID = notification.notifivationID,
                deviceGuid = mee.DeviceID.ToString(),
                userGuid = mee.userID.ToString()
            };
            try {
                await App.Https.Post(App.backend + "/api/Notifications/watch", body);
            }
            catch (Exception err) {
                Debug.WriteLine(err);
            }
        }

        public async Task<dynamic> getAvalNotific() {
            var body = new {
                UUID = App.Database.getCurrentNotificationID().userID
            };
            string returned = await App.Https.Post(App.backend + "/api/Notifications", body);
            dynamic ret = JsonConvert.DeserializeObject(returned);
            return ret;
        }
    }


    public class LocalInstrument {
        public int instrumentID;
        public string displayName;
        public List<Notification> notifications;
    }
    public class Notification {
        public int notifivationID;
        public string notificationName;
        public string notificationDescription;
        public bool isWatching;
    }

    public class watchingState {
        public int notifivationID;
        public bool start;
        public bool finish;

        public watchingState findThis(List<watchingState> findIn) {
            foreach(watchingState state in findIn) {
                if(state.notifivationID == this.notifivationID) { return state; }
            }
            return null;
        }

    }

    public class ButtonWinfo : Switch {
        public Notification _notification;
        public watchingState _thisState;
    }
}