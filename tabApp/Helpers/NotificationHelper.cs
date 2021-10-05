using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using MvvmCross;
using MvvmCross.Platforms.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.App.ActivityManager;
using Uri = Android.Net.Uri;

namespace tabApp.Helpers
{
    public class NotificationHelper : ContextWrapper
    {
        public const string PRIMARY_CHANNEL = "HighPriority1";
        private int SmallIcon => Android.Resource.Drawable.StatNotifyChat;

        NotificationManager manager;
        private AlertDialog _dialog;

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

            ShowNotificationPopPup(title, body);
        }

        private void ShowNotificationPopPup(string title, string body)
        {
            if (!IsApplicationSentToBackground(MainActivity.Instance))
            {
                ShowPopPup(title, body);
            }
        }

        private void ShowPopPup(string title, string body)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            AlertDialog.Builder alert = new AlertDialog.Builder(act);
            alert.SetView(Resource.Layout.NotificationPopPup);
            alert.SetCancelable(false);
            
            _dialog = alert.Create();
            _dialog.Show();

            var cancelBtn = _dialog.FindViewById<Button>(Resource.Id.cancelButton);
            var silentBtn = _dialog.FindViewById<Button>(Resource.Id.silentButton);
            var descTxt = _dialog.FindViewById<TextView>(Resource.Id.desc);
            var titleTxt = _dialog.FindViewById<TextView>(Resource.Id.title);

            cancelBtn.Click -= CancelBtnClick;
            cancelBtn.Click += CancelBtnClick;

            silentBtn.Click -= SilentBtnClick;
            silentBtn.Click += SilentBtnClick;

            titleTxt.Text = title;
            descTxt.Text = body;
        }

        private bool IsApplicationSentToBackground(Context context)
        {
            ActivityManager am = (ActivityManager)context.GetSystemService(Context.ActivityService);
            var tasks = am.GetRunningTasks(1);
            if (!(tasks.Count == 0))
            {
                ComponentName topActivity = tasks[0].TopActivity;
                if (!topActivity.PackageName.Equals(context.PackageName))
                {
                    return true;
                }
            }

            return false;
        }

        private void CancelBtnClick(object sender, EventArgs e)
        {
            Manager.CancelAll();
            _dialog?.Dismiss();
        }

        private void SilentBtnClick(object sender, EventArgs e)
        {
            Manager.CancelAll();
        }
    }
}