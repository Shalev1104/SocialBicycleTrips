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
using Google.Places;
using Android.Gms.Maps;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "CreateTripActivity")]
    public class CreateTripActivity : Activity
    {
        private EditText edtNotes;
        private Button btnDate;
        private Button btnTime;
        private Button btnStartup;
        private Button btnEndup;
        private Button btnAddParticipants;
        private Button btnCreateTrip;
        private TimePickerDialog timePicker;
        private DatePickerDialog datePicker;
        private Trip trip;
        private User user;
        private DateTime date;
        private DateTime time;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_createTrip);
            if (!PlacesApi.IsInitialized)
            {
                PlacesApi.Initialize(this, "AIzaSyAGWOt-4kO9ACMD7ZA2GMqhnXMUTbgs6ho");
            }
            SetViews();

            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;

            // Create your application here
        }
        public void SetViews()
        {
            edtNotes = FindViewById<EditText>(Resource.Id.edtNotesTripCreator);
            btnDate = FindViewById<Button>(Resource.Id.btnDateTripCreator);
            btnTime = FindViewById<Button>(Resource.Id.btnTimeTripCreator);
            btnStartup = FindViewById<Button>(Resource.Id.btnStartupTripCreator);
            btnEndup = FindViewById<Button>(Resource.Id.btnEndUpTripCreator);
            btnAddParticipants = FindViewById<Button>(Resource.Id.btnaddParticipantsTripCreator);
            btnCreateTrip = FindViewById<Button>(Resource.Id.btnCreateTripCreator);

            btnDate.Click += BtnDate_Click;
            btnTime.Click += BtnTime_Click;
            btnStartup.Click += BtnStartup_Click;
            btnEndup.Click += BtnEndup_Click;
            btnAddParticipants.Click += BtnAddParticipants_Click;
            btnCreateTrip.Click += BtnCreateTrip_Click;
        }

        private void BtnCreateTrip_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                DateTime dateTime = new DateTime(date.Date.Year, date.Date.Month, date.Date.Day, time.Hour, time.Minute,0);
                trip = new Trip(btnStartup.Text, btnEndup.Text, dateTime, edtNotes.Text, new TripManager(user.Image, user.Name));
                Toast.MakeText(this, "created successfully", ToastLength.Long);
                Intent intent = new Intent();
                intent.PutExtra("trip", Serializer.ObjectToByteArray(trip));
                SetResult(Result.Ok, intent);
                Finish();
            }
        }

        private bool IsValid()
        {
            return btnDate != null && !btnDate.Text.Equals("") && btnTime != null && !btnTime.Text.Equals("") && btnStartup != null && !btnStartup.Text.Equals("") && btnEndup != null && !btnEndup.Text.Equals("");
        }

        private void BtnAddParticipants_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnEndup_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnStartup_Click(object sender, EventArgs e)
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