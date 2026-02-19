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
        public NotificationsPage NotificationData;
        private MainActivity _activity;
        private NotificationsListAdapter adapter;
        private View emptyLayout;
        private RecyclerView recycler;

        public HomeNotificationsFragment(HomeViewModel viewModel, NotificationsPage notificationData)
        {
            this.viewModel = viewModel;
            this.NotificationData = notificationData;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomePageOrdersFragment, container, false);

            emptyLayout = view.FindViewById(Resource.Id.emptyLayout);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.listView);

            adapter = new NotificationsListAdapter(NotificationData.Value, viewModel);
            var layoutManager = new LinearLayoutManager(Context);

            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(adapter);

            UpdateUI();

            return view;
        }
        private void UpdateUI()
        {
            if (NotificationData.Value?.Count == 0)
            {
                emptyLayout.Visibility = ViewStates.Visible;
                recycler.Visibility = ViewStates.Invisible;
            }
            else
            {
                emptyLayout.Visibility = ViewStates.Invisible;
                recycler.Visibility = ViewStates.Visible;
            }
        }

        internal void UpdateNotificationsList(bool setNewAdapter = false)
        {
            if (setNewAdapter)
            {
                SetNewAdapter();
            }
            else
            {
                adapter?.NotifyDataSetChanged();
            }
            UpdateUI();
        }

        private void SetNewAdapter()
        {
            adapter = new NotificationsListAdapter(NotificationData.Value, viewModel);
            recycler.SetAdapter(adapter);
        }
    }
}