using Dal;
using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class MyTrips : BaseList<MyTrip>
    {
        public override bool Exists(MyTrip myTrip, bool forChange = false)
        {
            bool isTripAlreadyOnMyList;
            if (!forChange)
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

        public MyTrips GetAllMyTrips() // converts from list to a class(רבים)
        {
            MyTrips myTrips = new MyTrips();
            List<MyTrip> myTripList = DbTable<MyTrip>.SelectAll();

            if (myTripList != null)
            {
                myTrips.AddRange(myTripList);
            }

            return myTrips;
        }

        public int Insert(MyTrip myTrip)
        {
            return DbTable<MyTrip>.Insert(myTrip);
        }

        public int Update(MyTrip myTrip)
        {
            return DbTable<MyTrip>.Update(myTrip);
        }

        public int Delete(MyTrip myTrip)
        {
            return DbTable<MyTrip>.Delete(myTrip);
        }
    }
}