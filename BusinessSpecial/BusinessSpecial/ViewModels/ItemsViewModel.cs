using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BusinessSpecial.Helpers;
using BusinessSpecial.Model;
using BusinessSpecial.Models;
using System.Collections.Generic;

namespace BusinessSpecial.ViewModel
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Advert> Adverts { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Adverts = new ObservableRangeCollection<Advert>();
            var task =  GetAdvertsAsync();
            //task.Wait();
        }

        public async Task AddItem(Advert advert)
        {
            var _advert = advert as Advert;
            Adverts.Add(_advert);
            await DataStore.AddAdvertAsync(_advert);
        }

        public async Task AddUserActivity(UserActivity activity)
        {
            var _activity = activity as UserActivity;
             DataStore.AddUserActivityAsync(_activity);
        }

        public async Task GetAdvertsAsync()
        {
            //if (IsBusy)
            //    return;

            //IsBusy = true;

            //try
            //{
            // Adverts.Clear();
            Adverts = await DataStore.GetAdvertsAsync();
                //Adverts.ReplaceRange(adverts);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
            //finally
            //{
            //    IsBusy = false;
            //}
        }

        ItemDetailViewModel detailsViewModel;
    }
}