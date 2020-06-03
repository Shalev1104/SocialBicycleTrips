using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Helper;
using Android.Support.V4.App;
using Model;
using Android.Gms.Maps.Model;
using Bumptech.Glide;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "TripDetailsActivity")]
    public class TripDetailsActivity : FragmentActivity,IOnMapReadyCallback
    {
        private GoogleMap map;
        private SupportMapFragment mapFragment;
        MapFunctionHelper mapHelper;
        private Trip trip;
        private TextView tripManagerName;
        private ImageView tripManagerImage;
        private TextView tripNotes;
        private TextView dateTimeTrip;
        private Button startingLocation;
        private Button destination;
        private TextView txtDistance;
        private Button btnJoinOrAddParticipant;
        private ListView lvParticipants;
        private Adapters.PeopleAdapter participantsAdapter;
        private Users users;
        private User user;
        TripManager tripManager;
        Model.Location start;
        Model.Location end;
        private ProgressDialog loadingDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_tripDetails);
            loadingDialog = new ProgressDialog(this);
            loadingDialog.SetTitle("Loading");
            loadingDialog.SetCanceledOnTouchOutside(false);
            loadingDialog.Show();
            mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.mapDistanceCalculator);
            mapFragment.GetMapAsync(this);
            trip = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("trip")) as Trip;
            start = Serializer.ByteArrayToObject(trip.StartingLocation) as Model.Location;
            end = Serializer.ByteArrayToObject(trip.FinalLocation) as Model.Location;
            tripManager = Serializer.ByteArrayToObject(trip.TripManager) as TripManager;
            users = new Users().GetAllUsers();
            SetViews();
            SetFields();
            if (Intent.HasExtra("user"))
            {
                user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
                if (tripManager.Id == user.Id)
                {
                    btnJoinOrAddParticipant.Visibility = ViewStates.Visible;
                    btnJoinOrAddParticipant.Text = "Add Participants";
                }
                else if(!IsInTrip(user.Id,trip.Id))
                {
                    btnJoinOrAddParticipant.Visibility = ViewStates.Visible;
                    btnJoinOrAddParticipant.Text = "Join";
                }
                else
                {
                    btnJoinOrAddParticipant.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                btnJoinOrAddParticipant.Visibility = ViewStates.Gone;
            }
            UploadUpdatedList();
            // Create your application here
        }

        private bool IsInTrip(int userID, int tripID)
        {
            MyTrips myTrips = user.MyTrips.GetAllMyCurrentTrips(userID);
            if(myTrips != null)
            {
                for(int i = 0; i < myTrips.Count; i++)
                {
                    if (myTrips[i].TripID.Equals(tripID))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private async void DrawTripOnMap()
        {
            string json;
            LatLng firstCoordinate = new LatLng(start.Latitude, start.Longitude);
            LatLng lastCoordinate = new LatLng(end.Latitude, end.Longitude);
            json = await mapHelper.GetDirectionJsonAsync(firstCoordinate, lastCoordinate);
            if (!string.IsNullOrEmpty(json))
            {
                mapHelper.DrawTripOnMap(json);
                txtDistance.Text = mapHelper.distance.ToString();
            }
            else
            {
                txtDistance.Text = "Unable to draw route";
                Toast.MakeText(this, txtDistance.Text, ToastLength.Long).Show();
            }
            loadingDialog.Dismiss();
        }

        public void SetFields()
        {
            tripNotes.Text = trip.Notes;
            dateTimeTrip.Text = trip.DateTime.ToString("dddd, dd MMMM yyyy HH:mm");
            tripManagerName.Text = tripManager.Name;
            if (!users.GetUserByID(tripManager.Id).IsSocialMediaLogon())
            {
                tripManagerImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(tripManager.Image));
            }
            else
            {
                Glide.With(this).Load(tripManager.Image).Error(Resource.Drawable.StandardProfileImage).Into(tripManagerImage);
            }
        }
        public void SetViews()
        {
            tripManagerName = FindViewById<TextView>(Resource.Id.txtNameOfTripDetails);
            tripNotes = FindViewById<TextView>(Resource.Id.txtNotesOfTripDetails);
            dateTimeTrip = FindViewById<TextView>(Resource.Id.txtDateTimeOfTripDetails);
            startingLocation = FindViewById<Button>(Resource.Id.btnStartingLocationOfTripDetails);
            destination = FindViewById<Button>(Resource.Id.btnDestinationOfTripDetails);
            txtDistance = FindViewById<TextView>(Resource.Id.txtDistanceOfTripDetails);
            btnJoinOrAddParticipant = FindViewById<Button>(Resource.Id.btnAddParticipantsOrJoinOfTripDetails);
            lvParticipants = FindViewById<ListView>(Resource.Id.lvParticipants);
            tripManagerImage = FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.profileImageOfTripDetails);

            startingLocation.Click += StartingLocation_Click;
            destination.Click += Destination_Click;
            btnJoinOrAddParticipant.Click += BtnJoinOrAddParticipant_Click;
            tripManagerName.Click += ToProfileActivity_Click;
            tripManagerImage.Click += ToProfileActivity_Click;
            lvParticipants.ItemClick += LvParticipants_ItemClick;
            lvParticipants.ItemLongClick += LvParticipants_ItemLongClick;

        }

        private void LvParticipants_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            if (Intent.HasExtra("user"))
            {
                if (tripManager.Id == user.Id)
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDiag = new Android.Support.V7.App.AlertDialog.Builder(this);

                    alertDiag.SetTitle("Confirm delete");
                    alertDiag.SetMessage("Once deleted the move cannot be undone");

                    alertDiag.SetCancelable(true);

                    alertDiag.SetPositiveButton("Delete", (senderAlert, args)
                           => {
                               Participant participant = new Participants().GetAllParticipants(trip.Id)[e.Position];

                               trip.Participants.Delete(participant);
                               Toast.MakeText(this, "Deleted", ToastLength.Long).Show();
                               UploadUpdatedList();

                               alertDiag.Dispose();
                           });

                    alertDiag.SetNegativeButton("Cancel", (senderAlert, args)
                     => {
                         alertDiag.Dispose();
                     });

                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
            }
        }

        private void LvParticipants_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activities.ProfileActivity));
            User profile = users.GetUserByID(new Participants().GetAllParticipants(trip.Id)[e.Position].UserID);
            intent.PutExtra("profile", Serializer.ObjectToByteArray(profile));
            if (Intent.HasExtra("user"))
            {
                intent.PutExtra("user", Serializer.ObjectToByteArray(user));

                if (profile.Equals(user))
                {
                    intent.PutExtra("myself", true);
                }
            }
            StartActivity(intent);
        }

        private void ToProfileActivity_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activities.ProfileActivity));
            User profile = users.GetUserByID(tripManager.Id);
            intent.PutExtra("profile", Serializer.ObjectToByteArray(profile));
            if (Intent.HasExtra("user"))
            {
                intent.PutExtra("user", Serializer.ObjectToByteArray(user));

                if (profile.Equals(user))
                {
                    intent.PutExtra("myself", true);
                }
            }
            StartActivity(intent);
        }

        private void UploadUpdatedList()
        {
            trip.Participants = new Participants().GetAllParticipants(trip.Id);
            if(trip.Participants != null)
            {
                trip.Participants.Sort();
                participantsAdapter = new Adapters.PeopleAdapter(this, Resource.Layout.activity_peopleList, trip.Participants, users);
                lvParticipants.Adapter = participantsAdapter;
            }
        }
        private void BtnJoinOrAddParticipant_Click(object sender, EventArgs e)
        {
            if(btnJoinOrAddParticipant.Text.Equals("Add Participants"))
            {
                Intent intent = new Intent(this, typeof(Activities.AddParticipantsActivity));
                intent.PutExtra("user", Serializer.ObjectToByteArray(user));
                intent.PutExtra("trip", Serializer.ObjectToByteArray(trip));
                StartActivityForResult(intent, 1);
            }
            else if (btnJoinOrAddParticipant.Text.Equals("Join"))
            {
                user.MyTrips.Insert(new MyTrip(trip.Id, user.Id));
                trip.Participants.Insert(new Participant(user.Id, trip.Id));
                Toast.MakeText(this, "Joined to the trip", ToastLength.Long).Show();
                btnJoinOrAddParticipant.Visibility = ViewStates.Gone;
                UploadUpdatedList();
            }

        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == 1)
            {
                if(resultCode == Android.App.Result.Ok)
                {
                    Users usersToAdd = Serializer.ByteArrayToObject(data.GetByteArrayExtra("selected")) as Users;
                    for(int i = 0; i < usersToAdd.Count; i++)
                    {
                        trip.Participants.Insert(new Participant(usersToAdd[i].Id, trip.Id));
                        user.MyTrips.Insert(new MyTrip(trip.Id, usersToAdd[i].Id));
                    }
                    Toast.MakeText(this, "participants has been added successfully", ToastLength.Long).Show();
                    UploadUpdatedList();
                }
            }
        }
        private void Destination_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activities.LocationDetails));
            intent.PutExtra("location", Serializer.ObjectToByteArray(end));
            StartActivity(intent);
        }

        private void StartingLocation_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activities.LocationDetails));
            intent.PutExtra("location", Serializer.ObjectToByteArray(start));
            StartActivity(intent);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            SetMapStyle(googleMap);
            map = googleMap;
            mapHelper = new MapFunctionHelper("AIzaSyAH6n6XJq3ZCQSAKBSBNvQ12cBXltlOKvU", map);
            DrawTripOnMap();
        }
        public void SetMapStyle(GoogleMap googleMap)
        {
            switch (Settings.MapStyle)
            {
                case "Standard":
                    googleMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.StandardMapStyle));
                    break;
                case "Silver":
                    googleMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.SilverMapStyle));
                    break;
                case "Retro":
                    googleMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.RetroMapStyle));
                    break;
                case "Dark":
                    googleMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.DarkMapStyle));
                    break;
                case "Night":
                    googleMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.NightMapStyle));
                    break;
                case "Aubergine":
                    googleMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.AubergineMapStyle));
                    break;
                case "Hybrid":
                    googleMap.MapType = GoogleMap.MapTypeHybrid;
                    break;
                case "Terrain":
                    googleMap.MapType = GoogleMap.MapTypeTerrain;
                    break;
            }
        }
    }
}