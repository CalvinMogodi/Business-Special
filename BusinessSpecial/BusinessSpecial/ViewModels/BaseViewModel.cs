using BusinessSpecial.Helpers;
using BusinessSpecial.Model;
using BusinessSpecial.Models;
using BusinessSpecial.Services;



namespace BusinessSpecial.ViewModel
{
    public class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        public IDataStore<Item, User, Advert> DataStore => ServiceLocator.Instance.Get<IDataStore<Item, User, Advert>>();

        public bool IsComplete { get; set; }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
    }
}

