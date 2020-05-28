using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SocialBicycleTrips.Services
{
    [Service]
    public class MediaService : Service
    {
        private MediaPlayer player;

        private const int Mp3 = Resource.Raw.Music;

        public override IBinder OnBind(Intent intent)
        {
            return null;	// חובה להחזיר 
        }

        // Serviceהמטודה המבצעת את פעילות ה-
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);

            // Thread הפעלת 
            // טעינת הקובץ
            Task.Run(() =>
            {
                Toast.MakeText(this, "Loading..", ToastLength.Long).Show();
                // טעינת הקובץ
                player = MediaPlayer.Create(this, Mp3);

                // הגדרה שהמנגינה תחזור על עצמה
                player.Looping = true;

                // הפעלת הנגן
                player.Start();

            });


            // ראה הסבר בהמשך
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            try
            {
                player.Stop();
            }
            catch
            {
                Toast.MakeText(this, "media error", ToastLength.Short).Show();
            }
        }
    }
}