using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace Helper
{
    public class NotificationHelper : ContextWrapper
    {
        private string channelID = "channelID";
        private string channelName = "Channel Name";
        private NotificationManager mManager;

        public string ChannelID { get => channelID; set => channelID = value; }
        public string ChannelName { get => channelName; set => channelName = value; }

        public NotificationHelper(Context context,string channelID,string channelName) : base(context)
        {
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                CreateChannel();
                this.channelID = channelID;
                this.channelName = channelName;
            }
        }
        private void CreateChannel()
        {
            NotificationChannel channel = new NotificationChannel(channelID, channelName, NotificationManager.ImportanceHigh);
            GetManager().CreateNotificationChannel(channel);
        }
        public NotificationManager GetManager()
        {
            if (mManager == null)
            {
                mManager = (NotificationManager)GetSystemService(Context.NotificationService);
            }
            return mManager;
        }
    }
}