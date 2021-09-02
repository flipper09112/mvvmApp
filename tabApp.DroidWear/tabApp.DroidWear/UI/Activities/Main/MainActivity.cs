using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Net.Http.Handlers;
using tabApp.Core;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModelsWear;

namespace tabApp.DroidWear
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : MvxAppCompatActivity<MainViewModelWear>
    {
        private TextView _internetSpeed;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main); 
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            _internetSpeed = FindViewById<TextView>(Resource.Id.internetSpeed);

            ViewModel.UpdatePercentageDownloadEvent -= UpdatePercentageDownloadEvent;
            ViewModel.UpdatePercentageDownloadEvent += UpdatePercentageDownloadEvent;
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        private void UpdatePercentageDownloadEvent(object sender, EventArgs e)
        {
            RunOnUiThread(() => { _internetSpeed.Text = ((HttpProgressEventArgs)e).ProgressPercentage.ToString(); });
            
        }
    }
}


