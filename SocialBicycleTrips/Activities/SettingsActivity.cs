using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using Model;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        private Spinner mapstyle;
        private List<string> mapStyles;
        private Switch music;
        private Switch notification;
        private Spinner tripRemind;
        private List<string> tripReminds;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);
            SetViews();
            GenerateMapStyles();
            GenerateTripReminds();
            // Create your application here
        }
        public void SetViews()
        {
            mapstyle = FindViewById<Spinner>(Resource.Id.spnMapStyle);
            music = FindViewById<Switch>(Resource.Id.switchMusic);
            notification = FindViewById<Switch>(Resource.Id.switchNotification);
            tripRemind = FindViewById<Spinner>(Resource.Id.spnTripReminder);

            music.Click += Music_Click;
            notification.Click += Notification_Click;

        }

        private void Notification_Click(object sender, EventArgs e)
        {
            Model.Settings.Notification = !Model.Settings.Notification;
        }

        private void Music_Click(object sender, EventArgs e)
        {
            Model.Settings.Music = !Model.Settings.Music;
            if (Model.Settings.Music)
            {
                StartService(new Intent(this, typeof(Model.MediaService)));
            }
            else
            {
                StopService(new Intent(this, typeof(Model.MediaService)));
            }
        }

        public void GenerateMapStyles()
        {
            mapStyles = new List<string>();
            mapStyles.Add(Model.Settings.MapStyle);

            if (Model.Settings.MapStyle != "Standard")
            {
                mapStyles.Add("Standard");
            }
            if (Model.Settings.MapStyle != "Silver")
            {
                mapStyles.Add("Silver");
            }
            if (Model.Settings.MapStyle != "Retro")
            {
                mapStyles.Add("Retro");
            }
            if (Model.Settings.MapStyle != "Dark")
            {
                mapStyles.Add("Dark");
            }
            if (Model.Settings.MapStyle != "Night")
            {
                mapStyles.Add("Night");
            }
            if (Model.Settings.MapStyle != "Aubergine")
            {
                mapStyles.Add("Aubergine");
            }

            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this,Android.Resource.Layout.SimpleSpinnerItem, mapStyles);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            mapstyle.Adapter = dataAdapter;
            mapstyle.ItemSelected += Mapstyle_ItemSelected;
        }

        private void Mapstyle_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Model.Settings.MapStyle = mapStyles[e.Position];
        }

        public void GenerateTripReminds()
        {
            tripReminds = new List<string>();
            tripReminds.Add(Model.Settings.TripRemind.ToString());
            if (Model.Settings.TripRemind.ToString().Equals("0"))
            {
                tripReminds[0] = "When trip is started";
            }

            if (Model.Settings.TripRemind.ToString() != "When trip is started")
            {
                tripReminds.Add("When trip is started");
            }
            if (Model.Settings.TripRemind.ToString() != "5 minutes")
            {
                tripReminds.Add("5 minutes");
            }
            if (Model.Settings.TripRemind.ToString() != "15 minutes")
            {
                tripReminds.Add("15 minutes");
            }
            if (Model.Settings.TripRemind.ToString() != "30 minutes")
            {
                tripReminds.Add("30 minutes");
            }
            if (Model.Settings.TripRemind.ToString() != "60 minutes")
            {
                tripReminds.Add("60 minutes");
            }

            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, tripReminds);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            tripRemind.Adapter = dataAdapter;
            tripRemind.ItemSelected += TripRemind_ItemSelected;
        }

        private void TripRemind_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (tripReminds[e.Position].Equals("When trip is started"))
            {
                Model.Settings.TripRemind = 0;
            }
            else
            {
                char[] ch = { tripReminds[e.Position][0], tripReminds[e.Position][1] };
                string convertToNum = new string(ch);
                Model.Settings.TripRemind = int.Parse(convertToNum);
            }
        }
    }
}