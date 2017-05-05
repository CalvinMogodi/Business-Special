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
using Java.IO;
using Android.Util;
using System.IO;

namespace BusinessSpecial.Droid.Activities
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        Button signUpButton; Switch userType; LinearLayout uploadlogo;
        EditText username, password, displayName, confirmPassword, businessName, registrationNumber, websiteLink;
        TextView message;
        public string Logo { get; set; }
        public static readonly int PickImageId = 1000;
        ImageView profilePicture;
        public bool FormIsValid { get; set; }
        public User User { get; set; }
        public SignUpViewModel ViewModel { get; set; }
        public BaseViewModel BaseModel { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Initialize();
        }

        private async void SignUpButton_ClickAsync(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            MessageDialog messageDialog = new MessageDialog();
            messageDialog.ShowLoading();
            message.Text = "";

            var _user = new User()
            {
                Username = username.Text.Trim(),
                Password = password.Text.Trim(),
                Logo = Logo,
                UserTypeId = 2,
            };

            if (userType.Checked)
            {
                _user.UserTypeId = 3;
                _user.DisplayName = businessName.Text.Trim();
                _user.BusinessName = businessName.Text.Trim();
                _user.RegistrationNumber = registrationNumber.Text.Trim();
                _user.WebsiteLink = websiteLink.Text.Trim();
            }
            else
            {
                _user.DisplayName = displayName.Text.Trim();
            }


            await ViewModel.SignUpUser(_user);
            if (ViewModel.IsSignUp)
                StartActivity(new Intent(this, typeof(LoginActivity)));
            else
                message.Text = "Unable to sign you up, please try again";

            messageDialog.HideLoading();
        }


        private bool ValidateForm()
        {
            Validations validation = new Validations();
            Android.Graphics.Drawables.Drawable icon = Resources.GetDrawable(Resource.Drawable.error);
            icon.SetBounds(0, 0, 0, 0);

            FormIsValid = true;

            
            if (userType.Checked)
            {
                if (string.IsNullOrEmpty(Logo))
                {
                    MessageDialog messageDialog = new MessageDialog();
                    messageDialog.SendToast("Select loge");
                    FormIsValid = false;
                }
                if (!validation.IsRequired(businessName.Text))
                {
                    businessName.SetError("This field is required", icon);
                    FormIsValid = false;
                }
                if (!validation.IsRequired(registrationNumber.Text))
                {
                    registrationNumber.SetError("This field is required", icon);
                    FormIsValid = false;
                }
                if (!validation.IsRequired(websiteLink.Text))
                {
                    websiteLink.SetError("This field is required", icon);
                    FormIsValid = false;
                }
            }
            else {
                if (!validation.IsRequired(displayName.Text))
                {
                    username.SetError("This field is required", icon);
                    FormIsValid = false;
                }
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
            userType = FindViewById<Switch>(Resource.Id.signup_usertype);
            uploadlogo = FindViewById<LinearLayout>(Resource.Id.signup_uploadlogo);
            username = FindViewById<EditText>(Resource.Id.signup_etUsername);
            businessName = FindViewById<EditText>(Resource.Id.signup_businessname);
            registrationNumber = FindViewById<EditText>(Resource.Id.signup_registration_number);
            websiteLink = FindViewById<EditText>(Resource.Id.signup_website_link);
            displayName = FindViewById<EditText>(Resource.Id.signup_displayname);
            confirmPassword = FindViewById<EditText>(Resource.Id.signup_confirm_password);
            password = FindViewById<EditText>(Resource.Id.signup_password);            
            message = FindViewById<TextView>(Resource.Id.signup_tvmessage);
            profilePicture = FindViewById<ImageView>(Resource.Id.signup_profile_picture);

            userType.Click += OnCheckedChanged;
            profilePicture.Click += SelectLogoButton_Click;
            ViewModel = new SignUpViewModel();
            
            signUpButton.Click += SignUpButton_ClickAsync;
        }

        private void SelectLogoButton_Click(object sender, EventArgs eventArgs)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                profilePicture.SetImageURI(uri);

                profilePicture.DrawingCacheEnabled = true;

                profilePicture.BuildDrawingCache();

                Android.Graphics.Bitmap bm = profilePicture.GetDrawingCache(true);

                MemoryStream stream = new MemoryStream();
                bm.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, stream);
                byte[] byteArray = stream.ToArray();
               // String img_str = Base64.encodeToString(image, 0);
                Logo = Base64.EncodeToString(byteArray, 0); 
            }
        }


        public void OnCheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = userType.Checked;
            if (isChecked)
            {
                businessName.Visibility = ViewStates.Visible;
                registrationNumber.Visibility = ViewStates.Visible;
                websiteLink.Visibility = ViewStates.Visible;
                displayName.Visibility = ViewStates.Gone;
            }
            else
            {
                businessName.Visibility = ViewStates.Gone;
                registrationNumber.Visibility = ViewStates.Gone;
                websiteLink.Visibility = ViewStates.Gone;
                displayName.Visibility = ViewStates.Visible;
            }
        }

    }

}