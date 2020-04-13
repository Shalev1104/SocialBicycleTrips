using Android.App;
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
        private TripsAdapter tripsAdapter;
        private TripsDB tripDB;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            SetViews();
            tripDB = new TripsDB();
            trips = tripDB.GetAll();
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
            Intent intent;
            switch (item.ItemId)
            {
                case Resource.Id.mnuBrowseTrips:
                    {
                        intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyTrips:
                    {
                        intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuCreateTrip:
                    {
                        intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyFriends:
                    {
                        intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuUpdateProfile:
                    {
                        intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuChangePassword:
                    {
                        intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuSettings:
                    {
                        intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        item.SetChecked(true);
                        break;
                    }
            }
            return base.OnOptionsItemSelected(item);
        }
        private void GenerateTrips()
        {
            //trips.Add(new Trip("Rishon Lezion", "Tel Aviv", new DateTime(), "Heading to a long trip", new TripManager("http://www.mac-wallpapers.com/bulkupload/wallpapers/Apple%20Wallpapers/apple-black-logo-wallpaper.jpg","Albert Jax")));
        }

        private void UploadUpdatedList()
        {
            trips.Sort();
            tripsAdapter = new TripsAdapter(this, Resource.Layout.activity_tripListDesigner, trips);
            lvTrips.Adapter = tripsAdapter;
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
            throw new NotImplementedException();
        }
    }
}