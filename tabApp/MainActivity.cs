using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using tabApp.Core.ViewModels;
using tabApp.Helpers;
using tabApp.UI;

namespace tabApp
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, TurnScreenOn = true)]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>
    {
        public ProgressBar _indeterminateBar;
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_hamburger_menu);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            _indeterminateBar = FindViewById<ProgressBar>(Resource.Id.indeterminateBar);
            SetSupportActionBar(toolbar);

            if (savedInstanceState == null)
            {
                ViewModel.ShowHomePage.Execute(null);
            }

            _indeterminateBar.Visibility = ViewModel.IsBusy ? ViewStates.Visible : ViewStates.Invisible;

            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;

            ViewModel.UpdateUiHomePage -= UpdateUiHomePage;
            ViewModel.UpdateUiHomePage += UpdateUiHomePage;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewModelPropertyChanged(ViewModel.IsBusy, e);
        }

        private void UpdateUiHomePage(object sender, EventArgs e)
        {
            var frag = SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
            if (frag is HomeFragment)
            {
                ((HomeFragment)frag).SetUI();
            }
        }

        public void ViewModelPropertyChanged(bool sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.IsBusy):
                    _indeterminateBar.Visibility = sender ? ViewStates.Visible : ViewStates.Invisible;
                    break;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }
            if(id == Android.Resource.Id.Home)
            {
                _drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                return true;
            } 

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount == 1)
            {
                //do nothing
                return;
            } 
            base.OnBackPressed();
        }

        public void HideToolbar()
        {
            SupportActionBar.Hide();
        }
        public void ShowToolbar()
        {
            SupportActionBar.Show();
        }
    }
}
