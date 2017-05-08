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
        TextView displaynameText, category, registrationnumberText, websitelinkText, wusernameText;
        ImageView profilePicture;
        LinearLayout registrationnumber, websitelink;

        string[] items = { "Adventure Or Theme Park", "Art", "Bar, Club Or Pub", "Beauty And Spa", "Cars", "Fashion", "Games", "Health", "Hotel Or Casino", "Investor Or Bank", "Mall Or Shopping Center", "Music And Radio", "Restaurant Or Gas Station", "Software And Technology", "Sport", "Supermarket", "Travel", "Theater", "Wholesale And Hardware" };
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
                        if (position != -1)
                        {
                            checkedItems[position] = true;
                        }
                        
                    }
                }

                _user.Id = userId;
                User = _user;

                displaynameText.Text = String.Format("{0}", _user.DisplayName);
                registrationnumberText.Text = String.Format("{0}", _user.RegistrationNumber);
                websitelinkText.Text = String.Format("{0}", _user.WebsiteLink);
                wusernameText.Text = String.Format("{0}", _user.Username);

                if (!string.IsNullOrEmpty(_user.Logo))
                {
                    Bitmap bitmap = appPreferences.StringToBitMap(_user.Logo);
                    profilePicture.SetImageBitmap(bitmap);
                }

                if (_user.UserTypeId == 3)
                {
                    registrationnumberText.Visibility = ViewStates.Visible;
                    websitelinkText.Visibility = ViewStates.Visible;
                    registrationnumber.Visibility = ViewStates.Visible;
                    websitelink.Visibility = ViewStates.Visible;
                }
                else
                {
                    registrationnumberText.Visibility = ViewStates.Gone;
                    websitelinkText.Visibility = ViewStates.Gone;
                    registrationnumber.Visibility = ViewStates.Gone;
                    websitelinkText.Visibility = ViewStates.Gone;
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
            profilePicture = view.FindViewById<ImageView>(Resource.Id.viewaccount_profile_picture);

            
            displaynameText = view.FindViewById<TextView>(Resource.Id.viewaccount_displayname);
            registrationnumberText = view.FindViewById<TextView>(Resource.Id.viewaccount_registrationnumber);
            websitelinkText = view.FindViewById<TextView>(Resource.Id.viewaccount_websitelink);
            registrationnumber = view.FindViewById<LinearLayout>(Resource.Id.viewaccount_registrationnumberLL);
            websitelink = view.FindViewById<LinearLayout>(Resource.Id.viewaccount_websitelinkLL);
            category = view.FindViewById<TextView>(Resource.Id.category_text);
            wusernameText = view.FindViewById<TextView>(Resource.Id.viewaccount_username);


            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            category.Click += Categories_Click;
        }


        private void Categories_Click(object sender, System.EventArgs e)
        {
            CategoryDialog.Show();
        }

        public override void OnStop()
        {
            base.OnStop();
            ///categoryLinearLayout.Click += Categories_Click;
        }

        public void BecameVisible()
        {
        }
    }
}

