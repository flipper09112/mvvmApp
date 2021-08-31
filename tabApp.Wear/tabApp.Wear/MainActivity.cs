using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Activity;

namespace tabApp.Wear
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : WearableActivity
    {
        private ProgressBar _loadingView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            _loadingView = FindViewById<ProgressBar>(Resource.Id.loadingView);
            SetAmbientEnabled();
        }

        protected override void OnResume()
        {
           /* base.OnResume();

            if (_alreadyStarted || _clientsManagerService?.ClientsList?.Count > 0)
                return;
            _alreadyStarted = true;*/
            //IsBusy = true;

            //---------DB init-------------
            // await _dbService.StartAsync();
            //Here can write xls to sqllite
            // _dataBaseService.InsertAllDataFromXls(_clientsManagerService.ClientsList, _productsManagerService.ProductsList);

            //await _dataBaseService.LoadDataBase();
            //end DB init

            //UpdateUiHomePage?.Invoke(null, null);
            //IsBusy = false;
        }
    }
}


