using Dal;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public MyTrip GetMyTripByID(int id)
        {
            MyTrip myTrip = null;
            foreach (MyTrip found in this)
            {
                if (found.Id.Equals(id))
                {
                    myTrip = found;
                    break;
                }
            }
            return myTrip;
        }

        public MyTrips GetAllMyTrips(int userID) // converts from list to a class(רבים)
        {
            MyTrips myTrips = new MyTrips();
            List<MyTrip> myTripList = DbTable<MyTrip>.SelectAll();
            if(myTripList != null)
            {
                for (int i = 0; i < myTripList.Count; i++)
                {
                    if (!(myTripList[i].UserID == userID))
                    {
                        myTripList.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (myTripList != null)
            {
                myTrips.AddRange(myTripList);
            }

            return myTrips;
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