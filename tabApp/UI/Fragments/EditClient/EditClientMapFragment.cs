
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views;
using tabApp.Core.ViewModels;

namespace tabApp.UI.Fragments.EditClient
{
    public class EditClientMapFragment : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        private EditClientViewModel viewModel;
        private GoogleMap map;
        private SupportMapFragment _mapFragment;
        private GoogleMap _map;

        public EditClientMapFragment(EditClientViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MapViewEditClient, container, false);


            InitMapFragment();
            _mapFragment?.GetMapAsync(this);
            return view;
        }
        private void InitMapFragment()
        {
            _mapFragment = ChildFragmentManager.FindFragmentByTag("map") as SupportMapFragment;
            if (_mapFragment != null) return;

            var mapOptions = new GoogleMapOptions()
                .InvokeMapType(GoogleMap.MapTypeNormal)
                .InvokeZoomControlsEnabled(false)
                .InvokeCompassEnabled(true);

            Android.Support.V4.App.FragmentTransaction fragTx = ChildFragmentManager.BeginTransaction();
            _mapFragment = SupportMapFragment.NewInstance(mapOptions);
            fragTx.Add(Resource.Id.map, _mapFragment, "map");
            fragTx.Commit();
        }
        public override void OnResume()
        {
            base.OnResume();
            SetupMapIfNeeded();
        }
        private void SetupMapIfNeeded()
        {
            if (_mapFragment != null) return;

            InitMapFragment();
            _mapFragment?.GetMapAsync(this);
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;

            var centerPoint = new LatLng(39.6189756, -7.3626493);
            googleMap.UiSettings.CompassEnabled = true;
            var cameraUpdate = CameraUpdateFactory.NewLatLngZoom(centerPoint, 5);
            _map.MoveCamera(cameraUpdate);
        }

    }
}
