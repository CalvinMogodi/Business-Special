using BusinessSpecial.Model;
using BusinessSpecial.Models;
using BusinessSpecial.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecial.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public User User { get; set; }
        public bool IsAuthenticated { get; set; }
        public LoginViewModel()
        {
            Title = "Login";
        }

        public async Task LoginUserAsync(User user)
        {
            var _user = user as User;
            User = await DataStore.LoginUserAsync(_user);

            IsAuthenticated = User != null ? true : false;
        }

        public async Task ChangePasswordAsync(User user)
        {
            var _user = user as User;
            User = await DataStore.ChangePasswordAsync(_user);

            IsAuthenticated = User != null ? true : false;
        }
    }
}
