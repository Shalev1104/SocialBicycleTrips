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
using SQLite;

namespace Model
{
    [Serializable]
    [Table("MyTrips")]
    public class MyTrip : BaseEntity
    {
        private int tripID;
        private int userID;

        public MyTrip()
        {
            
        }

        public MyTrip(int tripID, int userID)
        {
            this.tripID = tripID;
            this.userID = userID;
        }

        public int TripID { get => tripID; set => tripID = value; }
        public int UserID { get => userID; set => userID = value; }
    }
}