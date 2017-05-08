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
        EditText username, password, displayName, confirmPassword, businessName, registrationNumber, websiteLink, categories;
        TextView message;
        List<string> mSelectedItems;
        AlertDialog categoryDialog;
        string[] items = { "Adventure Or Theme Park", "Art", "Bar, Club Or Pub", "Beauty And Spa", "Cars", "Fashion", "Games", "Health", "Hotel Or Casino", "Investor Or Bank", "Mall Or Shopping Center", "Music And Radio", "Restaurant Or Gas Station", "Software And Technology", "Sport", "Supermarket", "Travel", "Theater", "Wholesale And Hardware" };

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
                Categories = mSelectedItems,
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
            Android.Graphics.Drawables.Drawable icon = Resources.GetDrawable(Resource.Drawable.spam);
            icon.SetBounds(0, 0, icon.IntrinsicWidth, icon.IntrinsicHeight);

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
                    displayName.SetError("This field is required", icon);
                    FormIsValid = false;
                }
            }
            

            if (!validation.IsValidEmail(username.Text))
            {
                username.SetError("Invalid email address", icon);
                FormIsValid = false;
            }

            if (mSelectedItems == null)
            {                
                MessageDialog messageDialog = new MessageDialog();
                messageDialog.SendToast("Please choose atleast one category");
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
            username = FindViewById<EditText>(Resource.Id.signup_etUsername);
            businessName = FindViewById<EditText>(Resource.Id.signup_businessname);
            registrationNumber = FindViewById<EditText>(Resource.Id.signup_registration_number);
            websiteLink = FindViewById<EditText>(Resource.Id.signup_website_link);
            displayName = FindViewById<EditText>(Resource.Id.signup_displayname);
            confirmPassword = FindViewById<EditText>(Resource.Id.signup_confirm_password);
            password = FindViewById<EditText>(Resource.Id.signup_password);
            categories = FindViewById<EditText>(Resource.Id.signup_categories);
            message = FindViewById<TextView>(Resource.Id.signup_tvmessage);
            profilePicture = FindViewById<ImageView>(Resource.Id.signup_profile_picture);
            categories.Touch += Categories_Click;

            userType.Click += OnCheckedChanged;
            profilePicture.Click += SelectLogoButton_Click;
            ViewModel = new SignUpViewModel();
            
            signUpButton.Click += SignUpButton_ClickAsync;

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Choose Your Category");
            builder.SetMultiChoiceItems(Resource.Array.categories_choose, null, delegate { });
            builder.SetNegativeButton("Cancel", delegate { categoryDialog.Cancel(); });
            builder.SetPositiveButton("OK", delegate {
                var sads = categoryDialog.ListView.CheckedItemPositions;
                List<string> selectedItems = new List<string>();
                categories.Text = "";
                for (int i = 0; i < items.Length; i++)
                {
                    var isChecked = sads.Get(i);
                    if (isChecked)
                    {
                        var fd = items.ElementAt(i);
                        selectedItems.Add(fd);
                        if (categories.Text.Length > 30)
                        {
                            categories.Text = categories.Text.Substring(0, 31);
                            categories.Text = categories.Text + "...";
                        }
                        else {
                            if (string.IsNullOrWhiteSpace(categories.Text))
                            {
                                categories.Text = fd;
                            }
                            else
                            {
                                categories.Text = categories.Text + ", " + fd;
                            }
                        }
                        
                    }
                }

                mSelectedItems = selectedItems;
                
                categoryDialog.Cancel();
            });
            categoryDialog = builder.Create();
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

        private void Categories_Click(object sender, System.EventArgs e)
        {            
            categoryDialog.Show();
        }

    }

}