using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Model;
using Helper;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "MyTripsActivity")]
    public class MyTripsActivity : Activity
    {
        private ListView lvMyTrips;
        private Trips trips;
        private Adapters.MyTripsAdapter myTripsAdapter;
        private User user;
        private Users users;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_myTrips);
            SetViews();
            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
            trips = new Trips().GetAllCurrentTrips();
            users = new Users().GetAllUsers();
            UploadUpdatedList();
            // Create your application here
        }
        private void UploadUpdatedList()
        {
            if(trips != null)
            {
                trips.Sort();
                myTripsAdapter = new Adapters.MyTripsAdapter(this, Resource.Layout.activity_singleItemTripDesign, user.MyTrips.GetAllMyCurrentTrips(user.Id), trips,users);
                lvMyTrips.Adapter = myTripsAdapter;
            }
        }
        public void SetViews()
        {
            lvMyTrips = FindViewById<ListView>(Resource.Id.lvMyTrips);
            lvMyTrips.ItemClick += LvTrips_ItemClick;
        }
        private void LvTrips_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            Intent intent = new Intent(this, typeof(Activities.TripDetailsActivity));

            intent.PutExtra("trip", Serializer.ObjectToByteArray(trips.GetTripByID(user.MyTrips.GetAllMyCurrentTrips(user.Id)[e.Position].TripID)));
            intent.PutExtra("user", Serializer.ObjectToByteArray(user));

            StartActivity(intent);
        }
    }
}