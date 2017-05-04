using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BusinessSpecial.Model;
using BusinessSpecial.Models;
using Firebase.Xamarin.Database;
using BusinessSpecial.Helpers;

namespace BusinessSpecial.Services
{
    public class MockDataStore : IDataStore<Item, User, Advert, FAQ>
    {
        bool isInitialized;
        List<Item> items;
        ObservableRangeCollection<Advert> adverts;


        public async Task<bool> AddItemAsync(Item item)
        {
            await InitializeAsync();

            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<List<FAQ>> GetFAQAsync()
        {
            List<FAQ> faqsList = new List<FAQ>();
            var firebase = new FirebaseClient("https://courierrequest-6a586.firebaseio.com/");
            try
            {
                var faqs = await firebase.Child("FAQ").OnceAsync<FAQ>();
                faqsList.AddRange(faqs.Select(f => f.Object));

                return faqsList;
            }
            catch (Exception ex) {
                return faqsList;
            }
        }
        public async Task<User> LoginUserAsync(User user)
        {
            User userDetails = null;
            List<User> userList =new List<User>();
            var firebase = new FirebaseClient("https://courierrequest-6a586.firebaseio.com/");
            try
            {               

                var users = await firebase.Child("User").OnceAsync<User>();
                foreach (var item in users)
                {
                    item.Object.Id = item.Key;
                    userList.Add(item.Object);
                    if (item.Object.Password == user.Password && item.Object.Username == user.Username)
                    {
                        userDetails = item.Object;

                        DateTime today = DateTime.Now;
                        var advertsList = await firebase.Child("Advert").OnceAsync<Advert>();
                        foreach (var advert in advertsList)
                        {
                            DateTime advertDate = Convert.ToDateTime(advert.Object.StartDate);
                            if (advertDate >= today)
                            {
                                adverts.Add(advert.Object);

                                foreach (var _advert in adverts)
                                {
                                    _advert.User = userList.FirstOrDefault(u => u.Id.Trim() == _advert.UserId.Trim());
                                }
                            }
                        }
                    }
                }

                return userDetails;
            }
            catch (Exception ex)
            {
                return userDetails;
            }
            

            
        }

        public async Task<bool> SignUpUserAsync(User user)
        {
            var firebase = new FirebaseClient("https://courierrequest-6a586.firebaseio.com/");

            await firebase.Child("User").PostAsync(user);

            return await Task.FromResult(true);
        }


        public async Task<bool> AddAdvertAsync(Advert advert)
        {
            var firebase = new FirebaseClient("https://courierrequest-6a586.firebaseio.com/");

            await firebase.Child("Advert").PostAsync(advert);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            await InitializeAsync();

            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            await InitializeAsync();

            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public async Task<ObservableRangeCollection<Advert>> GetAdvertsAsync()
        {
            var firebase = new FirebaseClient("https://courierrequest-6a586.firebaseio.com/");
            try
            {
                adverts = new ObservableRangeCollection<Advert>();
                DateTime today = DateTime.Now;

                //var advertsList = await firebase.Child("Advert").OnceAsync<Advert>();
                //foreach (var item in advertsList)
                //{
                //    DateTime advertDate = Convert.ToDateTime(item.Object.StartDate);
                //    if (advertDate >= today)
                //    {
                //        adverts.Add(item.Object);
                //    }
                //}
                return await Task.FromResult(adverts);
            }
            catch (Exception ex)
            {
                return adverts;
            }

           
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;

            items = new List<Item>();
            var _items = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "Buy some cat food", Description="The cats are hungry"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Learn F#", Description="Seems like a functional idea"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Learn to play guitar", Description="Noted"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Buy some new candles", Description="Pine and cranberry for that winter feel"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Complete holiday shopping", Description="Keep it a secret!"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Finish a todo list", Description="Done"},
            };

            foreach (Item item in _items)
            {
                items.Add(item);
            }

            isInitialized = true;
        }
    }
}
