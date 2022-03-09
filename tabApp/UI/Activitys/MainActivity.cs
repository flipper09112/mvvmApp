using System;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
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
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
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
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // In this example OnReleaseAvailable is a method name in same class
            //Distribute.ReleaseAvailable = OnReleaseAvailable;
            AppCenter.Start("090e6c4a-73b9-4ce9-ab0e-19a958a1504f", typeof(Analytics), typeof(Crashes), typeof(Distribute));
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            NavigationView nav_view = FindViewById<NavigationView>(Resource.Id.nav_view);

            IMenu menu = nav_view.Menu;
            IMenuItem tools = menu.FindItem(Resource.Id.version);
            var code = Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionCode;
            tools.SetTitle("Version code: (" + code.ToString() + ")");

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

        /*private bool OnReleaseAvailable(ReleaseDetails releaseDetails)
        {
            // Look at releaseDetails public properties to get version information, release notes text or release notes URL
            string versionName = releaseDetails.ShortVersion;
            string versionCodeOrBuildNumber = releaseDetails.Version;
            string releaseNotes = releaseDetails.ReleaseNotes;
            Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

            // custom dialog
            var title = "Version " + versionName + " available!";
            Task answer;

            // On mandatory update, user can't postpone
            if (releaseDetails.MandatoryUpdate)
            {
                answer = Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install");
            }
            else
            {
                answer = Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install", "Maybe tomorrow...");
            }
            answer.ContinueWith((task) =>
            {
                // If mandatory or if answer was positive
                if (releaseDetails.MandatoryUpdate || (task as Task<bool>).Result)
                {
                    // Notify SDK that user selected update
                    Distribute.NotifyUpdateAction(UpdateAction.Update);
                }
                else
                {
                    // Notify SDK that user selected postpone (for 1 day)
                    // This method call is ignored by the SDK if the update is mandatory
                    Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                }
            });

            // Return true if you're using your own dialog, false otherwise
            return true;
        }*/

        protected override async void OnResume()
        {
            if(!IsServiceRunning(typeof(ForegroundService)))
            {
                _handler = new Handler();
                _handler.PostDelayed(StartForegroundService, 10000);
            }
            base.OnResume();

            ViewModel.RestartSwatch();

            bool enabled = await Distribute.IsEnabledAsync();
            int c = 2;
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

            if (id == Resource.Id.mouthBills)
            {
                ViewModel.MonthBillsCommand.Execute(null);
                return true;
            }

            return base.OnOptionsItemSelected(menuItem);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                ViewModel.ShowSettingsCommand.Execute();
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

            if (id == Resource.Id.openDoor)
            {
                Intent intent = PackageManager.GetLaunchIntentForPackage("com.companyname.opendoorapp");
                if (intent != null)
                {
                    // We found the activity now start the activity
                    intent.AddFlags(ActivityFlags.NewTask);
                    StartActivity(intent);
                }
                else
                {
                    // Bring user to the market or let them choose an app?
                    intent = new Intent(Intent.ActionView);
                    intent.AddFlags(ActivityFlags.NewTask);
                    intent.SetData(Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=com.companyname.opendoorapp"));
                    StartActivity(intent);
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
