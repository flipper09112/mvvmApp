using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using tabApp.Core.ViewModelsClient;

namespace tabApp.DroidClients.UI
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class InitialActivity : MvxAppCompatActivity<MainViewModelClient>
    {
        private ImageView _imageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_splash_screen);
            AppCenter.Start("5a0cfc0a-ba5b-4ea9-b2e2-08be8c0cae30", typeof(Analytics), typeof(Crashes));

            _imageView = FindViewById<ImageView>(Resource.Id.videoView);

            Handler handler = new Handler();
            handler.PostDelayed(ShowHomePage, 5500);
        }

        private void ShowHomePage()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();

            Glide.With(this)
                .Load(Resource.Drawable.gif_base)
                .Placeholder(Resource.Drawable.gif_base_image)
                .SetDiskCacheStrategy(DiskCacheStrategy.All)
                .Into(_imageView);
        }
    }
}