using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Model
{
    public class Settings
    {
        private static string mapStyle="Standard";
        private static bool notification = false;
        private static bool music = false;
        private static int tripRemind = 0;
        private static bool rememberMe = false;
        private static bool firebaseTempDisconnection = false;
        public static string MapStyle { get => mapStyle; set => mapStyle = value; }
        public static bool Notification { get => notification; set => notification = value; }
        public static bool Music { get => music; set => music = value; }
        public static int TripRemind { get => tripRemind; set => tripRemind = value; }
        public static bool RememberMe { get => rememberMe; set => rememberMe = value; }
        public static bool FirebaseTempDisconnection { get => firebaseTempDisconnection; set => firebaseTempDisconnection = value; }
    }
}