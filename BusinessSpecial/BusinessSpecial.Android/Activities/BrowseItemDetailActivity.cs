
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using BusinessSpecial.Model;
using BusinessSpecial.Models;
using BusinessSpecial.ViewModel;

namespace BusinessSpecial.Droid
{
    [Activity(Label = "Details", ParentActivity = typeof(MainActivity))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = ".MainActivity")]
    public class BrowseItemDetailActivity : BaseActivity
    {
        /// <summary>
        /// Specify the layout to inflace
        /// </summary>
        protected override int LayoutResource => Resource.Layout.activity_item_details;


        ItemDetailViewModel viewModel;
        Spinner spinner;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

           // var data = Intent.GetStringExtra("data");

            //var advert = Newtonsoft.Json.JsonConvert.DeserializeObject<Advert>(data);
            //viewModel = new ItemDetailViewModel(advert);

            //FindViewById<TextView>(Resource.Id.description).Text = advert.User.DisplayName;


            //SupportActionBar.Title = advert.SpecialName;
        }


        protected override void OnStart()
        {
            base.OnStart();
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

        }


        protected override void OnStop()
        {
            base.OnStop();
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}