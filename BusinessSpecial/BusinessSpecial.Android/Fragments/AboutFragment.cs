using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using BusinessSpecial.Droid.Helpers;
using BusinessSpecial.Models;
using BusinessSpecial.ViewModel;
using BusinessSpecial.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System;
using BusinessSpecial.Droid.Activities;
using Android.Graphics;
using Android.Util;

namespace BusinessSpecial.Droid
{
    public class AboutFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {

        public static AboutFragment NewInstance() =>
            new AboutFragment { Arguments = new Bundle() };

        public User User { get; set; }
        AlertDialog CategoryDialog;
        List<string> mSelectedItems;
        TextView displaynameText, businessNameText, registrationnumberText, websitelinkText, wusernameText;
        ImageView categoryImage, profilePicture;
        LinearLayout categoryLinearLayout;
        RelativeLayout displayName, businessName;
        LinearLayout  registrationnumber, websitelink;

        string[] items = { "Adventure Or Theme Park", "Art", "Bar, Club Or Pub", "Beauty And Spa", "Cars", "Fashion", "Games", "Health", "Hotal Or Casino", "Investor Or Bank", "Mall Or Shopping Center", "Music And Radio", "Restaurant Or Gas Station", "Software And Technology", "Sport", "Supermarket", "Travel", "Theater", "Wholesale And Hardware" };
        public ProfileViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            GetUserProfileAsync();

            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        private async void GetUserProfileAsync()
        {
            ViewModel = new ProfileViewModel();

            Context mContext = Android.App.Application.Context;
            AppPreferences appPreferences = new AppPreferences(mContext);
            string userId = appPreferences.getAccessKey();
            ViewModel = new ProfileViewModel();

           User _user = await ViewModel.GetUserProfileAsync(userId);
           bool[] checkedItems = new bool[19];

            if (_user != null)
            {
                if (_user.Categories != null)
                {
                    foreach (var item in _user.Categories)
                    {
                        int position = Array.IndexOf(items, item);
                        checkedItems[position] = true;
                    }
                }
                
                _user.Id = userId;
                User = _user;

                displaynameText.Text = _user.DisplayName;
                businessNameText.Text = _user.BusinessName;
                registrationnumberText.Text = _user.RegistrationNumber;
                websitelinkText.Text = _user.WebsiteLink;
                wusernameText.Text = _user.Username;

                if (!string.IsNullOrEmpty(_user.Logo))
                {
                    Bitmap bitmap = appPreferences.StringToBitMap(_user.Logo);
                    profilePicture.SetImageBitmap(bitmap);
                }

                if (_user.UserTypeId == 3)
                {
                    businessName.Visibility = ViewStates.Visible;
                    registrationnumber.Visibility = ViewStates.Visible;
                    websitelink.Visibility = ViewStates.Visible;
                    displayName.Visibility = ViewStates.Gone;
                }
                else
                {
                    businessName.Visibility = ViewStates.Gone;
                    registrationnumber.Visibility = ViewStates.Gone;
                    websitelink.Visibility = ViewStates.Gone;
                    displayName.Visibility = ViewStates.Visible;
                }
            }

            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Choose Your Category");

            builder.SetMultiChoiceItems(Resource.Array.categories_choose, checkedItems, delegate { });
            builder.SetNegativeButton("Cancel", delegate { CategoryDialog.Cancel(); });
            builder.SetPositiveButton("OK", delegate {
                var sads = CategoryDialog.ListView.CheckedItemPositions;
                List<string> selectedItems = new List<string>();
                for (int i = 0; i < items.Length; i++)
                {
                    var isChecked = sads.Get(i);
                    if (isChecked)
                    {
                        var fd = items.ElementAt(i);
                        selectedItems.Add(fd);
                    }
                }

                mSelectedItems = selectedItems;
                User.Categories = mSelectedItems;
                ViewModel.UpdateUserAsync(User);
                CategoryDialog.Cancel();
            });
            CategoryDialog = builder.Create();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_about, container, false);           
            categoryLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.category_linear_layout);

            profilePicture = view.FindViewById<ImageView>(Resource.Id.viewaccount_profile_picture);
            displayName = view.FindViewById<RelativeLayout>(Resource.Id.displayname);
            registrationnumber = view.FindViewById<LinearLayout>(Resource.Id.registrationnumber);
            websitelink = view.FindViewById<LinearLayout>(Resource.Id.websitelink);
            businessName = view.FindViewById<RelativeLayout>(Resource.Id.businessName);

            displaynameText = view.FindViewById<TextView>(Resource.Id.viewaccount_displayname);
            businessNameText = view.FindViewById<TextView>(Resource.Id.viewaccount_businessname);
            registrationnumberText = view.FindViewById<TextView>(Resource.Id.viewaccount_registrationnumber);
            websitelinkText = view.FindViewById<TextView>(Resource.Id.viewaccount_websitelink);
            wusernameText = view.FindViewById<TextView>(Resource.Id.viewaccount_username);

            
            return view;
        }
        
        public override void OnStart()
        {
            base.OnStart();
            categoryLinearLayout.Click += Categories_Click;
        }
        

        private void Categories_Click(object sender, System.EventArgs e)
        {
            CategoryDialog.Show();            
        }

        public override void OnStop()
        {
            base.OnStop();
            categoryLinearLayout.Click += Categories_Click;
        }

        public void BecameVisible()
        {
        }
    }
}

