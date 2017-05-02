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
using BusinessSpecial.Models;
using Android.Support.Design.Widget;
using BusinessSpecial.ViewModel;
using BusinessSpecial.ViewModels;
using BusinessSpecial.Helpers;

namespace BusinessSpecial.Droid.Activities
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        Button signUpButton;
        EditText username, password, displayName, confirmPassword;
        TextView message;
        public bool FormIsValid { get; set; }
        public User User { get; set; }
        public SignUpViewModel ViewModel { get; set; }
        public BaseViewModel BaseModel { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Initialize();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            message.Text = "";
            var _user = new User(){
                Username = username.Text.Trim(),
                DisplayName = displayName.Text.Trim(),
                Password = password.Text.Trim(),
                UserTypeId = 2,
            };
           

            ViewModel.SignUpUser(_user);
            if (ViewModel.IsSignUp)
                StartActivity(new Intent(this, typeof(LoginActivity)));
            else
                message.Text = "Unable to sign you up, please try again";
        }


        private bool ValidateForm()
        {
            Validations validation = new Validations();
            Android.Graphics.Drawables.Drawable icon = Resources.GetDrawable(Resource.Drawable.error);
            icon.SetBounds(0, 0, 0, 0);

            FormIsValid = true;

            if (!validation.IsRequired(displayName.Text))
            {
                username.SetError("This field is required", icon);
                FormIsValid = false;
            }

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

            if (!validation.IsValidPassword(confirmPassword.Text))
            {
                confirmPassword.SetError("Password cannot be empty and length must be greater than 6 characters", icon);
                FormIsValid = false;
            }

            if (confirmPassword.Text != password.Text)
            {
                confirmPassword.SetError("Password and confirm password don't match", icon);
                FormIsValid = false;
            }

           

            return FormIsValid;
        }

        private void Initialize()
        {
            // Create your application here
            SetContentView(Resource.Layout.activity_sign_up);
            signUpButton = FindViewById<Button>(Resource.Id.button_sign_up);
            username = FindViewById<EditText>(Resource.Id.signup_etUsername);
            displayName = FindViewById<EditText>(Resource.Id.signup_etdisplay_name);
            confirmPassword = FindViewById<EditText>(Resource.Id.signup_confirm_password);
            password = FindViewById<EditText>(Resource.Id.signup_password);            
            message = FindViewById<TextView>(Resource.Id.signup_tvmessage);
            ViewModel = new SignUpViewModel();
            
            signUpButton.Click += SignUpButton_Click;
        }

    }

}