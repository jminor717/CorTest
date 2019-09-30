using App2.Database;
using App2.Database.Entities;
using App2.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Xamarin.Forms;
//using Xamarin.Essentials;

namespace App2.Views
    
{
    public partial class Settings : ContentPage {
        long lastRefreshed;
        int i = 0;
        public Settings() {
            InitializeComponent();
            SetEntriesLayout();
        }
        public void SetEntriesLayout() {
            EntryFieldsStack.Children.Clear();
            lastRefreshed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            Button btn = new Button { Text = "create new user" + i.ToString() };
            btn.Clicked += Btn_Clicked;
            EntryFieldsStack.Children.Add(btn);

            Button btn2 = new Button { Text = "select notifications to recieve" };
            btn2.Clicked += GO_TONotifications;
            EntryFieldsStack.Children.Add(btn2);

            Button btn3 = new Button { Text = "change tags" };
            btn3.Clicked += Btn3_Clicked;
            EntryFieldsStack.Children.Add(btn3);

            Button btn4 = new Button { Text = "login" };
            btn4.Clicked += Btn4_Clicked;
            EntryFieldsStack.Children.Add(btn4);

            SettingsStorage setts = App.Database.GetSettings();
            List<PropertyInfo> setting_properties = typeof(SettingsStorage).GetRuntimeProperties().OrderBy(x => x.Name).ToList();
            //string current_catagory = "";
            foreach (PropertyInfo setting_type in setting_properties) {
                //([a-z,A-Z,0-9]*)(?:_)([a-z,A-Z,0-9]*)
                Regex rx = new Regex(@"\b([a-z,A-Z,0-9]*)(?:_)([a-z,A-Z,0-9]*)\b", RegexOptions.IgnoreCase);
                string cat = "", name = "";
                MatchCollection matches = rx.Matches(setting_type.Name);
                if (matches[0].Groups.Count == 3) {
                    cat = matches[0].Groups[1].ToString();
                    name = matches[0].Groups[2].ToString();
                    Debug.WriteLine(cat + " " + name);
                    if (cat == "Inernal") { continue; }
                    //
                }
                // Report the number of matches found.
                //Debug.WriteLine("{0} matches found in:\n   {1}\n   {2}\n   {3}", matches[0].Groups.Count, matches[0].Groups[0], );
            }
            Entry entry2 = new Entry { HorizontalOptions = LayoutOptions.FillAndExpand };
        }


        private void AddToCatagory<T>(string catagory, string name) {
            if (typeof(T) == typeof(bool)) {

            }
        }

        private void Btn3_Clicked(object sender, EventArgs e) {
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (now - lastRefreshed < 1000) {
                return;
            }
            persistantUserData mee = App.Database.getCurrentNotificationID();
            App.msger.registerForTagsAsync(new List<string>() { "HI" }, mee.token);

        }

        private void Btn_Clicked(object sender, EventArgs e) {
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (now - lastRefreshed < 1000) {
                return;
            }
            persistantUserData mee = App.Database.getCurrentNotificationID();
            string userName = mee.UserName;
            bool newuse = false;
            Random rnd = new Random();
            mee.UserName = "bob" + rnd.NextDouble().ToString();
            string OsString = Device.RuntimePlatform;
            switch (Device.RuntimePlatform) {
                case (Device.iOS): { OsString = "iOS"; break; }
                case (Device.Android): { OsString = "Android"; break; }
            }
            object obj = new {
                login = new {
                    userName = mee.UserName,
                    pwd = "passw04d1",
                },
                //uuid = mee.userID,
                devices = new object[] { new {
                    DeviceID =mee.DeviceID,
                    notificationHubRegistration=mee.DeviceID,
                    Platform = OsString
                } },
            };


            App.Https.Post(App.backend + "/api/Users/create", obj).ContinueWith(async (returned) => {
                dynamic ret = JsonConvert.DeserializeObject(await returned);
                Debug.WriteLine((object)ret);
                Guid thing = new Guid((string)ret.uuid);

                await App.Database.remoteAlerted(thing);
            });
            _ = App.Database.updateUserName(mee.UserName);





            i++;
            //InitializeComponent();
            SetEntriesLayout();

        }

        private void Btn4_Clicked(object sender, EventArgs e) {
            persistantUserData mee = App.Database.getCurrentNotificationID();
            object obj = new {
                login = new {
                    userName = mee.UserName,
                    pwd = "passw04d1",
                },
                //uuid = mee.userID,
                devices = new object[] { new {
                    DeviceID =mee.DeviceID,
                    notificationHubRegistration=mee.DeviceID,
                } },
            };
            App.Https.Post(App.backend + "/api/Users/loin", obj).ContinueWith(async (returned) => {
                dynamic ret = JsonConvert.DeserializeObject(await returned);
                Debug.WriteLine((object)ret);

            });
        }


        async void GO_TONotifications(object sender, EventArgs e) {
            await Navigation.PushAsync(new SelectNotifications());
        }
    }
}
