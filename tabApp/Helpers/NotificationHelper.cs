using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uri = Android.Net.Uri;

namespace tabApp.Helpers
{
    public class NotificationHelper : ContextWrapper
    {
        public const string PRIMARY_CHANNEL = "HighPriority1";
        private int SmallIcon => Android.Resource.Drawable.StatNotifyChat;

        NotificationManager manager;
        NotificationManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = (NotificationManager)GetSystemService(NotificationService);
                }
                return manager;
            }
        }

        public NotificationHelper(Context context) : base(context)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var chan1 = new NotificationChannel(PRIMARY_CHANNEL, "not", NotificationImportance.High);
                chan1.LightColor = Color.Green;
                chan1.LockscreenVisibility = NotificationVisibility.Public;

                Uri soundUri = Uri.Parse(
                         "android.resource://" +
                         ApplicationContext.PackageName +
                         "/" +
                         Resource.Raw.alarm);

                AudioAttributes audioAttributes = new AudioAttributes.Builder()
                            .SetContentType(AudioContentType.Sonification)
                            .SetUsage(AudioUsageKind.Alarm)
                            .Build();

                chan1.SetSound(soundUri, audioAttributes);

                Manager.CreateNotificationChannel(chan1);
            }
        }

        public void Notify(int id, string title, string body)
        {
            Uri soundUri = Uri.Parse(
                         "android.resource://" +
                         ApplicationContext.PackageName +
                         "/" +
                         Resource.Raw.alarm);

            var notificationBuilder = new NotificationCompat.Builder(ApplicationContext, PRIMARY_CHANNEL)
                        .SetContentText(body)
                        .SetContentTitle(title)
                        .SetSmallIcon(SmallIcon)
                        .SetPriority(NotificationCompat.PriorityHigh)
                        .SetCategory(NotificationCompat.CategoryAlarm)
                        .SetSound(soundUri)
                        .SetAutoCancel(true);

            Manager.Notify(id, notificationBuilder.Build());
        }
    }
}