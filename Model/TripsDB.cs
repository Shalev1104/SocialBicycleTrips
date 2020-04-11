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
using Helper;
using Dal;
using SQLite;

namespace Model
{
    public class TripsDB : BaseDB
    {
        private int affectedRows;

        public SQLite.CreateTableResult CreateTable()
        {
            result = connection.CreateTable<Trip>();
            return result;
        }

        public int Insert(Trip trip)
        {
            CreateTable();
            affectedRows = connection.Insert(trip);
            return affectedRows;
        }

        public int Update(Trip trip)
        {
            affectedRows = connection.Update(trip);
            return affectedRows;
        }

        public int Delete(Trip trip)
        {
            affectedRows = connection.Delete(trip);
            return affectedRows;
        }

        public int Delete(int id)
        {
            affectedRows = connection.Delete(id);
            return affectedRows;
        }

        public Trips GetAll(bool sorted = true)
        {
            Trips trips = new Trips();

            try
            {
                List<Trip> tripsList = new List<Trip>();

                if (sorted)
                {
                    string sql = "SELECT * FROM Trips ORDER BY ID";
                    tripsList = connection.Query<Trip>(sql).ToList();
                }
                else
                {
                    tripsList = connection.Table<Trip>().ToList();
                }

                foreach (Trip trip in tripsList)
                    trips.Add(trip);

            }
            catch (SQLite.SQLiteException e)
            {
                return trips;
            }

            return trips;
        }

        public Trip GetTrip(int id)
        {
            return connection.Get<Trip>(id);
        }
    }
}