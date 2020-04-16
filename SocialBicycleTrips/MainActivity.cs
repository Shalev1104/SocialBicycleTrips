﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Content;
using System;
using Model;
using Dal;
using Helper;

namespace SocialBicycleTrips
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        private ListView lvTrips;
        private Trips trips;
        private Adapters.TripAdapter tripAdapter;
        private TripsDB tripsDB;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            SetViews();
            tripsDB = new TripsDB();
            trips = tripsDB.GetAll();
            GenerateTrips();
            UploadUpdatedList();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.dots, menu);
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
        private void GenerateTrips()
        {
            User user = new User("avi", "bbb@gmail.com", "121212ss", new DateTime(2002, 11, 4), "0123456789");
            trips.Add(new Trip("somethere", "somewhere", new DateTime(2020, 12, 1,16,5,25), "Checker", new TripManager(user.Image, user.Name)));
        }

        private void UploadUpdatedList()
        {
            trips.Sort();
            tripAdapter = new Adapters.TripAdapter(this, Resource.Layout.activity_singleItemTripDesign, trips);
            lvTrips.Adapter = tripAdapter;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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