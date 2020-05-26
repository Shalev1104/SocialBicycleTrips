using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Helper;
using Model;

namespace SocialBicycleTrips.Broadcast
{
    [BroadcastReceiver]
    public class ReminderBroadcast : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Trip myTrip = Serializer.ByteArrayToObject(intent.GetByteArrayExtra("mytrip")) as Trip;
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, "notifyLemubit").SetContentTitle("Your trip").SetContentText(myTrip.Notes + " Is About to start in " + Settings.TripRemind + " minutes").SetPriority(NotificationCompat.PriorityDefault).SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification)).SetTicker("SocialBicycleTrips").SetSmallIcon(Resource.Drawable.ProjectIcon);
            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(200, builder.Build());
        }
    }
}