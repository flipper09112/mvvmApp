using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using tabApp.UI.Adapters.Home;

namespace tabApp.UI.Fragments.Home.ViewPager
{
    public class HomeNotificationsFragment : Android.Support.V4.App.Fragment
    {
        private HomeViewModel viewModel;
        private NotificationsPage notificationData;
        private MainActivity _activity;
        private NotificationsListAdapter adapter;
        private RecyclerView recycler;

        public HomeNotificationsFragment(HomeViewModel viewModel, NotificationsPage notificationData)
        {
            this.viewModel = viewModel;
            this.notificationData = notificationData;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if ((notificationData.Value?.Count ?? 0) == 0)
                return inflater.Inflate(Resource.Layout.EmptyListItems, container, false);

            View view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

            adapter = new NotificationsListAdapter(notificationData.Value, viewModel);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(Context);

            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(adapter);

            return view;
        }
    }
}