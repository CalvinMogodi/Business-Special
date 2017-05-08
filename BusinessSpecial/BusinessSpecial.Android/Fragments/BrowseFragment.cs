
using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using BusinessSpecial.ViewModel;
using Android.Support.V4.Widget;
using Android.App;
using Android.Content;
using BusinessSpecial.Helpers;
using BusinessSpecial.Services;
using System.Threading.Tasks;
using BusinessSpecial.Model;
using BusinessSpecial.Models;
using BusinessSpecial.Droid.Helpers;
using Android.Graphics.Drawables;

namespace BusinessSpecial.Droid
{
    public class BrowseFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {

        public static BrowseFragment NewInstance() =>
            new BrowseFragment { Arguments = new Bundle() };

        BrowseItemsAdapter adapter;
        SwipeRefreshLayout refresher;
        Task loadItems;
        AlertDialog dialog;
        ProgressBar progress;
        public ItemsViewModel ViewModel
        {
            get;
            set;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ServiceLocator.Instance.Register<MockDataStore, MockDataStore>();

            ViewModel = new ItemsViewModel();
            loadItems = ViewModel.GetAdvertsAsync();


            //MessagingCenter.Subscribe<AddItemActivity, Advert>(this, "AddItem", async (obj, advert) =>
            //{
            //    var _advert = advert as Advert;
            //    await ViewModel.AddItem(_advert);
            //});
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_browse, container, false);
            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;
            recyclerView.SetAdapter(adapter = new BrowseItemsAdapter(Activity, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

            refresher.SetColorSchemeColors(Resource.Color.accent);

            progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_loading);
            progress.Visibility = ViewStates.Gone;

            return view;
        }


        public override void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;
            adapter.ItemClick += Adapter_ItemClick;

            if (ViewModel.Adverts.Count == 0)
                loadItems.Wait();
        }


        public override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
            adapter.ItemClick -= Adapter_ItemClick;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            MessagingCenter.Unsubscribe<AddItemActivity>(this, "AddItem");
        }

        private void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            var item = ViewModel.Adverts[e.Position];
            Context mContext = Android.App.Application.Context;
            AppPreferences appPreferences = new AppPreferences(mContext);

            Drawable logo = new BitmapDrawable(appPreferences.StringToBitMap(item.User.Logo));

            AlertDialog.Builder builder = new AlertDialog.Builder(Activity, Resource.Style.Base_Widget_AppCompat_Button_Colored);
            // Get the layout inflater
            LayoutInflater inflater = Activity.LayoutInflater;
            builder.SetView(inflater.Inflate(Resource.Layout.view_advert, null));
            builder.SetTitle(String.Format("{0} ({1})", item.User.DisplayName, item.SpecialName));
            builder.SetIcon(logo);
            builder.SetPositiveButton("Add To Favourites", delegate
             {

                 UserActivity userActivity = new UserActivity() { 
                     AdvertCategory = item.Category,
                     SpecialName = item.SpecialName,
                     UserId = appPreferences.getAccessKey(),
                     Date = DateTime.Now.ToLongDateString(),
                     ActivityType = "Like Advert",
                     BusinessName = item.User.DisplayName,
                     BusinessLoge = item.Logo,
                     BusinessId = item.User.Id,
                 };
                 ViewModel.AddUserActivity(userActivity);
                 dialog.Cancel();
             }).SetNegativeButton("Not Now", delegate
             {
                 dialog.Cancel();
             });
            dialog = builder.Create();

            dialog.Show();
            dialog.SetCanceledOnTouchOutside(false);
            InitializeLogin(item);

        }

        private void InitializeLogin(Advert advert)
        {

            // Create your application here
            TextView category = dialog.FindViewById<TextView>(Resource.Id.viewadvert_category);
            TextView location = dialog.FindViewById<TextView>(Resource.Id.viewadvert_location);
            TextView date = dialog.FindViewById<TextView>(Resource.Id.viewadvert_date);
            TextView time = dialog.FindViewById<TextView>(Resource.Id.viewadvert_time);
            TextView email = dialog.FindViewById<TextView>(Resource.Id.viewadvert_email);
            TextView phone = dialog.FindViewById<TextView>(Resource.Id.viewadvert_phone);
            TextView websiteLink = dialog.FindViewById<TextView>(Resource.Id.viewadvert_websiteLink);

            category.Text = string.Format("Category: {0}", advert.Category);
            location.Text = string.Format("Location: {0}", advert.Location);
            date.Text = string.Format("Date: {0} - {1}", advert.StartDate, advert.EndDate);
            time.Text = string.Format("Time: {0} - {1}", advert.StartTime, advert.EndTime);
            email.Text = string.Format("Email: {0}", advert.Email);
            phone.Text = string.Format("Phone: {0}", advert.Phone);
            websiteLink.Text = string.Format("Website: {0}", advert.User.WebsiteLink);
        }

        private async void Refresher_Refresh(object sender, EventArgs e)
        {
            await ViewModel.GetAdvertsAsync();
            refresher.Refreshing = false;
        }

        public void BecameVisible()
        {
        }
    }

    class BrowseItemsAdapter : BaseRecycleViewAdapter
    {

        ItemsViewModel viewModel;
        Activity activity;

        public BrowseItemsAdapter(Activity activity, ItemsViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;

            this.viewModel.Adverts.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.item_browse;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new MyViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var advert = viewModel.Adverts[position];
            Context mContext = Android.App.Application.Context;
            AppPreferences appPreferences = new AppPreferences(mContext);

            // Replace the contents of the view with that element
            var myHolder = holder as MyViewHolder;
            myHolder.TextView.Text =  advert.User.DisplayName; 
            myHolder.DetailTextView.Text = String.Format("{0} ({1} - {2})", advert.SpecialName, advert.StartDate, advert.EndDate);
            myHolder.ProfilePictureImageView.SetImageBitmap(appPreferences.StringToBitMap(advert.Logo)); 
        }

        public override int ItemCount => viewModel.Adverts.Count;


    }

    public class MyViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextView { get; set; }
        public TextView DetailTextView { get; set; }

        public ImageView ProfilePictureImageView { get; set; }

        public MyViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            TextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text1);
            DetailTextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text2);
            ProfilePictureImageView = itemView.FindViewById<ImageView>(Resource.Id.imageView1);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

}

