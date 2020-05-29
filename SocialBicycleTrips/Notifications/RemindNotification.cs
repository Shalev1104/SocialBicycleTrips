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

namespace SocialBicycleTrips.Notifications
{
    public class RemindNotification
    {
        public static void ShowNotification(Trip trip,Context context)
        {
            NotificationHelper notificationHelper = new NotificationHelper(context,"channelID","tripReminder");
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, notificationHelper.ChannelID).SetContentTitle("Your trip").SetContentText(trip.Notes + " Is About to start in " + Settings.TripRemind + " minutes").SetPriority(NotificationCompat.PriorityDefault).SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification)).SetTicker("SocialBicycleTrips").SetSmallIcon(Resource.Drawable.ProjectIcon);
            notificationHelper.GetManager().Notify(1, builder.Build());
        }
    }
}