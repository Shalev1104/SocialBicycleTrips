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

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "MyTripsActivity")]
    public class MyTripsActivity : Activity
    {
        private ListView lvTrips;
        private Trips trips;
        private Adapters.TripAdapter tripAdapter;
        private TripsDB tripsDB;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetViews();
            tripsDB = new TripsDB();
            trips = tripsDB.GetAll();
            UploadUpdatedList();
            // Create your application here
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.userMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mnuBrowseTrips:
                    {
                        StartActivity(new Intent(this, typeof(MainActivity)));
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyTrips:
                    {
                        StartActivity(new Intent(this, typeof(Activities.MyTripsActivity)));
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuCreateTrip:
                    {
                        StartActivity(new Intent(this, typeof(MainActivity)));
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyFriends:
                    {
                        StartActivity(new Intent(this, typeof(MainActivity)));
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyProfile:
                    {
                        StartActivity(new Intent(this, typeof(Activities.ProfileActivity)));
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuUpdateProfile:
                    {
                        StartActivity(new Intent(this, typeof(MainActivity)));
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuChangePassword:
                    {
                        StartActivity(new Intent(this, typeof(MainActivity)));
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuSettings:
                    {
                        StartActivity(new Intent(this, typeof(MainActivity)));
                        item.SetChecked(true);
                        break;
                    }
            }
            return base.OnOptionsItemSelected(item);
        }
        private void UploadUpdatedList()
        {
            trips.Sort();
            tripAdapter = new Adapters.TripAdapter(this, Resource.Layout.activity_singleItemTripDesign, trips);
            lvTrips.Adapter = tripAdapter;
        }
        public void SetViews()
        {
            lvTrips = FindViewById<ListView>(Resource.Id.lvTrips);
            lvTrips.ItemClick += LvTrips_ItemClick;
        }
        private void LvTrips_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

        }
    }
}