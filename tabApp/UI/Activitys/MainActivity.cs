using System;
using System.Runtime.Remoting.Contexts;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.ViewModels;
using tabApp.Helpers;
using tabApp.Services.Implementations.Native;
using tabApp.UI;
using tabApp.UI.Fragments.Snooze;
using ForegroundService = tabApp.Services.Implementations.Native.ForegroundService;

namespace tabApp
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        public static MainActivity Instance;

        public ProgressBar _indeterminateBar;
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;
        private bool _FindClosestClient;
        private string locationProvider;

        public Action<Location> LocationEvent { get; internal set; }
        public MvxCommand<Location> LocationEventCommand;
        private int timer = 0;
        private Intent foregroundIntent;
        private Handler _handler;
        private bool _startForegroundServiceRunning;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            NavigationView nav_view = FindViewById<NavigationView>(Resource.Id.nav_view);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_hamburger_menu);

            nav_view.SetNavigationItemSelectedListener(this);

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

            ViewModel.StarCounting();

            LocationEventCommand = new MvxCommand<Location>(LocationEventCmd);
            Instance = this;
        }

        protected override void OnResume()
        {
            if(!IsServiceRunning(typeof(ForegroundService)))
            {
                _handler = new Handler();
                _handler.PostDelayed(StartForegroundService, 10000);
            }
            base.OnResume();

            ViewModel.RestartSwatch();
        }

        private void StartForegroundService()
        {
            if(!_startForegroundServiceRunning)
            {
                _startForegroundServiceRunning = true;
                StartForegroundServiceCompat<ForegroundService>();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            ViewModel.StopCounting();
        }

        protected override void OnDestroy()
        {
            if(foregroundIntent != null)
            {
                StopService(foregroundIntent);
                foregroundIntent = null;
                _startForegroundServiceRunning = false;
            }
            ViewModel.DestroyCounting();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }

        public bool IsServiceRunning(System.Type ClassTypeof)
        {
            ActivityManager manager = (ActivityManager)ApplicationContext.GetSystemService(ActivityService);
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ShortClassName.Contains(ClassTypeof.Name))
                {
                    return true;
                }
            }
            return false;
        }

        internal void HideMenu()
        {
            _drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);  
        }
        internal void ShowMenu()
        {
            _drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
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
                ((HomeFragment)frag).UpdateAllLists();
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

        #region MENUS
        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            int id = menuItem.ItemId;

            if (id == Resource.Id.globalOrder)
            {
                ViewModel.ShowGlobalOrderPageCommand.Execute(null);
                return true;
            }
            if (id == Resource.Id.tabela)
            {
                ViewModel.ShowPriceTableCommand.Execute(null);
                return true;
            }

            if (id == Resource.Id.conectBt)
            {
                ViewModel.SyncronizeCommand.Execute(null);
                return true;
            }

            if (id == Resource.Id.otherOptions)
            {
                ViewModel.OtherOptionsCommand.Execute(null);
                return true;
            }

            return base.OnOptionsItemSelected(menuItem);
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
        #endregion

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            var frag = SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
            if (frag is SnoozeFragment) {
                OnUserInteraction();
                return;
            }
            else if (SupportFragmentManager.BackStackEntryCount == 1)
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

        public override void OnUserInteraction()
        {
            var frag = SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
            if (frag is SnoozeFragment)
            {
                SupportFragmentManager.PopBackStack();
                ViewModel.RestartSwatch();
                ViewModel.StarCounting();
            }
            else
            {
                ViewModel.RestartSwatch();
            }
            base.OnUserInteraction();
        }

        private void OrderNotification()
        {
        }

        public void LoadingView(bool visible)
        {
            _indeterminateBar.Indeterminate = true;
            _indeterminateBar.Visibility = visible ? ViewStates.Visible : ViewStates.Invisible;
        }

        #region ForeGroundService
        public void StartForegroundServiceCompat<T>(Bundle args = null) where T : Service
        {
            if (foregroundIntent != null) return;

            foregroundIntent = new Intent(this, typeof(T));
            if (args != null)
            {
                foregroundIntent.PutExtras(args);
            }

           /* if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                this.StartForegroundService(intent);

            }
            else
            {*/
                this.StartService(foregroundIntent);
            /*}*/
        }
        #endregion

        #region GPS

        private void LocationEventCmd(Location location)
        {
            if(_FindClosestClient)
            {
                ViewModel.SetClosestClientCommand.Execute((location.Latitude, location.Longitude));
                _FindClosestClient = false;
            }

            LocationEvent?.Invoke(location);
        }
        #endregion
    }
}
