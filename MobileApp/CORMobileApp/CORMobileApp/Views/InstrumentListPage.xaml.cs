using System;
using CORMobileApp.ViewModels;
using Xamarin.Forms;
using CORMobileApp.Database;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using CORMobileApp.Database.Entities;

namespace CORMobileApp.Views
{
    public partial class InstrumentListPage : ContentPage {
        InstrumentsViewModel viewModel;
        bool selectorBusy = false;
        private static readonly HttpClient client = new HttpClient();
        public InstrumentListPage() {
            InitializeComponent();
            BindingContext = viewModel = new InstrumentsViewModel();
            updateInstrumentList();


        }

        private static readonly HttpClient httpclient = new HttpClient();
        /*
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            //clear the selector
            ItemsListView.SelectedItem = null;
            if (selectorBusy) return;//stop the app from launching mor than one detail page at a time
            selectorBusy = true;
            var item = args.SelectedItem as Instrument;
            if (item == null) { selectorBusy = false; return; }
            await Navigation.PushAsync(new InstrumentDetailPage(new InstrumentDetailViewModel(item)));
            selectorBusy = false;
        }
        //*/

        async void AddItem_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new NewInstrumentPage());
        }
        async void Settings_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new Settings());
        }

        protected override async void OnAppearing() {
            base.OnAppearing();
          /*  Task.Run(async () => {//when the viewmodel is updated the data displayed on the page is changed as well
                await Task.Delay(600);
                viewModel.UpDate();//somehow when adding an instrument it creates two in the view but not the database this syncs them up
            }); */
            await viewModel.UpDate();
            updateInstrumentList();
            try {
                 pingServer();
            }
            catch { }
        }

        private async Task pingServer() {
            try {
                var mee = App.Database.getCurrentNotificationID();
                var body = new {
                    DeviceID = mee.DeviceID
                };
                await App.Https.Post(App.backend + "/api/Users/ping", body);
            }
            catch { }
        }


        private async void updateInstrumentList() {
            Debug.WriteLine("OUTstrumeny");
            instrumentsStack.Children.Clear();
            instrumentsStack.BackgroundColor = App.theme.HomebackGround;
            foreach (Instrument instru in viewModel.SavedInstruments) {
                string sn = instru.InstrumentData.modules[0].InstrumentSerialNumber;

                Debug.WriteLine("instrumeny " + sn);


                Frame insHeader = new Frame {
                    BackgroundColor =App.theme.secondary
                };
                StackLayout fullNmae = new StackLayout {
                    Orientation = StackOrientation.Horizontal,
                    Children = {
                            new Label {Text= instru.Name, LineBreakMode = LineBreakMode.NoWrap,FontSize = 18,FontAttributes=FontAttributes.Bold },

                        }
                };
                foreach (dynamic mod in instru.InstrumentData.modules) {
                    Color ccr = Color.Green;
                    bool on = mod.IsOnline;
                    if (!on) {
                        ccr = Color.Red;
                    }
                    fullNmae.Children.Add(new Label { Text = mod.Type, LineBreakMode = LineBreakMode.NoWrap, FontSize = 18 });
                    fullNmae.Children.Add(new Label { Text = "(" + mod.InstrumentSerialNumber + ") ", LineBreakMode = LineBreakMode.NoWrap, FontSize = 14, TextColor = ccr });

                }
                Button button = new Button {
                    Text = "show low inventory",
                    //VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Start
                };
                //button.Clicked += async (sender, args) => await Navigation.PushAsync(new InstrumentDetailPage(new InstrumentDetailViewModel(instru)));//InvenToryPage
                button.Clicked += async (sender, args) => await Navigation.PushAsync(new InvenToryPage(viewModel, instru));
                StackLayout View = new StackLayout {
                    BackgroundColor = App.theme.HomeCard,//Color.Aqua,//usless collor
                    //Orientation = StackOrientation.Vertical,
                    // HorizontalOptions = LayoutOptions.FillAndExpand,
                    //VerticalOptions = LayoutOptions.StartAndExpand,
                    Padding = new Thickness(15, 5, 5, 15),
                    Children = {//{ label1, label2 }
                        insHeader,
                        fullNmae,
                        button
                    }
                };
                instrumentsStack.Children.Add(View);
            }
        }


        private async void refresh_Clicked(object sender, EventArgs e) {

            try {

                //var responseString = await httpclient.GetStringAsync("https://10.0.2.2:7000/signalr//negotiate?clientProtocol=1.5&_=1567610154102");
                /*
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        // Make request here.   https://10.0.2.2:7000/api/corinventory/1  
                    }
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://10.0.2.2:7000/api/corinventory/0");
                //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var responseString =  await reader.ReadToEndAsync();
                    Debug.WriteLine(responseString);
                }

                
                using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
                {
                    var responseString = await client.GetAsync("https://10.0.2.2:7000/api/corinventory/1");
                    //var responseString = await client.GetAsync("https://google.com");
                    Debug.WriteLine(responseString);

                }
                //*/
                //string jsonData = App.getFakeData();
                //dynamic input = JsonConvert.DeserializeObject(jsonData);// JsonConvert.DeserializeObject(jsonData);
                //JArray modules = input.modules;

                /*
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://10.0.2.2:3303/avalabelInstruments");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().GetAwaiter().GetResult())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream)) {
                    var responseString = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    Debug.WriteLine(responseString);
                    //Debug.WriteLine(responseString);
                }
                */
                //  persistantUserData notie = App.Database.getCurrentNotificationID();
                //    Debug.WriteLine(notie.userID + "  " + notie.registeredID + "  " + notie.token);
                /*
                    var values = new Dictionary<string, string> {
                        { "thing1", "hello" },
                        { "thing2", "world" }
                    };

                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync("https://10.0.2.2:3303/notifictions/register", content);

                    var responseString = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(responseString);
                    //*/


                

                //var str = new List<string> {"alerts","ttt34" };
                //App.msger.registerForTags(str);
                await viewModel.UpDate();

                updateInstrumentList();


            }
            catch (Exception err) {
                //System.Net.Sockets.SocketException     System.IO.IOException

                Debug.WriteLine(err);
            }

        }
    }

}
