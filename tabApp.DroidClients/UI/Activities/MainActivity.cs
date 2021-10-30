using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using tabApp.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using tabApp.Core.ViewModelsClient;
using Android.Widget;

namespace tabApp.DroidClients
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : MvxAppCompatActivity<HomePageViewModel>
    {
        private ProgressBar _indeterminateBar;
        private FrameLayout _container;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            _indeterminateBar = FindViewById<ProgressBar>(Resource.Id.indeterminateBar);
            _container = FindViewById<FrameLayout>(Resource.Id.fragmentContainer);

            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;

            _indeterminateBar.Visibility = ViewModel.IsBusy ? ViewStates.Visible : ViewStates.Invisible;

            if (SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer) == null)
            {
                ViewModel.ShowHomePageCommand.Execute();
            }
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewModelPropertyChanged(ViewModel.IsBusy, e);
        }

        public void ViewModelPropertyChanged(bool sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsBusy):
                    _indeterminateBar.Visibility = sender ? ViewStates.Visible : ViewStates.Invisible;
                    break;
            }
        }

        public void HideToolbar()
        {
            SupportActionBar.Hide();
        }
        public void ShowToolbar()
        {
            SupportActionBar.Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
