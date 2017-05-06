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
    public class MockDataStore : IDataStore<Item, User, Advert, FAQ, UserActivity>
    {
        bool isInitialized;
        List<Item> items;
        ObservableRangeCollection<Advert> adverts;

        FirebaseClient firebase = new FirebaseClient("https://vert-7c966.firebaseio.com/");
        public async Task<bool> AddItemAsync(Item item)
        {
            await InitializeAsync();

            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<List<FAQ>> GetFAQAsync()
        {
            List<FAQ> faqsList = new List<FAQ>();
            
            try
            {
                var faqs = await firebase.Child("FAQ").OnceAsync<FAQ>();
                faqsList.AddRange(faqs.Select(f => f.Object));

                return faqsList;
            }
            catch (Exception ex)
            {
                return faqsList;
            }
        }
        public async Task<User> GetUserProfileAsync(string userId)
        {
            User user = null;
            var firebase = new FirebaseClient("https://vert-7c966.firebaseio.com/User");
            try
            {
                user = await firebase.Child(userId).OnceSingleAsync<User>();

                return user;
            }
            catch (Exception ex)
            {
                return user;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var firebase = new FirebaseClient("https://vert-7c966.firebaseio.com/User");
            try
            {
                await firebase.Child(user.Id).PutAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<User> ChangePasswordAsync(User user)
        {
            User userDetails = null;
            var firebase = new FirebaseClient("https://vert-7c966.firebaseio.com/");
            try
            {
                var users = await firebase.Child("User").OnceAsync<User>();
                foreach (var item in users)
                {                   
                    if (item.Object.Username.ToLower().Trim() == user.Username.ToLower().Trim())
                    {
                        userDetails = item.Object;
                        userDetails.Id = item.Key;
                        await UpdateUserAsync(userDetails);
                    }
                }
                return userDetails;
            }
            catch (Exception ex) {
                return userDetails;
            }
            
        }

        public async Task<User> LoginUserAsync(User user)
        {
            User userDetails = null;
            List<User> userList = new List<User>();
            adverts = new ObservableRangeCollection<Advert>();
            try
            {
                var users = await firebase.Child("User").OnceAsync<User>();
                foreach (var item in users)
                {
                    item.Object.Id = item.Key;
                    userList.Add(item.Object);
                    if (item.Object.Password == user.Password && item.Object.Username.ToLower().Trim() == user.Username.ToLower().Trim())
                    {
                        userDetails = item.Object;
                    }
                }

                if (userDetails != null)
                {
                    if (userDetails.Categories != null)
                    {
                        DateTime today = DateTime.Now;
                        var advertsList = await firebase.Child("Advert").OnceAsync<Advert>();
                        foreach (var advert in advertsList)
                        {
                            bool addAdvert = userDetails.Categories.Contains(advert.Object.Category);
                            DateTime advertDate = Convert.ToDateTime(advert.Object.StartDate);

                            if (addAdvert && advertDate >= today)
                            {
                             
                                var _user = userList.FirstOrDefault(u => u.Id.Trim() == advert.Object.UserId.Trim());
                                if (_user != null)
                                {
                                    advert.Object.User = _user;
                                    advert.Object.Logo = _user.Logo;
                                    adverts.Add(advert.Object);
                                }
                                
                            }
                        }
                    }
                }
                UserActivity activity = new UserActivity()
                {
                    AdvertCategory = "",
                    SpecialName = "",
                    UserId = userDetails.Id,
                    Date = DateTime.Now.ToString(),
                    ActivityType = "Login",
                    BusinessName = "",
                    BusinessLoge = "",
                    BusinessId = "",
                };
                AddUserActivityAsync(activity);
                return userDetails;
            }
            catch (Exception ex)
            {
                return userDetails;
            }



        }

        public async Task AddUserActivityAsync(UserActivity activity)
        {
            try
            {
                await firebase.Child("UserActivity").PostAsync(activity);
            }
            catch (Exception ex)
            {

            }

        }
        public async Task<bool> SignUpUserAsync(User user)
        {

            await firebase.Child("User").PostAsync(user);

            return await Task.FromResult(true);
        }


        public async Task<bool> AddAdvertAsync(Advert advert)
        {
            var firebase = new FirebaseClient("https://vert-7c966.firebaseio.com/");

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
            var firebase = new FirebaseClient("https://vert-7c966.firebaseio.com/");
            try
            {
                //adverts = new ObservableRangeCollection<Advert>();
                //DateTime today = DateTime.Now;

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
