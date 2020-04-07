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
    public class MyTrip : BaseEntity
    {
        private int tripID;

        public MyTrip()
        {
            
        }

        public MyTrip(int tripID)
        {
            this.tripID = tripID;
        }

        public int TripID { get => tripID; set => tripID = value; }
    }
}