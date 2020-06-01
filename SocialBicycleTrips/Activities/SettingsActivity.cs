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
using Dal;
using Helper;
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
        private User user;
        private Trips trips;
        private MyTrips myTrips;
        private LinearLayout notificationLayout;
        private LinearLayout notificationLayoutReminder;
        private ISharedPreferences pref = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);
            SetViews();
            if (Intent.HasExtra("user"))
            {
                user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
                trips = new Trips().GetAllCurrentTrips();
                myTrips = new MyTrips().GetAllMyCurrentTrips(user.Id);
                notificationLayout.Visibility = ViewStates.Visible;
                if (Model.Settings.Notification)
                {
                    notification.Checked = true;
                    notificationLayoutReminder.Visibility = ViewStates.Visible;
                }
            }
            else
            {
                notificationLayout.Visibility = ViewStates.Gone;
            }
            if (Model.Settings.Music)
            {
                music.Checked = true;
            }
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
            notificationLayout = FindViewById<LinearLayout>(Resource.Id.layoutNotificationVisibillity);
            notificationLayoutReminder = FindViewById<LinearLayout>(Resource.Id.layoutNotificationReminder);

            music.Click += Music_Click;
            notification.Click += Notification_Click;

        }
        private void Notification_Click(object sender, EventArgs e)
        {
            Model.Settings.Notification = !Model.Settings.Notification;
            ISharedPreferencesEditor editor = pref.Edit();
            editor.PutBoolean("NotificationSwitcher", notification.Checked);
            editor.Apply();

            if (Model.Settings.Notification)
            {
                notificationLayoutReminder.Visibility = ViewStates.Visible;
                StartAlarm();
                Toast.MakeText(this, "Alarm has been added", ToastLength.Long).Show();
            }
            else
            {
                notificationLayoutReminder.Visibility = ViewStates.Gone;
                CancelAlarm();
                Toast.MakeText(this, "Alarm Canceled", ToastLength.Long).Show();
            }
        }
        public void StartAlarm()
        {
            for (int i = 0; i < myTrips.Count; i++)
            {
                Intent intent = new Intent(this, typeof(Broadcast.ReminderBroadcast)).PutExtra("mytrip", Serializer.ObjectToByteArray(trips.GetTripByID(myTrips[i].TripID)));
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 1, intent, 0);
                AlarmManager alarmManager = (AlarmManager)GetSystemService(AlarmService);
                long totalMilliseconds = (long)(trips.GetTripByID(myTrips[i].TripID).DateTime - DateTime.Now).TotalMilliseconds;
                if (totalMilliseconds > 0)
                {
                    alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + totalMilliseconds - (Model.Settings.TripRemind * 60000), pendingIntent);
                }
            }
        }
        public void CancelAlarm()
        {
            for (int i = 0; i < myTrips.Count; i++)
            {
                Intent intent = new Intent(this, typeof(Broadcast.ReminderBroadcast)).PutExtra("mytrip", Serializer.ObjectToByteArray(trips.GetTripByID(myTrips[i].TripID)));
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 1, intent, 0);
                AlarmManager alarmManager = (AlarmManager)GetSystemService(AlarmService);
                alarmManager.Cancel(pendingIntent);
            }
        }
        private void Music_Click(object sender, EventArgs e)
        {
            ISharedPreferencesEditor editor = pref.Edit();
            editor.PutBoolean("MusicSwitcher", music.Checked);
            editor.Apply();
            Model.Settings.Music = !Model.Settings.Music;
            if (Model.Settings.Music)
            {
                StartService(new Intent(this, typeof(Services.MediaService)));
            }
            else
            {
                StopService(new Intent(this, typeof(Services.MediaService)));
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (Intent.HasExtra("user") && Model.Settings.RememberMe)
            {
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                ISharedPreferencesEditor editor = pref.Edit();
                editor.PutString("user", Android.Util.Base64.EncodeToString(Serializer.ObjectToByteArray(user), Android.Util.Base64.Default));
                editor.PutInt("userId", user.Id);
                editor.PutInt("OngoingTrips", user.UpcomingTrips);
                editor.PutInt("CompletedTrips", user.CompletedTrips);
                editor.Apply();
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
            if(Model.Settings.MapStyle != "Hybrid")
            {
                mapStyles.Add("Hybrid");
            }
            if(Model.Settings.MapStyle != "Terrain")
            {
                mapStyles.Add("Terrain");
            }

            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this,Android.Resource.Layout.SimpleSpinnerItem, mapStyles);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            mapstyle.Adapter = dataAdapter;
            mapstyle.ItemSelected += Mapstyle_ItemSelected;
        }

        private void Mapstyle_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ISharedPreferencesEditor editor = pref.Edit();
            editor.PutString("MapStyle", mapStyles[e.Position]);
            editor.Apply();
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
            else
            {
                tripReminds[0] = Model.Settings.TripRemind.ToString() + " minutes before";
            }

            if (!Model.Settings.TripRemind.ToString().Equals("0"))
            {
                tripReminds.Add("When trip is started");
            }
            if (!Model.Settings.TripRemind.ToString().Equals("5 minutes"))
            {
                tripReminds.Add("5 minutes before");
            }
            if (!Model.Settings.TripRemind.ToString().Equals("15 minutes"))
            {
                tripReminds.Add("15 minutes before");
            }
            if (!Model.Settings.TripRemind.ToString().Equals("30 minutes"))
            {
                tripReminds.Add("30 minutes before");
            }
            if (!Model.Settings.TripRemind.ToString().Equals("60 minutes"))
            {
                tripReminds.Add("60 minutes before");
            }

            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, tripReminds);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            tripRemind.Adapter = dataAdapter;
            tripRemind.ItemSelected += TripRemind_ItemSelected;
        }

        private void TripRemind_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            CancelAlarm();
            ISharedPreferencesEditor editor = pref.Edit();
            if (tripReminds[e.Position].Equals("When trip is started"))
            {
                Model.Settings.TripRemind = 0;
                editor.PutInt("TripRemind", 0);
                editor.Apply();
            }
            else
            {
                char[] ch = { tripReminds[e.Position][0], tripReminds[e.Position][1] };
                string convertToNum = new string(ch);
                Model.Settings.TripRemind = int.Parse(convertToNum);
                editor.PutInt("TripRemind", int.Parse(convertToNum));
                editor.Apply();
            }
            StartAlarm();
        }
    }
}