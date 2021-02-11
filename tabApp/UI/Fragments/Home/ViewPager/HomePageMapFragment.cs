using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;

namespace tabApp.UI.Fragments.Home.ViewPager
{
    public class HomePageMapFragment : Android.Support.V4.App.Fragment, IOnMapReadyCallback, GoogleMap.IInfoWindowAdapter
    {
        private HomeViewModel viewModel;
        private GoogleMap _map;
        private MainActivity _activity;
        private SupportMapFragment _mapFragment;
        private LatLng _locationSaved;
        private MarkerOptions options;
        private Marker marker;
        private bool _selected;
        private bool _clientsAdded;

        public HomePageMapFragment(HomeViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomePageMapFragment, container, false);
            _activity = Activity as MainActivity;

            InitMapFragment();
            _mapFragment?.GetMapAsync(this);

            _locationSaved = new LatLng(0, 0);

            return view;
        }
        public override void OnResume()
        {
            base.OnResume();
            SetupMapIfNeeded();

            _activity.RequestCurrentLocationLoopUpdates();
            _activity.UpdateHomeMapLocation += UpdateHomeMapLocation;
        }

        private void SetPoints()
        {
            if(_clientsAdded) return;
            _clientsAdded = true;
            foreach(var client in viewModel.ClientsList)
            {
                if(client.Address.Coordenadas != null && !client.Address.Coordenadas.Equals("") && !client.Address.Coordenadas.Equals("null"))
                {
                    MarkerOptions options = new MarkerOptions()
                                                .SetTitle(client.Name)
                                                .SetSnippet(viewModel.GetClientDailyOrderDesc(client));

                    BitmapDrawable bitmapdraw = null;
                    if (viewModel.CheckClientHasExtraOrderToday(client))
                    {
                        bitmapdraw = (BitmapDrawable)Context.Resources.GetDrawable(Resource.Drawable.ic_alarm_png);
                    } else
                    {
                        bitmapdraw = (BitmapDrawable)Context.Resources.GetDrawable(Resource.Drawable.ic_client_png);
                    }
                    int height = 80;
                    int width = 80;
                    Bitmap b = bitmapdraw.Bitmap;
                    Bitmap smallMarker = Bitmap.CreateScaledBitmap(b, width, height, false);
                    options.SetIcon(BitmapDescriptorFactory.FromBitmap(smallMarker));

                    options.SetPosition(new LatLng(double.Parse(client.Address.Lat), double.Parse(client.Address.Lgt)));
                    Marker marker = _map?.AddMarker(options);
                }
            }
        }

        public override void OnPause()
        {
            base.OnPause();
            _activity.StopRequestCurrentLocationLoopUpdates();
            _activity.UpdateHomeMapLocation -= UpdateHomeMapLocation;
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

        private void MoveCamera(LatLng latlngall)
        {

            options = new MarkerOptions().SetPosition(_locationSaved).SetTitle("Nova localização");
            int height = 80;
            int width = 120;
            BitmapDrawable bitmapdraw = (BitmapDrawable)Context.Resources.GetDrawable(Resource.Drawable.car_logo);
            Bitmap b = bitmapdraw.Bitmap;
            Bitmap smallMarker = Bitmap.CreateScaledBitmap(b, width, height, false);
            options.SetIcon(BitmapDescriptorFactory.FromBitmap(smallMarker));
            // options.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_click_me));
            
            //_map.Clear();
            SetPoints();
            if (marker == null)
                marker = _map?.AddMarker(options);
            else
                marker.Position = latlngall;
            
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(latlngall);
            builder.Zoom(17);
            builder.Bearing(0);
            builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            _map?.MoveCamera(cameraUpdate);
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;

            _map.MarkerClick -= MapMarkerClick;
            _map.MarkerClick += MapMarkerClick;

            _map.SetInfoWindowAdapter(this);

            if (_locationSaved != null)
                MoveCamera(_locationSaved);
        }

        private void MapMarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            
            if (e.Marker.IsInfoWindowShown)
            {
                _selected = false;
                e.Marker.HideInfoWindow();
            } else
            {
                _selected = true;
                e.Marker.ShowInfoWindow();
            }
        }

        private void SetupMapIfNeeded()
        {
            if (_mapFragment != null) return;

            InitMapFragment();
            _mapFragment?.GetMapAsync(this);
        }

        private void UpdateHomeMapLocation(Location obj)
        {
            _locationSaved = new LatLng(obj.Latitude, obj.Longitude);
            if (_selected) return;
            MoveCamera(_locationSaved);
        }

        public View GetInfoContents(Marker marker)
        {
            View v = null;

            v = LayoutInflater.Inflate(Resource.Layout.custom_infowindow, null);

            TextView nameTxt = v.FindViewById<TextView>(Resource.Id.nameTxt);
            nameTxt.Text = marker.Title;

            TextView desc = v.FindViewById<TextView>(Resource.Id.desc);
            desc.Text = marker.Snippet;

            return v;
        }

        public View GetInfoWindow(Marker marker)
        {
            return null;
        }
    }
}