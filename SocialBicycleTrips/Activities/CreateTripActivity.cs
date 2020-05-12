using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Model;
using Helper;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "CreateTripActivity")]
    public class CreateTripActivity : Activity
    {
        private EditText edtNotes;
        private Button btnDate;
        private Button btnTime;
        private Button locationChooser;
        private Button btnAddParticipants;
        private Button btnCreateTrip;
        private TimePickerDialog timePicker;
        private DatePickerDialog datePicker;
        private Trip trip;
        private User user;
        private DateTime date;
        private DateTime time;
        Model.Location firstLocation;
        Model.Location lastLocation;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_createTrip);
            SetViews();

            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;

            // Create your application here
        }
        public void SetViews()
        {
            edtNotes = FindViewById<EditText>(Resource.Id.edtNotesTripCreator);
            btnDate = FindViewById<Button>(Resource.Id.btnDateTripCreator);
            btnTime = FindViewById<Button>(Resource.Id.btnTimeTripCreator);
            locationChooser = FindViewById<Button>(Resource.Id.btnLocationChooser);
            btnAddParticipants = FindViewById<Button>(Resource.Id.btnaddParticipantsTripCreator);
            btnCreateTrip = FindViewById<Button>(Resource.Id.btnCreateTripCreator);

            btnDate.Click += BtnDate_Click;
            btnTime.Click += BtnTime_Click;
            locationChooser.Click += LocationChooser_Click;
            btnAddParticipants.Click += BtnAddParticipants_Click;
            btnCreateTrip.Click += BtnCreateTrip_Click;
        }

        private void LocationChooser_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activities.MapActivity));
            StartActivityForResult(intent, 0);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == 0)
            {
                if(resultCode == Android.App.Result.Ok)
                {
                    firstLocation = Serializer.ByteArrayToObject(data.GetByteArrayExtra("firstLocation")) as Model.Location;
                    lastLocation = Serializer.ByteArrayToObject(data.GetByteArrayExtra("lastLocation")) as Model.Location;
                }
            }
        }

        private void BtnCreateTrip_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                DateTime dateTime = new DateTime(date.Date.Year, date.Date.Month, date.Date.Day, time.Hour, time.Minute,0);
                byte[] first = Serializer.ObjectToByteArray(firstLocation);
                byte[] last = Serializer.ObjectToByteArray(lastLocation);
                byte[] manager = Serializer.ObjectToByteArray(new TripManager(user.Image, user.Name));
                trip = new Trip(first, last, dateTime, edtNotes.Text, manager);
                Toast.MakeText(this, "created successfully", ToastLength.Long);
                Intent intent = new Intent();
                intent.PutExtra("trip", Serializer.ObjectToByteArray(trip));
                SetResult(Android.App.Result.Ok, intent);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "please tyoe all the requested fields", ToastLength.Long).Show();
            }
        }

        private bool IsValid()
        {
            return btnDate != null && !btnDate.Text.Equals("") && btnTime != null && !btnTime.Text.Equals("") && firstLocation != null && lastLocation != null;
        }

        private void BtnAddParticipants_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnTime_Click(object sender, EventArgs e)
        {
            PerformTimePickerDialog();
        }

        private void PerformTimePickerDialog()
        {
            DateTime d = DateTime.Now;

            timePicker = new TimePickerDialog(this, OnTimeClick, d.Hour, d.Minute, true);
            timePicker.Show();
        }

        private void OnTimeClick(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            time = new DateTime();
            time.AddHours(e.HourOfDay);
            time.AddMinutes(e.Minute);
            btnTime.Text = e.HourOfDay + ":" + e.Minute;
        }


        private void BtnDate_Click(object sender, EventArgs e)
        {
            PerformDatePickerDialog();
        }

        private void PerformDatePickerDialog()
        {
            DateTime today = DateTime.Today;

            datePicker = new DatePickerDialog(this, OnDateClick, today.Year, today.Month - 1, today.Day);
            datePicker.Show();
        }

        private void OnDateClick(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            date = new DateTime(e.Date.Year, e.Date.Month, e.Date.Day);
            btnDate.Text = e.Date.Day + "/" + e.Date.Month + "/" + e.Date.Year;
        }

    }
}