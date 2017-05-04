using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using BusinessSpecial.Models;
using BusinessSpecial.ViewModels;
using BusinessSpecial.ViewModel;
using BusinessSpecial.Helpers;
using Android.Locations;
using Android;
using Android.Content.PM;
using BusinessSpecial.Droid.Helpers;
using BusinessSpecial.Droid.Activities;

namespace BusinessSpecial.Droid
{
    [Activity(Label = "LoginActivity", LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        Button loginButton;
        EditText username, password;
        TextView message;
        public bool FormIsValid { get; set; }

        public User User { get; set; }
        public LoginViewModel ViewModel { get; set; }
        public BaseViewModel BaseModel { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Initialize();
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;
            FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Visible;
            message.Text = "";
            var _user = new User()
            {
                Username = username.Text,
                Password = password.Text,
            };

            await ViewModel.LoginUserAsync(_user);

            if (ViewModel.IsAuthenticated)
            {
                Intent mainIntent = new Intent(this, typeof(MainActivity));
                mainIntent.AddFlags(ActivityFlags.ClearTop);
                mainIntent.AddFlags(ActivityFlags.SingleTop);

                Context mContext = Android.App.Application.Context;
                AppPreferences ap = new AppPreferences(mContext);
                ap.saveAccessKey(ViewModel.User.Id);

                StartActivity(mainIntent);
            }

            else
                message.Text = "Invaild username or password";

            FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;

        }

        private bool ValidateForm()
        {
            Validations validation = new Validations();
            Android.Graphics.Drawables.Drawable icon = Resources.GetDrawable(Resource.Drawable.error);
            icon.SetBounds(0, 0, 0, 0);

            FormIsValid = true;

            if (!validation.IsValidEmail(username.Text))
            {
                username.SetError("Invalid email address", icon);
                FormIsValid = false;
            }

            if (!validation.IsValidPassword(password.Text))
            {
                password.SetError("Password cannot be empty and length must be greater than 6 characters", icon);
                FormIsValid = false;
            }

            return FormIsValid;
        }

        private void Initialize()
        {

            // Create your application here
            SetContentView(Resource.Layout.activity_login);
            loginButton = FindViewById<Button>(Resource.Id.button_login);
            username = FindViewById<EditText>(Resource.Id.login_txtUsername);
            password = FindViewById<EditText>(Resource.Id.login_txtPassword);
            message = FindViewById<TextView>(Resource.Id.login_tvmessage);
            TextView forgotpassword = FindViewById<TextView>(Resource.Id.login_forgot_password);
            TextView register = FindViewById<TextView>(Resource.Id.login_register);
            ViewModel = new LoginViewModel();
            loginButton.Click += LoginButton_Click;
            forgotpassword.Click += ForgotPassword_Click;
            register.Click += Register_Click;
        }

        public  void Register_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent = new Intent(this, typeof(SignUpActivity));
            StartActivity(intent);
        }

        public void ForgotPassword_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent = new Intent(this, typeof(SignUpActivity));
            StartActivity(intent);
        }
    }
}