using System;
using Android.App;
using Android.Widget;
using BusinessSpecial.Interfaces;
using Plugin.CurrentActivity;
using Android.Telephony;
using System.Collections.Generic;

namespace BusinessSpecial.Droid
{
    public class MessageDialog : IMessageDialog
    {
        ProgressDialog progress;
        public void ShowLoading()
        {
            var activity = CrossCurrentActivity.Current.Activity;
            progress = new ProgressDialog(activity, Resource.Style.MyTheme);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading...");
            progress.SetCancelable(false);
           // progress.Create();
            progress.Show();
        }
        public void HideLoading()
        {
            progress.Cancel();
        }

        public void SendSMS(string message)
        {
            string[] mobileNumbers = { "0731697774", "0611230358" };
            foreach (var mobileNumber in mobileNumbers)
            {
                SmsManager.Default.SendTextMessage(mobileNumber, null, message, null, null);
            }
           
            SendToast("Message is sent sucessfully.");
        }

        public void SendMessage(string message, string title = null)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            var builder = new AlertDialog.Builder(activity);
            builder
                .SetTitle(title ?? string.Empty)
                .SetMessage(message)
                .SetPositiveButton(Android.Resource.String.Ok, delegate
                {

                });

            activity.RunOnUiThread(() =>
            {
                AlertDialog alert = builder.Create();
                alert.Show();
            });
        }


        public void SendToast(string message)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            activity.RunOnUiThread(() =>
            {
                Toast.MakeText(activity, message, ToastLength.Long).Show();
            });

        }


        public void SendConfirmation(string message, string title, Action<bool> confirmationAction)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            var builder = new AlertDialog.Builder(activity);
            builder
            .SetTitle(title ?? string.Empty)
            .SetMessage(message)
            .SetPositiveButton(Android.Resource.String.Ok, delegate
            {
                confirmationAction(true);
            }).SetNegativeButton(Android.Resource.String.Cancel, delegate
            {
                confirmationAction(false);
            });

            activity.RunOnUiThread(() =>
            {
                AlertDialog alert = builder.Create();
                alert.Show();
            });
        }

    }
}