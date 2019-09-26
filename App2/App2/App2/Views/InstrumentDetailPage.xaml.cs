
using App2.Database;
using App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.Views
{
    public partial class InstrumentDetailPage : ContentPage
    {
        InstrumentDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public InstrumentDetailPage()
        {
            InitializeComponent();
        }

        public InstrumentDetailPage(InstrumentDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private void Remove_Clicked(object sender, System.EventArgs e)
        {
            MessagingCenter.Send(this, "RemoveInstrument", viewModel.InstrumentDetail);
            Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //int i = 0;
            //foreach (Batch batch in viewModel.InstrumentDetail.batches)
            //{
                //i++;

                //botches.Children.Add(freme);
           // }
            ListView instruments = new ListView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = Color.Black,
                //Padding = new Thickness(15, 5, 5, 15)
                HasUnevenRows = true
                //IsRefreshing = view.IsBusy,
                // ItemSelected = OnItemSelected
            };
            // instruments.IsPullToRefreshEnabled = true;
            //instruments.RefreshCommand = view.LoadItemsCommand;
            
            instruments.ItemSelected += async (object sender, SelectedItemChangedEventArgs args) => {
                ((ListView)sender).SelectedItem = null;
                var item = args.SelectedItem as Batch;
                if (item == null)
                    return;
                // Debug.WriteLine(item.State);
                // await Navigation.PushAsync(new InstrumentDetailPage(new InstrumentDetailViewModel(item)));
                await Task.Delay(10);
                
            };
            instruments.ItemTemplate = new DataTemplate(typeof(CustomCells));
            instruments.ItemsSource = viewModel.InstrumentDetail.batches;
            botches.Children.Add(instruments);
        }
    }
    public class CustomCells : ViewCell
    {
        int count = 0;
        public CustomCells()
        {
            count++;
            var label1 = new Label { LineBreakMode = LineBreakMode.CharacterWrap };
            label1.SetBinding(Label.TextProperty, new Binding(".State", BindingMode.Default, null, null, "batch is {0}"));

            var label2 = new Label { LineBreakMode = LineBreakMode.NoWrap, HorizontalOptions = LayoutOptions.EndAndExpand };
            label2.SetBinding(Label.TextProperty, new Binding (".NumTubes", BindingMode.Default,null,null, "{0,2} tubes"));

            var label3 = new Label {LineBreakMode = LineBreakMode.CharacterWrap, HorizontalOptions = LayoutOptions.EndAndExpand };
            label3.SetBinding(Label.TextProperty, new Binding(".BatchExternalID"));

            View = new StackLayout
            {//Orientation = StackOrientation.Vertical, // HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(2, 5, 2, 5),
                Children = {//{ label1, label2 }
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            label1,label2
                        }
                    },
                    label3
                }
            };

        }
    }
}






/*
new StackLayout {
    Orientation = StackOrientation.Horizontal,
    Children = { label1, label2 }
},
            View = new Frame
            {
                OutlineColor = Color.FromHex("#60abe7"),
                Padding = new Thickness(0, 0, 0, 0),
                Content = stac
            };

    */
