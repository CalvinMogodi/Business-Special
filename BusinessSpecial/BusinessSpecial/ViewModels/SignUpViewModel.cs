using BusinessSpecial.Models;
using BusinessSpecial.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecial.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public User User { get; set; }

        public bool IsSignUp { get; set; }

        public SignUpViewModel()
        {
            Title = "Sign Up";
        }

        public async Task SignUpUser(User user)
        {
            var _user = user as User;
            IsSignUp = await DataStore.SignUpUserAsync(_user);
        }
    }
}
