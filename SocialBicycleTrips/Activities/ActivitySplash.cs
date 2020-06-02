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
        private ISharedPreferences settings;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            settings = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);

            if (settings.Contains("NotificationSwitcher"))
            {
                Settings.Notification = settings.GetBoolean("NotificationSwitcher", false);
            }
            if (settings.Contains("MusicSwitcher"))
            {
                Settings.Music = settings.GetBoolean("MusicSwitcher", false);
                if (Settings.Music)
                {
                    StartService(new Intent(this, typeof(Services.MediaService)));
                }
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
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}