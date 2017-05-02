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

            postButton.Click += PostButton_Click;
            startDate.Touch += StartDateSelect_OnClick;
            endDate.Touch += EndDateSelect_OnClick;
            startTime.Touch += (o, e) => ShowDialog(startTimeDialog);
            endTime.Touch += (o, e) => ShowDialog(endTimeDialog);

        }

        public void VehiclebodytypeItemSelected(object sender, EventArgs e)
        {
            var selctedItem = categories.SelectedItem;

            if (selctedItem.Equals("Motorcycle") || selctedItem.Equals("Passenger") || selctedItem.Equals("Select Vehicle Body Type"))
            {
                
            }
            else
            {
               
            }
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
        private void StartDateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                startDate.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void PostButton_Click(object sender, EventArgs eventArgs) {
            message.Text = "";
            if (!ValidateForm())
                return;

            Advert ad = new Advert() {
               Category = categories.SelectedItem.ToString(),
                SpecialName = specialName.Text,
                StartTime = startTime.Text ,
                EndTime = endTime.Text,
                StartDate = startDate.Text,
                EndDate = endDate.Text,
                Email = email.Text,
                Phone = phone.Text,
                Location = location.Text,
        };
            if (ViewModel.IsComplete)
            {
                Intent mainIntent = new Intent(this, typeof(MainActivity));
                mainIntent.AddFlags(ActivityFlags.ClearTop);
                mainIntent.AddFlags(ActivityFlags.SingleTop);
                StartActivity(mainIntent);
            }

            else
                message.Text = "Unable to post you advert, please try again!";

        }

        private void EndDateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                endDate.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
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
            Android.Graphics.Drawables.Drawable icon = Resources.GetDrawable(Resource.Drawable.error);
            icon.SetBounds(0, 0, 0, 0);

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
                email.SetError("Invalid mobile number", icon);
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