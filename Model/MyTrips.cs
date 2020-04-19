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
    [Serializable]
    public class MyTrips : BaseList<MyTrip>
    {
        public override bool Exists(MyTrip myTrip, bool forChange = false)
        {
            bool isTripAlreadyOnMyList;
            if(!forChange)
            {
                isTripAlreadyOnMyList = base.Exists(item => item.TripID.Equals(myTrip.TripID));
            }
            else
            {
                isTripAlreadyOnMyList = base.Exists(item => item.TripID.Equals(myTrip.TripID) && item.Id != myTrip.Id);
            }
            return isTripAlreadyOnMyList;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.TripID.CompareTo(item2.TripID));
        }
    }
}