using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;

namespace SocialBicycleTrips.Broadcast
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { TelephonyManager.ActionPhoneStateChanged })]

    public class PhoneCallReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Extras != null)
            {
                if (intent.Action.Equals(Intent.ActionNewOutgoingCall))
                {
                    Toast.MakeText(context, "Outcoming call", ToastLength.Long).Show();
                }

                string state = intent.GetStringExtra(TelephonyManager.ExtraState);

                if (state == TelephonyManager.ExtraStateRinging)
                {
                    string telephone = intent.GetStringExtra(TelephonyManager.ExtraIncomingNumber);

                    if (string.IsNullOrEmpty(telephone))
                        telephone = string.Empty;

                    Toast.MakeText(context, "Incoming call " + telephone, ToastLength.Long).Show();
                }
                if (state == TelephonyManager.ExtraStateIdle)
                {
                    Toast.MakeText(context, "Call ended", ToastLength.Long).Show();
                }
            }
        }
    }
}