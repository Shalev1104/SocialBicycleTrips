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
            Toast.MakeText(context, "received Notification", ToastLength.Long).Show();
            Notifications.RemindNotification.ShowNotification(myTrip, context);
        }
    }
}