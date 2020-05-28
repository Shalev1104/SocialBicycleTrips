using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Helper;
using Model;

namespace SocialBicycleTrips.Activities
{

    [Activity(Theme ="@style/Theme.Splash", MainLauncher = true, NoHistory = true, Icon = "@drawable/ProjectIcon")]
    public class ActivitySplash : Activity
    {
        private User toNavigate;
        private ISharedPreferences pref;
        private ISharedPreferences settings;
        private byte[] userArray;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            settings = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);

            if (pref.Contains("user"))
            {
                userArray = Android.Util.Base64.Decode(pref.GetString("user", null), Android.Util.Base64.Default);
            }
            if(userArray != null)
            {
                toNavigate = Serializer.ByteArrayToObject(userArray) as User;
                toNavigate.Id = pref.GetInt("userId", 0);
                toNavigate.UpcomingTrips = pref.GetInt("OngoingTrips", 0);
                toNavigate.CompletedTrips = pref.GetInt("CompletedTrips", 0);
            }

            if (settings.Contains("RememberMe"))
            {
                Settings.RememberMe = settings.GetBoolean("RememberMe", false);
            }

            if (settings.Contains("NotificationSwitcher"))
            {
                Settings.Notification = settings.GetBoolean("NotificationSwitcher", false);
            }
            if (settings.Contains("MusicSwitcher"))
            {
                Settings.Music = settings.GetBoolean("MusicSwitcher", false);
            }
            if (settings.Contains("MapStyle"))
            {
                Settings.MapStyle = settings.GetString("MapStyle", "Standard");
            }
            if (settings.Contains("TripRemind"))
            {
                Settings.TripRemind = settings.GetInt("TripRemind", 0);
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }
        async void SimulateStartup()
        {
            await Task.Delay(1000);
            if (toNavigate != null)
            {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)).PutExtra("user",Serializer.ObjectToByteArray(toNavigate)));
            }
            else
            {
                ISharedPreferencesEditor editor = pref.Edit();
                editor.Clear();
                editor.Apply();
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
        }
    }
}