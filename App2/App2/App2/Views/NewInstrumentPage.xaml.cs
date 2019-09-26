using App2.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.Views
{

    public partial class NewInstrumentPage : ContentPage
    {
        public Instrument instrument { get; set; }

        private static readonly HttpClient httpclient = new HttpClient();

        public NewInstrumentPage()
        {
            InitializeComponent();

            instrument = new Instrument
            {
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (instrument.Name == ""|| instrument.Name == null)
            {
                await DisplayAlert("Alert", "Give it a name", "OK");
                return;
            }
            List<string> curreentNames= App.Database.GetNames();
            if (curreentNames.Contains(instrument.Name))
            {
                await DisplayAlert("Alert", "this Name already exists Names must be unique", "OK");
                return;
            }
            //TODO: hash password here, or dont depending on how or if cor is expepecting log in to happen
            //TODO: tyr to conect to COR if conection can be established procede
            //
            try
            {
                //var responseString = await httpclient.GetStringAsync("https://10.0.2.2:7000/signalr//negotiate?clientProtocol=1.5&_=1567610154102");

                //Debug.WriteLine(responseString);
            }
            catch(Exception err)
            {
               //System.Net.Sockets.SocketException

                Debug.WriteLine(err);
            }
            /* 

                       using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                        {
                            client.BaseAddress = new Uri("https://localhost:7000/signalr//negotiate");
                            HttpResponseMessage response = client.GetAsync("").GetAwaiter().GetResult();
                            response.EnsureSuccessStatusCode();
                            string result = response.Content.ReadAsStringAsync().Result;
                            Debug.WriteLine("Result: " + result);
                        }
                        //string data;
                        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:7000/signalr//negotiate?clientProtocol=1.5&_=1567610154102");

                        //request.BeginGetResponse(calback,);

                         using (HttpWebResponse response = (HttpWebResponse)request.BeginGetResponse())
                         using (Stream stream = response.GetResponseStream())
                         using (StreamReader reader = new StreamReader(stream))
                         {
                             data = reader.ReadToEnd();
                         }*/

            //  Debug.WriteLine(data);

            MessagingCenter.Send(this, "AddInstrument", instrument);
            //App.Database.SaveInstrumentAsync(instrument);
            Navigation.PopAsync();
           // await Navigation.PopToRootAsync();
        }


        void calback()
        {

        }
    }
}