using BusinessSpecial.Models;
using BusinessSpecial.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecial.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public User User { get; set; }
        public ProfileViewModel()
        {
            Title = "Profile";
        }

        public async Task GetUserProfileAsync(string userId)
        {
            User = await DataStore.GetUserProfileAsync(userId);

            IsComplete = User != null ? true : false;
        }
    }
}
