using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using In.UnicodeLabs.KdGaugeViewLib;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Snooze;
using tabApp.UI.Adapters.Home;

namespace tabApp.UI.Fragments.Snooze
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class SnoozeFragment : BaseFragment<SnoozeViewModel>
    {
        private MainActivity _activity;
        private KdGaugeView _speedometer;
        private RecyclerView _todayOrdersList;
        private AndroidX.RecyclerView.Widget.RecyclerView _notificationList;
        private ImageView _withoutOrders;
        private ImageView _withoutNotifications;
        private List<(Client Client, ExtraOrder ExtraOrder)> _orders;
        private HomePageOrdersListAdapter _adapter;
        private bool _running;
        private NotificationsListAdapter _notificationsAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SnoozeFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _speedometer = view.FindViewById<KdGaugeView>(Resource.Id.speedometer);
            _todayOrdersList = view.FindViewById<RecyclerView>(Resource.Id.todayOrdersList);
            _notificationList = view.FindViewById<AndroidX.RecyclerView.Widget.RecyclerView>(Resource.Id.notificationList);
            _withoutOrders = view.FindViewById<ImageView>(Resource.Id.withoutOrders);
            _withoutNotifications = view.FindViewById<ImageView>(Resource.Id.withoutNotifications);
           
            var layoutManager2 = new AndroidX.RecyclerView.Widget.LinearLayoutManager(Context);
            _notificationList.SetLayoutManager(layoutManager2);

            var layoutManager = new LinearLayoutManager(Context);
            _todayOrdersList.SetLayoutManager(layoutManager);

            return view;
        }
        public override void CleanBindings()
        {
            _activity.LocationEvent -= LocationEvent;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();

            SetAdapter();
        }

        private void SetAdapter()
        {
            _orders = ViewModel.TodayOrders;

            if(_orders.Count == 0)
            {
                _withoutOrders.Visibility = ViewStates.Visible;
                _todayOrdersList.Visibility = ViewStates.Invisible;
            }
            else
            {
                _withoutOrders.Visibility = ViewStates.Invisible;
                _todayOrdersList.Visibility = ViewStates.Visible;

                _adapter = new HomePageOrdersListAdapter(_orders, ViewModel);
                _todayOrdersList.SetAdapter(_adapter);
            }

            if (ViewModel.NotificationsList?.Count == 0)
            {
                _withoutNotifications.Visibility = ViewStates.Visible;
                _notificationList.Visibility = ViewStates.Invisible;
            }
            else
            {
                _withoutNotifications.Visibility = ViewStates.Invisible;
                _notificationList.Visibility = ViewStates.Visible;

                _notificationsAdapter = new NotificationsListAdapter(ViewModel.NotificationsList, ViewModel);
                _notificationList.SetAdapter(_notificationsAdapter);
            }
        }

        public override void SetupBindings()
        {
            _activity.LocationEvent += LocationEvent;
        }

        private void LocationEvent(Location obj)
        {
            _speedometer.SetSpeed(obj.Speed * 3.6f);

            SetClosestOrder(obj);
        }

        private void SetClosestOrder(Location obj)
        {
            if (_running) return;
            _running = true;
            if (_orders == null || _orders.Count == 0) return;

            Dictionary<(Client Client, ExtraOrder ExtraOrder), double> distances = new Dictionary<(Client Client, ExtraOrder ExtraOrder), double>();

            foreach (var item in _orders)
            {
                if (item.Client.Address.Coordenadas.Equals("null") || item.Client.Address.Coordenadas != null) continue;

                double distance = Math.Sqrt(Math.Pow(double.Parse(item.Client.Address.Lat) - obj.Latitude, 2) + Math.Pow(double.Parse(item.Client.Address.Lgt) - obj.Longitude, 2));
                distances.Add(item, distance);
            }

            if (distances.Count == 0) return;

            double minimumDistance = distances.Min(distance => distance.Value);
            var closestOrder = distances.First(distance => distance.Value == minimumDistance).Key;

            int pos = _orders.IndexOf(closestOrder);
            _todayOrdersList.SmoothScrollToPosition(pos);

            _running = false;
        }
    }
}