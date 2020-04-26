using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Helper;
using Dal;
using SQLite;

namespace Model
{
    public class TripsDB : DbTable<Trip>
    {
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
    }
}