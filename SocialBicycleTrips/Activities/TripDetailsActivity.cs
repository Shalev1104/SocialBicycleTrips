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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_tripDetails);
            trip = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("trip")) as Trip;
            users = new Users().GetAllUsers();

            SetViews();
            DrawTripOnMap();
            SetFields();
            if (Intent.HasExtra("user"))
            {
                user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
                if (trip.TripManager.Id == user.Id)
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
            LatLng firstCoordinate = new LatLng(trip.StartingLocation.Latitude, trip.StartingLocation.Longitude);
            LatLng lastCoordinate = new LatLng(trip.FinalLocation.Latitude, trip.FinalLocation.Longitude);
            json = await mapHelper.GetDirectionJsonAsync(firstCoordinate, lastCoordinate);
            if (!string.IsNullOrEmpty(json))
            {
                mapHelper.DrawTripOnMap(json);
            }
        }

        public void SetFields()
        {
            tripNotes.Text = trip.Notes;
            dateTimeTrip.Text = trip.DateTime.Date.ToString() + trip.DateTime.Date.DayOfWeek + trip.DateTime.TimeOfDay;
            tripManagerName.Text = trip.TripManager.Name;
            tripManagerImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(trip.TripManager.Image));
            txtDistance.Text = mapHelper.distance.ToString();
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

            mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.mapDistanceCalculator);
            mapFragment.GetMapAsync(this);

            startingLocation.Click += StartingLocation_Click;
            destination.Click += Destination_Click;
            btnJoinOrAddParticipant.Click += BtnJoinOrAddParticipant_Click;
        }
        private void UploadUpdatedList()
        {
            trip.Participants.Sort();
            participantsAdapter = new Adapters.PeopleAdapter(this, Resource.Layout.activity_peopleList, trip.Participants,users);
            lvParticipants.Adapter = participantsAdapter;
        }
        private void BtnJoinOrAddParticipant_Click(object sender, EventArgs e)
        {

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
            throw new NotImplementedException();
        }

        private void StartingLocation_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            mapHelper = new MapFunctionHelper("AIzaSyAH6n6XJq3ZCQSAKBSBNvQ12cBXltlOKvU", map);
        }
    }
}