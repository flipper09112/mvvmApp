using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using In.UnicodeLabs.KdGaugeViewLib;
using Java.Lang;
using Java.Util.Concurrent;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Snooze;
using tabApp.UI.Adapters.Home;
using Xamarin.Essentials;
using Location = Android.Locations.Location;
using Math = System.Math;

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
        private TextView _clientNameLabel;
        private List<(Client Client, ExtraOrder ExtraOrder)> _orders;
        private List<Core.Models.Notifications.Notification> _notifications;
        private HomePageOrdersListAdapter _adapter;
        private bool _running;
        private NotificationsListAdapter _notificationsAdapter;
        private bool _runningNot;
        private TextView _clientDailyOrderLabel;
        private ConstraintLayout _mainView;
        private Runnable _runnable;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SnoozeFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _speedometer = view.FindViewById<KdGaugeView>(Resource.Id.speedometer);
            _todayOrdersList = view.FindViewById<RecyclerView>(Resource.Id.todayOrdersList);
            _notificationList = view.FindViewById<AndroidX.RecyclerView.Widget.RecyclerView>(Resource.Id.notificationList);
            _withoutOrders = view.FindViewById<ImageView>(Resource.Id.withoutOrders);
            _withoutNotifications = view.FindViewById<ImageView>(Resource.Id.withoutNotifications);
            _clientNameLabel = view.FindViewById<TextView>(Resource.Id.clientNameLabel);
            _clientDailyOrderLabel = view.FindViewById<TextView>(Resource.Id.clientDailyOrderLabel);
            _mainView = view.FindViewById<ConstraintLayout>(Resource.Id.mainView);

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
            _notifications = ViewModel.NotificationsList;

            if (_orders.Count == 0)
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
            SetClosestNotification(obj);
            SetClosestClient(obj);
        }

        private void SetClosestClient(Location coord)
        {
            Client client = ViewModel.GetClosestClient(coord.Latitude, coord.Longitude);
            if (client == null) return;

            _clientNameLabel.Text = client.Name + " (" + client.Id + " )";
            _clientDailyOrderLabel.Text = ViewModel.GetDailyOrderDesc(client);

            /*if(_runnable == null)
            {
                var executorService = Executors.NewSingleThreadExecutor();

                _runnable = new Runnable(async () => {

                    while(true)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            _mainView.SetBackgroundColor(GetColor(Resource.Color.yellow));
                        });

                        await Task.Delay(500);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            _mainView.SetBackgroundColor(GetColor(Resource.Color.white));
                        });

                        await Task.Delay(500);
                    }
                });

                var longRunningTaskFuture = executorService.Submit(_runnable);
            }*/
        }
        private Android.Graphics.Color GetColor(int color)
        {
            return CrossCurrentActivity.Current.AppContext.Resources.GetColor(color);
        }

        private void SetClosestNotification(Location obj)
        {
            if (_runningNot) return;
            _runningNot = true;
            if (_notifications == null || _notifications.Count == 0) return;

            Dictionary<Core.Models.Notifications.Notification, double> distances = new Dictionary<Core.Models.Notifications.Notification, double>();

            foreach (var item in _notifications)
            {
                if (item.Latitude.Equals("null") || item.Latitude == null || item.Latitude.Equals("")) continue;

                double distance = Math.Sqrt(Math.Pow(double.Parse(item.Latitude) - obj.Latitude, 2) + Math.Pow(double.Parse(item.Longitude) - obj.Longitude, 2));
                distances.Add(item, distance);
            }

            if (distances.Count == 0) return;

            double minimumDistance = distances.Min(distance => distance.Value);
            var closestOrder = distances.First(distance => distance.Value == minimumDistance).Key;

            int pos = _notifications.IndexOf(closestOrder);
            _notificationList.SmoothScrollToPosition(pos);
            _runningNot = false;
        }

        private void SetClosestOrder(Location obj)
        {
            if (_running) return;
            _running = true;
            if (_orders == null || _orders.Count == 0) return;

            Dictionary<(Client Client, ExtraOrder ExtraOrder), double> distances = new Dictionary<(Client Client, ExtraOrder ExtraOrder), double>();

            foreach (var item in _orders)
            {
                if (item.Client.Address.Coordenadas.Equals("null") || item.Client.Address.Coordenadas == null) continue;

                double distance = Math.Sqrt(Math.Pow(double.Parse(item.Client.Address.Lat) - obj.Latitude, 2) + Math.Pow(double.Parse(item.Client.Address.Lgt) - obj.Longitude, 2));
                distances.Add(item, distance);
            }

            if (distances.Count == 0) {
                _running = false;
                return;
            }

            double minimumDistance = distances.Min(distance => distance.Value);
            var closestOrder = distances.First(distance => distance.Value == minimumDistance).Key;

            int pos = _orders.IndexOf(closestOrder);
            _todayOrdersList.SmoothScrollToPosition(pos);

            _running = false;
        }
    }
}