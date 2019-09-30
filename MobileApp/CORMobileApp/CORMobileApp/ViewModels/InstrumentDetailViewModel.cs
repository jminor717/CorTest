
using CORMobileApp.Database;

namespace CORMobileApp.ViewModels
{
    public class InstrumentDetailViewModel : BaseViewModel
    {
        public Instrument InstrumentDetail { get; set; }
        public InstrumentDetailViewModel(Instrument item = null)
        {
            item= App.Database.GetInstrumentAsync(item.InternalInstrumentID);
            Title = item.Name;
            InstrumentDetail = item;
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
    }
}