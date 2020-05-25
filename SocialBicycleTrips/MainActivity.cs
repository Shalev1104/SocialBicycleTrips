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
using Firebase.Auth;
using Xamarin.Facebook;
using Org.Apache.Http.Conn;
using System.Runtime.CompilerServices;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Gms.Tasks;
using Android.Telephony;

namespace SocialBicycleTrips
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        private ListView lvTrips;
        private Trips trips;
        private Adapters.TripAdapter tripAdapter;
        private User user;
        private Users users;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            FacebookSdk.SdkInitialize(ApplicationContext);
            SetViews();
            trips = new Trips().GetAllTrips();
            users = new Users().GetAllUsers();
            if (Intent.HasExtra("AddToMyTrips") && Intent.GetBooleanExtra("AddToMyTrips", false) == true)
            {
                user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
                user.MyTrips.Insert(new MyTrip(trips[trips.Count - 1].Id, user.Id));
            }
            UploadUpdatedList();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (Intent.HasExtra("user"))
            {
                user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
                if (user.IsSocialMediaLogon())
                {
                    MenuInflater.Inflate(Resource.Menu.socialMediaMenu, menu);
                }
                else
                {
                    MenuInflater.Inflate(Resource.Menu.userMenu, menu);
                }
            }
            else
            {
                MenuInflater.Inflate(Resource.Menu.guestMenu, menu);
            }
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent getUser = new Intent();
            if(Intent.HasExtra("user"))
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
                        StartActivityForResult(getUser,0);
                        item.SetChecked(true);
                        break;
                    }

                case Resource.Id.mnuMyFriends:
                    {
                        getUser.SetClass(this, typeof(Activities.MyFriendsActivity));
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
                        break;
                    }

                case Resource.Id.mnuChangePassword:
                    {
                        break;
                    }

                case Resource.Id.mnuSettings:
                    {
                        StartActivity(new Intent(this, typeof(Activities.SettingsActivity)));
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
                        StartActivityForResult(new Intent(this, typeof(Activities.LoginActivity)),2);
                        item.SetChecked(true);
                        break;
                    }
                case Resource.Id.mnuAddDate:
                    {
                        break;
                    }
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == 0)
            {
                if(resultCode == Android.App.Result.Ok)
                {
                    Trip trip = Serializer.ByteArrayToObject(data.GetByteArrayExtra("trip")) as Trip;
                    trips.Insert(trip);
                    StartActivity(new Intent(this, typeof(MainActivity)).PutExtra("user", Serializer.ObjectToByteArray(user)).PutExtra("AddToMyTrips",true));
                }
            }
            if(requestCode == 2)
            {
                if(resultCode == Android.App.Result.Ok)
                {
                    User user = Serializer.ByteArrayToObject(data.GetByteArrayExtra("user")) as User;
                    if (data.HasExtra("toAdd") && data.GetBooleanExtra("toAdd", false) == true)
                    {
                        users.Insert(user);
                    }
                    StartActivity(new Intent(this, typeof(MainActivity)).PutExtra("user", Serializer.ObjectToByteArray(user)));
                }
            }
        }

        private void UploadUpdatedList()
        {
            trips.Sort();
            tripAdapter = new Adapters.TripAdapter(this, Resource.Layout.activity_singleItemTripDesign, trips,users);
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

            Intent intent = new Intent(this, typeof(Activities.TripDetailsActivity));

            intent.PutExtra("trip", Serializer.ObjectToByteArray(trips[e.Position]));
            if (Intent.HasExtra("user"))
                intent.PutExtra("user", Serializer.ObjectToByteArray(user));

            StartActivity(intent);
        }
    }
}