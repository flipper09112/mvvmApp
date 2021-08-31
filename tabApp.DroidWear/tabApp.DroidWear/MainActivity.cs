using Android.App;
using Android.OS;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using tabApp.Core.ViewModels;

namespace tabApp.DroidWear
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity/* MvxAppCompatActivity<MainViewModel>*/
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);
            
            //SetAmbientEnabled();
        }
    }
}


