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
        private User user;
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
        protected override void OnStop()
        {
            base.OnStop();
            if (Intent.HasExtra("user") && Settings.RememberMe)
            {
                user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                ISharedPreferencesEditor editor = pref.Edit();
                editor.PutString("user", Android.Util.Base64.EncodeToString(Serializer.ObjectToByteArray(user), Android.Util.Base64.Default));
                editor.PutInt("userId", user.Id);
                editor.PutInt("OngoingTrips", user.UpcomingTrips);
                editor.PutInt("CompletedTrips", user.CompletedTrips);
                editor.Apply();
            }
        }
    }
}