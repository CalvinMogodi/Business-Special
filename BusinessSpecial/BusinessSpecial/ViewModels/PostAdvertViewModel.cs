using BusinessSpecial.Models;
using BusinessSpecial.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecial.ViewModels
{
    public class PostAdvertViewModel : BaseViewModel
    {
         
        public PostAdvertViewModel()
        {
            Title = "Login";
        }

        public async Task LoginUserAsync(Advert advert)
        {
            IsComplete = false;
            var _advert = advert as Advert;
            IsComplete = await DataStore.AddAdvertAsync(_advert);
        }
    }
}
