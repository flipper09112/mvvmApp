using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
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
    public class MainActivity : MvxAppCompatActivity<MainViewModel>, Android.Locations.ILocationListener
    {
        public ProgressBar _indeterminateBar;
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;
        private bool _FindClosestClient;

        public Action<Location> UpdateEditClientLocation { get; internal set; }

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

        public void RequestCurrentLocationUpdates()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
            {
                Criteria locationCriteria = new Criteria();
                locationCriteria.Accuracy = Accuracy.Coarse;
                locationCriteria.PowerRequirement = Power.Medium;

                LocationManager locationManager = (LocationManager)GetSystemService(LocationService);
                string locationProvider = locationManager.GetBestProvider(locationCriteria, true);
                locationManager.RequestSingleUpdate(locationProvider, this, null);
            }
            else
            {
                // The app does not have permission ACCESS_FINE_LOCATION 
            }
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

            //set search bar configs
            var mSearch = menu.FindItem(Resource.Id.action_search);
            Android.Support.V7.Widget.SearchView mSearchView = (Android.Support.V7.Widget.SearchView) mSearch.ActionView;
            mSearchView.QueryHint = "Insira dados do cliente";
            EditText searchEditText = (EditText)mSearchView.FindViewById(Resource.Id.search_src_text);
            searchEditText.SetTextColor(Resources.GetColor(Resource.Color.white));
            searchEditText.SetHintTextColor(Resources.GetColor(Resource.Color.white));
            mSearchView.QueryTextChange -= MSearchViewQueryTextChange;
            mSearchView.QueryTextChange += MSearchViewQueryTextChange;

            return true;
        }

        private void MSearchViewQueryTextChange(object sender, Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            ViewModel.SetFilterCommand.Execute(e.NewText.ToString());
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }
            if (id == Resource.Id.getClosestClient)
            {
                ViewModel.IsBusy = true;
                _FindClosestClient = true;
                RequestCurrentLocationUpdates();
                return true;
            }
            if (id == Android.Resource.Id.Home)
            {
                _drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                return true;
            }
            if (id == Resource.Id.action_search)
            {
                var frag = SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if(frag is HomeFragment)
                {

                    HomeFragment homeFragment = frag as HomeFragment;
                }
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

        #region GPS
        public void OnLocationChanged(Location location)
        {
            System.Diagnostics.Debug.WriteLine(location.ToString());

            if(_FindClosestClient)
            {
                ViewModel.SetClosestClientCommand.Execute((location.Latitude, location.Longitude));
                _FindClosestClient = false;
            }
            UpdateEditClientLocation?.Invoke(location);
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
        #endregion
    }
}
