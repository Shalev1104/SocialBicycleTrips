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
using Dal;

namespace Model
{
    [Serializable]
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

        public Trips GetAllTrips() // converts from list to a class(רבים)
        {
            Trips trips = new Trips();
            List<Trip> tripsList = DbTable<Trip>.SelectAll();

            if (tripsList != null)
            {
                trips.AddRange(tripsList);
            }

            return trips;
        }

        public int Insert(Trip trip)
        {
            return DbTable<Trip>.Insert(trip);
        }

        public int Update(Trip trip)
        {
            return DbTable<Trip>.Update(trip);
        }

        public int Delete(Trip trip)
        {
            return DbTable<Trip>.Delete(trip);
        }

        public Trip GetTripByID(int id)
        {
            Trip trip = null;
            foreach (Trip found in this)
            {
                if (found.Id.Equals(id))
                {
                    trip = found;
                    break;
                }
            }
            return trip;
        }
    }
}