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
    public class iChangeable
    {
        public interface TripChangeable
        {
            public void UpdateLocation(string startup,string endup);
            public void ChangeDateAndTime(DateTime dateTime, TimeSpan time);
            public void UpdateDetails(string updatedDetails);
        }

        public interface UserChangeable
        {
            public void ChangeName(string name);
            public void ChangePassword(string newPassword);
            public void ChangeProfilePicture(string image);
        }
    }
}