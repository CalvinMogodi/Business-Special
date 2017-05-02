using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BusinessSpecial.Model;
using BusinessSpecial.Models;
using Firebase.Xamarin.Database;

namespace BusinessSpecial.Services
{
    public class MockDataStore : IDataStore<Item, User, Advert>
    {
        bool isInitialized;
        List<Item> items;


        public async Task<bool> AddItemAsync(Item item)
        {
            await InitializeAsync();

            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<User> LoginUserAsync(User user)
        {
            User userDetails = null;
            var firebase = new FirebaseClient("https://courierrequest-6a586.firebaseio.com/");
            try
            {
                var users = await firebase.Child("User").OnceAsync<User>();
                foreach (var item in users)
                {
                    if (item.Object.Password == user.Password && item.Object.Username == user.Username)
                    {
                        userDetails = item.Object;
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
