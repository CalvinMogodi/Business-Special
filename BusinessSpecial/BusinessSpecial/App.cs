using BusinessSpecial.Helpers;
using BusinessSpecial.Interfaces;
using BusinessSpecial.Services;
using BusinessSpecial.Model;
using BusinessSpecial.Models;

namespace BusinessSpecial
{
    public partial class App
    {
        public App()
        {
        }

        public static void Initialize()
        {
            ServiceLocator.Instance.Register<IDataStore<Item, User, Advert>, MockDataStore>();
        }
    }
}