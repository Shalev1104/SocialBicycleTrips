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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_myTrips);
            SetViews();
            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
            trips = new Trips().GetAllTrips();
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
            Intent getUser = new Intent();
            if (Intent.HasExtra("user"))
                getUser.PutExtra("user", Serializer.ObjectToByteArray(user));

            switch (item.ItemId)
            {
                case Resource.Id.mnuBrowseTrips:
                    {
                        getUser.SetClass(this, typeof(MainActivity));
                        StartActivity(getUser);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyTrips:
                    {
                        getUser.SetClass(this, typeof(Activities.MyTripsActivity));
                        StartActivity(getUser);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuCreateTrip:
                    {
                        getUser.SetClass(this, typeof(Activities.CreateTripActivity));
                        StartActivityForResult(getUser, 0);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyFriends:
                    {
                        getUser.SetClass(this, typeof(MainActivity));
                        StartActivity(getUser);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyProfile:
                    {
                        getUser.SetClass(this, typeof(Activities.ProfileActivity));
                        getUser.PutExtra("myself", true);
                        StartActivity(getUser);
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
                case Resource.Id.mnuDisconnect:
                    {
                        if (user.IsSocialMediaLogon())
                        {
                            Intent intent = new Intent(this, typeof(Activities.LoginActivity));
                            intent.PutExtra("social media disconnect", true);
                            StartActivityForResult(intent, 1);
                        }
                        StartActivity(new Intent(this, typeof(MainActivity)));
                        item.SetChecked(true);
                        break;
                    }
                case Resource.Id.mnuLogin:
                    {
                        StartActivity(new Intent(this, typeof(Activities.LoginActivity)));
                        item.SetChecked(true);
                        break;
                    }
                case Resource.Id.mnuAddDate:
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
            myTripsAdapter = new Adapters.MyTripsAdapter(this, Resource.Layout.activity_singleItemTripDesign, user.MyTrips.GetAllMyTrips(user.Id),trips);
            lvMyTrips.Adapter = myTripsAdapter;
        }
        public void SetViews()
        {
            lvMyTrips = FindViewById<ListView>(Resource.Id.lvMyTrips);
            lvMyTrips.ItemClick += LvTrips_ItemClick;
        }
        private void LvTrips_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            Intent intent = new Intent(this, typeof(Activities.TripDetailsActivity));

            intent.PutExtra("trip", Serializer.ObjectToByteArray(trips[e.Position]));
            intent.PutExtra("user", Serializer.ObjectToByteArray(user));

            StartActivity(intent);
        }
    }
}