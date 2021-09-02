using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModelsWear.Errors;

namespace tabApp.DroidWear.UI.Activities.Errors
{
    [Activity(Label = "@string/app_name", MainLauncher = false)]
    public class NoDatabaseActivity : MvxAppCompatActivity<NoDatabaseViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.NoDatabaseActivity);
        }
    }
}