﻿
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

namespace BusinessSpecial.Droid
{
    public class BrowseFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {

        public static BrowseFragment NewInstance() =>
            new BrowseFragment { Arguments = new Bundle() };

        BrowseItemsAdapter adapter;
        SwipeRefreshLayout refresher;
        Task loadItems;

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
            loadItems = ViewModel.ExecuteLoadItemsCommand();


            MessagingCenter.Subscribe<AddItemActivity, Advert>(this, "AddItem", async (obj, advert) =>
            {
                var _advert = advert as Advert;
                await ViewModel.AddItem(_advert);
            });
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_browse, container, false);
            var recyclerView =
                view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

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
            var intent = new Intent(Activity, typeof(BrowseItemDetailActivity));

            intent.PutExtra("data", Newtonsoft.Json.JsonConvert.SerializeObject(item));
            Activity.StartActivity(intent);
        }

        private async void Refresher_Refresh(object sender, EventArgs e)
        {
            await ViewModel.ExecuteLoadItemsCommand();
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

            // Replace the contents of the view with that element
            var myHolder = holder as MyViewHolder;
            myHolder.TextView.Text = advert.SpecialName;
            myHolder.DetailTextView.Text = advert.User.BusinessName;
        }

        public override int ItemCount => viewModel.Adverts.Count;


    }

    public class MyViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextView { get; set; }
        public TextView DetailTextView { get; set; }

        public MyViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            TextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text1);
            DetailTextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text2);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

}

