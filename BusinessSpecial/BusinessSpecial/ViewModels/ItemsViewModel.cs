using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BusinessSpecial.Helpers;
using BusinessSpecial.Model;
using BusinessSpecial.Models;

namespace BusinessSpecial.ViewModel
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Advert> Adverts { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Adverts = new ObservableRangeCollection<Advert>();
            var task = ExecuteLoadItemsCommand();
            task.Wait();
        }

        public async Task AddItem(Advert advert)
        {
            var _advert = advert as Advert;
            Adverts.Add(_advert);
            await DataStore.AddAdvertAsync(_advert);
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Adverts.Clear();
                var adverts = await DataStore.GetAdvertsAsync(true);
                Adverts.ReplaceRange(adverts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        ItemDetailViewModel detailsViewModel;
    }
}