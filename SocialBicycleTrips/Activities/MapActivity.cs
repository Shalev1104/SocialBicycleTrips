using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Location;
using Android.Gms.Location.Places.UI;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Google.Places;
using Helper;
using Model;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : FragmentActivity, IOnMapReadyCallback
    {
        GoogleMap mainMap;
        readonly string[] permissionGroupLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        const int requestLocationId = 0;
        SupportMapFragment mapFragment;
        LocationRequest locationRequest;
        FusedLocationProviderClient locationClient;
        Android.Locations.Location lastLocation;
        LocationCallbackHelper locationCallback;
        private TextView txtStart;
        private TextView txtEnd;
        private RelativeLayout layoutStart;
        private RelativeLayout layoutEnd;
        private RadioButton startRadio;
        private RadioButton endRadio;
        private ImageView centerMarker;
        private Button btnDone;
        private RelativeLayout returnToMyLocation;

        static int update_interval = 5;//seconds
        static int fastest_interval = 5;
        static int displacement = 3;//meters

        MapFunctionHelper mapHelper;
        LatLng startLocationLatLng;
        LatLng endLocationLatLng;

        int addressRequest = 1;
        bool takeAddressFromSearch;
        Model.Location first;
        Model.Location last;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_map);
            SetViews();
            mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
            CheckLocationPermission();
            CreateLocationRequest();
            GetMyLocation();
            StartLocationUpdates();
            // Create your application here
        }
        void SetViews()
        {
            txtStart = FindViewById<TextView>(Resource.Id.txtStartLocation);
            txtEnd = FindViewById<TextView>(Resource.Id.txtEndLocation);
            layoutStart = FindViewById<RelativeLayout>(Resource.Id.layoutStart);
            layoutEnd = FindViewById<RelativeLayout>(Resource.Id.layoutEnd);
            startRadio = FindViewById<RadioButton>(Resource.Id.startRadio);
            endRadio = FindViewById<RadioButton>(Resource.Id.endRadio);
            centerMarker = FindViewById<ImageView>(Resource.Id.centerMarker);
            btnDone = FindViewById<Button>(Resource.Id.btnFinished);
            returnToMyLocation = FindViewById<RelativeLayout>(Resource.Id.currentLocation);

            returnToMyLocation.Click += ReturnToMyLocation_Click;
            btnDone.Click += BtnDone_Click;
            startRadio.Click += StartRadio_Click;
            endRadio.Click += EndRadio_Click;
            layoutStart.Click += LayoutStart_Click;
            layoutEnd.Click += LayoutEnd_Click;
            if (!PlacesApi.IsInitialized)
            {
                PlacesApi.Initialize(this, "AIzaSyAH6n6XJq3ZCQSAKBSBNvQ12cBXltlOKvU");
            }
        }

        private void ReturnToMyLocation_Click(object sender, EventArgs e)
        {
            GetMyLocation();
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.PutExtra("firstLocation", Serializer.ObjectToByteArray(first));
            intent.PutExtra("lastLocation", Serializer.ObjectToByteArray(last));
            SetResult(Android.App.Result.Ok, intent);
            Finish();
        }

        private void EndRadio_Click(object sender, EventArgs e)
        {
            addressRequest = 2;
            startRadio.Checked = false;
            endRadio.Checked = true;
            takeAddressFromSearch = false;
            centerMarker.SetColorFilter(Android.Graphics.Color.Red);
            btnDone.Visibility = ViewStates.Visible;
        }

        private void StartRadio_Click(object sender, EventArgs e)
        {
            addressRequest = 1;
            startRadio.Checked = true;
            endRadio.Checked = false;
            takeAddressFromSearch = false;
            centerMarker.SetColorFilter(Android.Graphics.Color.Green);
        }

        private void LayoutEnd_Click(object sender, EventArgs e)
        {
            List<Place.Field> fields = new List<Place.Field>();
            fields.Add(Place.Field.Id);
            fields.Add(Place.Field.Name);
            fields.Add(Place.Field.LatLng);
            fields.Add(Place.Field.Address);
            Intent intent = new Autocomplete.IntentBuilder(AutocompleteActivityMode.Overlay, fields).Build(this);
            StartActivityForResult(intent, 2);
        }

        private void LayoutStart_Click(object sender, EventArgs e)
        {
            List<Place.Field> fields = new List<Place.Field>();
            fields.Add(Place.Field.Id);
            fields.Add(Place.Field.Name);
            fields.Add(Place.Field.LatLng);
            fields.Add(Place.Field.Address);
            Intent intent = new Autocomplete.IntentBuilder(AutocompleteActivityMode.Overlay, fields).Build(this);
            StartActivityForResult(intent, 1);
        }

        private async void MainMap_CameraIdle(object sender, EventArgs e)
        {
            if (!takeAddressFromSearch)
            {
                if (addressRequest == 1)
                {
                    startLocationLatLng = mainMap.CameraPosition.Target;
                    string address = await mapHelper.FindCordinateAddress(startLocationLatLng);
                    txtStart.Text = address;
                    first = new Model.Location(address, startLocationLatLng);
                    
                }
                else if (addressRequest == 2)
                {
                    endLocationLatLng = mainMap.CameraPosition.Target;
                    string address = await mapHelper.FindCordinateAddress(endLocationLatLng);
                    txtEnd.Text = address;
                    btnDone.Visibility = ViewStates.Visible;
                    last = new Model.Location(address, endLocationLatLng);
                }
            }
        }

        bool CheckLocationPermission()
        {
            bool permissionGranted = false;
            if(ActivityCompat.CheckSelfPermission(this,Manifest.Permission.AccessFineLocation) != Permission.Granted && ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
            {
                permissionGranted = false;
                RequestPermissions(permissionGroupLocation, requestLocationId);
            }
            else
            {
                permissionGranted = true;
            }
            return permissionGranted;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if(grantResults.Length > 1)
            {
                return;
            }
            if(grantResults[0] == (int)Permission.Granted)
            {
                StartLocationUpdates();
            }
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            if (requestCode == 1)
            {
                if (resultCode == Android.App.Result.Ok)
                {
                    takeAddressFromSearch = true;
                    startRadio.Checked = false;
                    endRadio.Checked = false;

                    var place = Autocomplete.GetPlaceFromIntent(data);
                    startLocationLatLng = place.LatLng;
                    txtStart.Text = place.Name;

                    mainMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(place.LatLng, 15));
                    centerMarker.SetColorFilter(Android.Graphics.Color.Green);
                    first = new Model.Location(place.Name, place.Address, place.LatLng.Latitude, place.LatLng.Longitude);
                }
            }
            else if (requestCode == 2)
            {
                if (resultCode == Android.App.Result.Ok)
                {
                    takeAddressFromSearch = true;
                    startRadio.Checked = false;
                    endRadio.Checked = false;

                    var place = Autocomplete.GetPlaceFromIntent(data);
                    endLocationLatLng = place.LatLng;
                    txtEnd.Text = place.Name;

                    mainMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(place.LatLng, 15));
                    centerMarker.SetColorFilter(Android.Graphics.Color.Red);
                    last = new Model.Location(place.Name, place.Address, place.LatLng.Latitude, place.LatLng.Longitude);
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
        void CreateLocationRequest()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetInterval(update_interval);
            locationRequest.SetFastestInterval(fastest_interval);
            locationRequest.SetSmallestDisplacement(displacement);
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationClient = LocationServices.GetFusedLocationProviderClient(this);
            locationCallback = new LocationCallbackHelper();
            locationCallback.MyLocation += LocationCallback_MyLocation;
        }

        private void LocationCallback_MyLocation(object sender, LocationCallbackHelper.OnLocationCapturedEventArgs e)
        {
            lastLocation = e.Location;
            LatLng myPosition = new LatLng(lastLocation.Latitude, lastLocation.Longitude);
            mainMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(myPosition, 12));
        }

        private async void GetMyLocation()
        {
            if (!CheckLocationPermission())
            {
                return;
            }
            lastLocation = await locationClient.GetLastLocationAsync();
            if(lastLocation != null)
            {
                LatLng myPosition = new LatLng(lastLocation.Latitude, lastLocation.Longitude);
                mainMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(myPosition, 17));
            }
        }
        void StartLocationUpdates()
        {
            if (CheckLocationPermission())
            {
                locationClient.RequestLocationUpdates(locationRequest, locationCallback,null);
            }
        }
        void StopLocationUpdates()
        {
            if(locationClient != null  && locationCallback != null)
            {
                locationClient.RemoveLocationUpdates(locationCallback);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mainMap = googleMap;
            mainMap.CameraIdle += MainMap_CameraIdle;
            string mapkey = "AIzaSyAH6n6XJq3ZCQSAKBSBNvQ12cBXltlOKvU";
            mapHelper = new MapFunctionHelper(mapkey, mainMap);
        }
    }
}