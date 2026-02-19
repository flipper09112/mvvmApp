
using Android;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using tabApp.Core.ViewModels;

namespace tabApp.UI.Fragments.EditClient
{
    public class EditClientMapFragment : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        private EditClientViewModel viewModel;
        private SupportMapFragment _mapFragment;
        private GoogleMap _map;
        private MainActivity _activity;
        private Button _getCurrentLocationButton;
        private TextView _locationLabel;
        private LatLng _locationSaved;
        private bool _setNewLocation;

        public EditClientMapFragment(EditClientViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MapViewEditClient, container, false);
            _activity = Activity as MainActivity;
            _getCurrentLocationButton = view.FindViewById<Button>(Resource.Id.getCurrentLocationButton);
            _locationLabel = view.FindViewById<TextView>(Resource.Id.locationLabel);

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
            _getCurrentLocationButton.Click += GetCurrentLocationButtonClick;
            _activity.LocationEvent += SetLocation;
            SetUi();
        }

        private void SetUi()
        {
            _locationLabel.Text = viewModel.NewLocation;
           
            if(!viewModel.Client.Address.Coordenadas.Equals("null"))
            {
                _locationSaved = new LatLng(double.Parse(viewModel.Client.Address.Lat), double.Parse(viewModel.Client.Address.Lgt));
            }
        }

        private void GetCurrentLocationButtonClick(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            _setNewLocation = true;
        }
        private void SetLocation(Location location)
        {
            if (!_setNewLocation) return;
            _locationLabel.Text = location.Latitude + "," + location.Longitude;
            _locationLabel.SetBackgroundResource(Resource.Color.green);
            viewModel.NewLocation = _locationLabel.Text;
            viewModel.IsBusy = false;
            _setNewLocation = false;

            _locationSaved = new LatLng(location.Latitude, location.Longitude);
            MoveCamera(_locationSaved);
        }

        private void MoveCamera(LatLng latlngall)
        {
            MarkerOptions options = new MarkerOptions().SetPosition(_locationSaved).SetTitle("Nova localização");
            //options.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.marker));
            _map.Clear();
            _map?.AddMarker(options);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(latlngall);
            builder.Zoom(18);
            builder.Bearing(155);
            builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            _map?.MoveCamera(cameraUpdate);
        }

        public override void OnPause()
        {
            base.OnPause();
            _getCurrentLocationButton.Click -= GetCurrentLocationButtonClick;
            _activity.LocationEvent -= SetLocation;
        }

        #region set Map functions
        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;

            if (_locationSaved != null)
                MoveCamera(_locationSaved);
        }
        private void SetupMapIfNeeded()
        {
            if (_mapFragment != null) return;

            InitMapFragment();
            _mapFragment?.GetMapAsync(this);
        }
        #endregion

    }
}
