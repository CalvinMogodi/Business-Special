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
using BusinessSpecial.ViewModels;
using BusinessSpecial.Models;

namespace BusinessSpecial.Droid.Activities
{
    [Activity(Label = "HelpActivity")]
    public class HelpActivity : Activity
    {
        ListView listView;
        List<FAQ> FAQs;
        HelpViewModel ViewModel;
        string[] items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            // Create your application here
            SetContentView(Resource.Layout.activity_help);
            listView = FindViewById<ListView>(Resource.Id.help_list);
            HelpViewModel ViewModel = new HelpViewModel();
            await ViewModel.GetFAQsAsync();
            FAQs = new List<FAQ>();
            FAQs = ViewModel.FAQs;

           items = ViewModel.FAQs.Select( f => f.Question.ToString()).ToArray();
            listView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
            listView.ItemClick += Handle_ItemSelected;
        }
       void Handle_ItemSelected(object sender, AdapterView.ItemClickEventArgs e) { 
        var t = items[e.Position];
        MessageDialog messageDialog = new MessageDialog();

        FAQ faq = FAQs.FirstOrDefault(f => f.Question.ToLower() == t.ToLower().ToString());
        messageDialog.SendMessage(faq.Answer, faq.Question);
        }


}
}