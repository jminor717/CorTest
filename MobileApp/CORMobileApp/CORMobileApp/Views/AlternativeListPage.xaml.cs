using CORMobileApp.Database;
using CORMobileApp.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CORMobileApp.Views
{
    public partial class AlternativeListPage : ContentPage
    {
        long lastRefreshed;
        int i = 0;
        public AlternativeListPage()
        {
            InitializeComponent();
            SetEntriesLayout();
        }
        public void SetEntriesLayout()
        {
            lastRefreshed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            Button btn = new Button { Text = "hi" + i.ToString() };
            btn.Clicked += Btn_Clicked;
            EntryFieldsStack.Children.Add(btn);
            List<Instrument> inss = App.Database.GetInstrumentsAsync().GetAwaiter().GetResult();
            ListView instruments = new ListView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = Color.Black
                //Padding = new Thickness(15, 5, 5, 15)
                // HasUnevenRows = true,
                //IsRefreshing = view.IsBusy,
                // ItemSelected = OnItemSelected
            };
            // instruments.IsPullToRefreshEnabled = true;
            //instruments.RefreshCommand = view.LoadItemsCommand;

            instruments.ItemSelected += async (object sender, SelectedItemChangedEventArgs args) => {
                var item = args.SelectedItem as Instrument;
                if (item == null)
                    return;
                item.Name += "A";
                App.Database.SaveInstrumentAsync(item);
                await Navigation.PushAsync(new InstrumentDetailPage(new InstrumentDetailViewModel(item)));
                ((ListView)sender).SelectedItem = null;
            };
            instruments.ItemTemplate = new DataTemplate(typeof(CustomCell2));
            //instruments.HeightRequest = 50;
            //Padding = new Thickness(0, 20, 0, 0);
            instruments.ItemsSource = inss;
            EntryFieldsStack.Children.Add(instruments);
            Content = instruments;

            Entry entry2 = new Entry { HorizontalOptions = LayoutOptions.FillAndExpand };
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (now - lastRefreshed < 1000)
            {
                return;
            }
            i++;
            InitializeComponent();
            SetEntriesLayout();
        }
    }
    public class CustomCell2 : ViewCell
    {
        public CustomCell2()
        {
            var label1 = new Label
            {
                LineBreakMode = LineBreakMode.NoWrap
                //Text = "Label 1",
                // FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                // FontAttributes = FontAttributes.Bold
            };
            label1.SetBinding(Label.TextProperty, new Binding(".Name"));

            var label2 = new Label
            {
                LineBreakMode = LineBreakMode.NoWrap
                //Text = "Label 2",
                // FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };
            label2.SetBinding(Label.TextProperty, new Binding(".Adress"));
            label2.HorizontalOptions = LayoutOptions.EndAndExpand; // = TextAlignment.End;

            View = new StackLayout
            {
                BackgroundColor = Color.Aqua,//usless collor
                                             //Orientation = StackOrientation.Vertical,
                                             // HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(15, 5, 5, 15),
                Children = {//{ label1, label2 }
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = { label1, label2 }
                    },
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = { new Label {Text= "batches", LineBreakMode = LineBreakMode.NoWrap } }
                    }
                }
            };
        }
    }
}
