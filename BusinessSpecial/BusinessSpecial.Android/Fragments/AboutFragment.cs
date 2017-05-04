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

namespace BusinessSpecial.Droid
{
    public class AboutFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {

        public static AboutFragment NewInstance() =>
            new AboutFragment { Arguments = new Bundle() };

        User User;
        AlertDialog CategoryDialog;
        List<string> mSelectedItems;
       
        string[] items = {  "Adventure Or Theme Park",    "Art",    "Bar, Club Or Pub",    "Beauty And Spa",    "Cars",    "Fashion",    "Games",    "Health",    "Hotal Or Casino",    "Investor Or Bank",    "Mall Or Shopping Center",    "Music And Radio",    "Restaurant Or Gas Station",    "Software And Technology",    "Sport",    "Supermarket",    "Travel","Theater","Wholesale And Hardware"};
        public ProfileViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            GetUserProfileAsync();

            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        private async void GetUserProfileAsync()
        {
            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);
            string userId = ap.getAccessKey();
            ViewModel = new ProfileViewModel();

            await ViewModel.GetUserProfileAsync(userId);
            User = ViewModel.User;
        }

        Button learnMoreButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_about, container, false);
            ViewModel = new ProfileViewModel();
            learnMoreButton = view.FindViewById<Button>(Resource.Id.button_learn_more);

            
            return view;
        }

       

        public override void OnStart()
        {
            base.OnStart();
            learnMoreButton.Click += LearnMoreButton_Click;
        }

        private void LearnMoreButton_Click(object sender, System.EventArgs e)
        {
            bool[] checkedItems = new bool[18];
             
            foreach (var item in User.Categories)
            {
                int position = Array.IndexOf(items, item);
                checkedItems[position] = true;
            }
           
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Choose Your Category");

            builder.SetMultiChoiceItems(Resource.Array.categories_choose, checkedItems, delegate {


            });
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
                CategoryDialog.Cancel();
            });
            CategoryDialog = builder.Create();
            CategoryDialog.Show();
        }

        public override void OnStop()
        {
            base.OnStop();
            learnMoreButton.Click -= LearnMoreButton_Click;
        }

        public void BecameVisible()
        {
        }
    }
}

