﻿
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using System;

namespace tabApp.Services.Implementations.Native
{
    [Service]
    public class ForegroundService : Service, ILocationListener
    {
        const int SERVICE_RUNNING_NOTIFICATION_ID = 123;
        const string NOTIFICATION_CHANNEL_ID = "com.company.app.channel";

        readonly string logTag = "LocationService";
        IBinder binder;

        // Set our location manager as the system location service
        protected LocationManager LocMgr = Application.Context.GetSystemService("location") as LocationManager;

        // ILocationListener is a way for the Service to subscribe for updates
        // from the System location Service

        public void OnLocationChanged(Android.Locations.Location location)
        {
            MainActivity.Instance?.LocationEventCommand?.Execute(location);

            // This should be updating every time we request new location updates
            // both when teh app is in the background, and in the foreground
            Log.Debug(logTag, $"Latitude is {location.Latitude}");
            Log.Debug(logTag, $"Longitude is {location.Longitude}");
            Log.Debug(logTag, $"Altitude is {location.Altitude}");
            Log.Debug(logTag, $"Speed is {location.Speed}");
            Log.Debug(logTag, $"Accuracy is {location.Accuracy}");
            Log.Debug(logTag, $"Bearing is {location.Bearing}");
        }

        public void OnProviderDisabled(string provider)
        {
            ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
        }

        public void OnProviderEnabled(string provider)
        {
            ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
        }

        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate {};
        public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
        public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
        public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug(logTag, "OnCreate called in the Location Service");
            StartLocationUpdates();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            Log.Debug(logTag, "LocationService started");

            // Check if device is running Android 8.0 or higher and call StartForeground() if so
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                                   .SetContentTitle(Resources.GetString(Resource.String.app_name))
                                   .SetContentText("A aplicação está a rastrear a sua localização para poder criar geo alertas!")
                                   .SetSmallIcon(Resource.Drawable.notification_icon_background)
                                   .SetOngoing(true)
                                   .SetContentIntent(BuildIntentToShowMainActivity())
                                   .Build();

                var notificationManager =
                    GetSystemService(NotificationService) as NotificationManager;

                var chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "On-going Notification", NotificationImportance.Max);

                notificationManager.CreateNotificationChannel(chan);

                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }

            return StartCommandResult.Sticky;
        }
        /// <summary>
         /// Builds a PendingIntent that will display the main activity of the app. This is used when the 
         /// user taps on the notification; it will take them to the main activity of the app.
         /// </summary>
         /// <returns>The content intent.</returns>
        private PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);
            return pendingIntent;
        }

        /*public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Debug(logTag, "LocationService started");

            // Check if device is running Android 8.0 or higher and call StartForeground() if so
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Intent notificationIntent = new Intent(this, typeof(MainActivity));
                PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

                var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                                   .SetContentTitle(Resources.GetString(Resource.String.app_name))
                                   .SetContentText("A aplicação está a rastrear a sua localização para poder criar geo alertas!")
                                   .SetSmallIcon(Resource.Drawable.notification_icon_background)
                                   .SetOngoing(true)
                                   .SetContentIntent(pendingIntent)
                                   .Build();

                var notificationManager =
                    GetSystemService(NotificationService) as NotificationManager;

                var chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "On-going Notification", NotificationImportance.Max);

                notificationManager.CreateNotificationChannel(chan);

                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }

            return StartCommandResult.Sticky;
        }*/

        // This gets called once, the first time any client bind to the Service
        // and returns an instance of the LocationServiceBinder. All future clients will
        // reuse the same instance of the binder
        public override IBinder OnBind(Intent intent)
        {
            Log.Debug(logTag, "Client now bound to service");

            //binder = new LocationServiceBinder(this);
            return /*binder*/null;
        }

        // Handle location updates from the location manager
        public void StartLocationUpdates()
        {
            //we can set different location criteria based on requirements for our app -
            //for example, we might want to preserve power, or get extreme accuracy
            var locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            // get provider: GPS, Network, etc.
            var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
            Log.Debug(logTag, string.Format("You are about to get location updates via {0}", locationProvider));

            // Get an initial fix on location
            LocMgr.RequestLocationUpdates(locationProvider, 250, 0, this);

            Log.Debug(logTag, "Now sending location updates");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(logTag, "Service has been terminated");

            // Stop getting updates from the location manager:
            LocMgr.RemoveUpdates(this);
        }

    }
}