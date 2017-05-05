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
using Android.Icu.Util;
using BusinessSpecial.Droid.Fragments;
using BusinessSpecial.Helpers;
using BusinessSpecial.Models;
using BusinessSpecial.ViewModels;
using BusinessSpecial.Droid.Helpers;

namespace BusinessSpecial.Droid.Activities
{
    [Activity(Label = "PostAnADActivity")]
    public class PostAnADActivity : Activity
    {
        TextView message;
        Button postButton;
        EditText startDate, specialName, email, startTime, endTime, endDate, location, phone;
        Spinner categories;
        public bool FormIsValid { get; set; }
        private int mHour, mMinute;
        const int startTimeDialog = 0;
        const int endTimeDialog = 1;
        private TimePickerDialog dialog = null;
        public PostAdvertViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Initialize();
        }

        private void Initialize()
        {
            // Create your application here
            SetContentView(Resource.Layout.activity_post_an_ad);
            postButton = FindViewById<Button>(Resource.Id.button_post_an_ad);
            categories = FindViewById<Spinner>(Resource.Id.post_pickup_category);
            specialName = FindViewById<EditText>(Resource.Id.post_etspecial_name);
            startTime = FindViewById<EditText>(Resource.Id.post_start_time);
            endTime = FindViewById<EditText>(Resource.Id.post_end_time);
            startDate = FindViewById<EditText>(Resource.Id.post_start_date);
            endDate = FindViewById<EditText>(Resource.Id.post_end_date);
            email = FindViewById<EditText>(Resource.Id.post_email_address);
            phone = FindViewById<EditText>(Resource.Id.post_phone);
            location = FindViewById<EditText>(Resource.Id.post_location);
            message = FindViewById <TextView>(Resource.Id.post_tvmessage);

            //set drop down
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.categories, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            categories.Adapter = adapter;
            ViewModel = new PostAdvertViewModel();

            postButton.Click += PostButton_ClickAsync;
            startDate.Click += (sender, e) => {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnStartDateSet, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };
            endDate.Click += (sender, e) => {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnEndDateSet, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };
            startTime.Touch += (o, e) => ShowDialog(startTimeDialog);
            endTime.Touch += (o, e) => ShowDialog(endTimeDialog);

        }

        void OnStartDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            startDate.Text = e.Date.ToLongDateString();
        }

        void OnEndDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            endDate.Text = e.Date.ToLongDateString();
        }

        private void StartTimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            string time = string.Format("{0}:{1}", e.HourOfDay, e.Minute.ToString().PadLeft(2, '0'));
            startTime.Text = time;
        }

        private void EndTimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            string time = string.Format("{0}:{1}", e.HourOfDay, e.Minute.ToString().PadLeft(2, '0'));
            endTime.Text = time;
        }
       
        private async void PostButton_ClickAsync(object sender, EventArgs eventArgs)
        {
            message.Text = "";
            if (!ValidateForm())
                return;

            MessageDialog messageDialog = new MessageDialog();
            messageDialog.ShowLoading();

            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);
            string userId = ap.getAccessKey();

            Advert ad = new Advert()
            {
                Category = categories.SelectedItem.ToString(),
                SpecialName = specialName.Text,
                StartTime = startTime.Text,
                EndTime = endTime.Text,
                StartDate = startDate.Text,
                EndDate = endDate.Text,
                Email = email.Text,
                Phone = phone.Text,
                Location = location.Text,
                UserId = userId,
            };
            await ViewModel.AddAdvertAsync(ad);
            if (ViewModel.IsComplete)
            {
                Intent mainIntent = new Intent(this, typeof(MainActivity));
                mainIntent.AddFlags(ActivityFlags.ClearTop);
                mainIntent.AddFlags(ActivityFlags.SingleTop);
                StartActivity(mainIntent);
            }

            else
                message.Text = "Unable to post you advert, please try again!";

            messageDialog.HideLoading();
        }
        
        protected override Dialog OnCreateDialog(int id)
        {
            if (id == startTimeDialog)
                return new TimePickerDialog(this, StartTimePickerCallback, mHour, mMinute, false);

            if (id == endTimeDialog)
                return new TimePickerDialog(this, EndTimePickerCallback, mHour, mMinute, false);

            return null;
        }
        private bool ValidateForm()
        {
            Validations validation = new Validations();
            Android.Graphics.Drawables.Drawable icon = Resources.GetDrawable(Resource.Drawable.spam);
            icon.SetBounds(0, 0, icon.IntrinsicWidth, icon.IntrinsicHeight);

            FormIsValid = true;

            if (categories.SelectedItem.ToString() == "Select Category")
            {
                MessageDialog message = new MessageDialog();
                message.SendToast("Please Select Category");
                FormIsValid = false;
            }

            if (!validation.IsRequired(startDate.Text))
            {
                startDate.SetError("This field is required", icon);
                FormIsValid = false;
            }
            if (!validation.IsRequired(startTime.Text))
            {
                startTime.SetError("This field is required", icon);
                FormIsValid = false;
            }
            if (!validation.IsRequired(endTime.Text))
            {
                endTime.SetError("This field is required", icon);
                FormIsValid = false;
            }
            if (!validation.IsRequired(endDate.Text))
            {
                endDate.SetError("This field is required", icon);
                FormIsValid = false;
            }
            if (!validation.IsValidEmail(email.Text))
            {
                email.SetError("Invalid email address", icon);
                FormIsValid = false;
            }
            if (!validation.IsValidPhone(phone.Text))
            {
                phone.SetError("Invalid mobile number", icon);
                FormIsValid = false;
            }
            if (!validation.IsRequired(specialName.Text))
            {
                specialName.SetError("This field is required", icon);
                FormIsValid = false;
            }
            if (!validation.IsRequired(location.Text))
            {
                location.SetError("This field is required", icon);
                FormIsValid = false;
            }

            return FormIsValid;
        }
        }
}