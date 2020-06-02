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
        IBinder binder;
        MediaPlayer player;
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            player = MediaPlayer.Create(this, Resource.Raw.Music);
            player.Looping = true;
            player.Start();

            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new MediaServiceBinder(this);
            return binder;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (player != null)
            {
                player.Stop();
                player.Release();
                player = null;
            }

        }
    }


    public class MediaServiceBinder : Binder
    {
        readonly MediaService service;

        public MediaServiceBinder(MediaService service)
        {
            this.service = service;
        }

        public MediaService GetFirstService()
        {
            return service;
        }
    }
}