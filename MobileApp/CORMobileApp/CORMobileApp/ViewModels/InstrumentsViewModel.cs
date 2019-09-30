using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CORMobileApp.Helpers;
using CORMobileApp.Views;

using Xamarin.Forms;
using CORMobileApp.Database;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CORMobileApp.ViewModels
{
    public class InstrumentsViewModel : BaseViewModel
    {
        public ObservableCollection<Instrument> SavedInstruments { get; set; }
        public Command LoadItemsCommand { get; set; }

        public InstrumentsViewModel()
        {
            //Title = "Browse";
            SavedInstruments = new ObservableCollection<Instrument>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewInstrumentPage, Instrument>(this, "AddInstrument", async (obj, item) => 
            {
                Instrument _item = item as Instrument;
                App.Database.SaveInstrumentAsync(_item);
                /*
                _item = App.Database.GetInstrumentAsync(_item.Name);
                Batch tem = new Batch();
                //_item.batches = tem.GenerateRandomBatches(_item.InternalInstrumentID,6);
                foreach (Batch batch in _item.batches)
                {
                    if(batch.State == "completed"){ _item.CompletedBatches++; }
                    else { _item.UnfinisedBatches++; }
                }
                await App.Database.SaveInstrumentAsync(_item);
                //await App.Database.AddBatches(_item.batches);
                */
                SavedInstruments.Add(_item);
                //await DataStore.AddItemAsync(_item);
            });

            MessagingCenter.Subscribe<InstrumentDetailPage, Instrument>(this, "RemoveInstrument", async (obj, item) =>
            {
                Instrument _item = item as Instrument;
                SavedInstruments.Remove(_item);
                await App.Database.DeleteInstrumentAsync(_item);
                
                //await DataStore.AddItemAsync(_item);
            });
        }

        public async Task UpDate()
        {
            await ExecuteLoadItemsCommand();
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                SavedInstruments.Clear();
                // var items = await DataStore.GetItemsAsync(true);
                var items = await App.Database.GetInstrumentsAsync();
                foreach(Instrument item in items)
                {
                    string jsonData = App.getFakeData();
                    dynamic input = JsonConvert.DeserializeObject(jsonData);
                    item.InstrumentData = input;//////////////////TODO: update this when the http requests work
                    SavedInstruments.Add(item);
                }

                //SavedInstruments.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}