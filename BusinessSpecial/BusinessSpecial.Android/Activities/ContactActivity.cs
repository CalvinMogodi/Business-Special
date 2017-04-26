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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Initialize();
        }

        private void Initialize()
        {
            // Create your application here
            SetContentView(Resource.Layout.activity_contact_us);
        }
    }
}