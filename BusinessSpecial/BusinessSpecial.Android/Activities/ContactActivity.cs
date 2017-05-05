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

namespace BusinessSpecial.Droid.Activities
{
    [Activity(Label = "Contact_Activity")]
    public class ContactActivity : Activity
    {
        EditText message;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Initialize();
        }

        private void Initialize()
        {
            // Create your application here
            SetContentView(Resource.Layout.activity_contact_us);
            Button sendButton = FindViewById<Button>(Resource.Id.contact_us_send_button);
            message = FindViewById<EditText>(Resource.Id.contact_us_message);
            sendButton.Click += SendSMSButton_Click;
        }

        void SendSMSButton_Click(object sender, EventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog();
            messageDialog.SendSMS(message.Text.Trim());
            message.Text = "";
        }
    }
}