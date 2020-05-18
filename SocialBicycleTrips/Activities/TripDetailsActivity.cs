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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_tripDetails);
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
                    btnJoinOrAddParticipant.Text = "Add Participants";
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
        }

        public void SetFields()
        {
            tripNotes.Text = trip.Notes;
            dateTimeTrip.Text = trip.DateTime.ToString("dddd, dd MMMM yyyy HH:mm");
            tripManagerName.Text = tripManager.Name;
            try
            {
                tripManagerImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(tripManager.Image));
            }
            catch
            {
                tripManagerImage.SetImageBitmap(BitMapHelper.TransferMediaImages(tripManager.Image));
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
            trip.Participants.Sort();
            participantsAdapter = new Adapters.PeopleAdapter(this, Resource.Layout.activity_peopleList, trip.Participants,users);
            lvParticipants.Adapter = participantsAdapter;
        }
        private void BtnJoinOrAddParticipant_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activities.AddParticipantsActivity));
            intent.PutExtra("user", Serializer.ObjectToByteArray(user));
            intent.PutExtra("trip", Serializer.ObjectToByteArray(trip));
            StartActivityForResult(intent, 1);

        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == 1)
            {
                if(resultCode == Android.App.Result.Ok)
                {

                }
            }
        }
        private void Destination_Click(object sender, EventArgs e)
        {

        }

        private void StartingLocation_Click(object sender, EventArgs e)
        {

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
            }
        }
    }
}