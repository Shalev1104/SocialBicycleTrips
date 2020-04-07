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
    public class Trips : BaseList<Trip>
    {
        public override bool Exists(Trip trip, bool forChange = false)
        {
            bool tripExists;
            if (!forChange)
            {
                tripExists = base.Exists(item => item.DateTime.Equals(trip.DateTime) && item.StartingLocation.Equals(trip.StartingLocation) && item.FinalLocation.Equals(trip.FinalLocation));
            }
            else
            {
                tripExists = base.Exists(item => item.DateTime.Equals(trip.DateTime) && item.StartingLocation.Equals(trip.StartingLocation) && item.FinalLocation.Equals(trip.FinalLocation) && item.Id != trip.Id);
            }
            return tripExists;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.DateTime.CompareTo(item2.DateTime));
        }
    }
}