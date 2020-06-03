using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Helper;
using Model;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "LocationDetails")]
    public class LocationDetails : Activity
    {
        private TextView locationAddress;
        private TextView locationName;
        private TextView locationCoordiante;
        private Model.Location location;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_locationDetails);
            location = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("location")) as Model.Location;
            SetViews();
            SetFields();
            // Create your application here
        }
        public void SetViews()
        {
            locationAddress = FindViewById<TextView>(Resource.Id.txtPlaceAddress);
            locationName = FindViewById<TextView>(Resource.Id.txtPlaceName);
            locationCoordiante = FindViewById<TextView>(Resource.Id.txtPlaceCoordinate);
        }
        public void SetFields()
        {
            locationAddress.Text = location.Address;
            locationCoordiante.Text = location.Latitude.ToString() + "," + location.Longitude.ToString();
            if(location.Name != null)
            {
                locationName.Text = location.Name;
            }
            else
            {
                locationName.Text = "unknown";
            }
        }
    }
}