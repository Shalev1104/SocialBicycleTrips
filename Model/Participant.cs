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
    public class Participant : BaseEntity
    {
        private int userID;
        private int tripID;

        public Participant()
        {

        }

        public Participant(int userID, int tripID)
        {
            this.userID = userID;
            this.tripID = tripID;
        }

        public int UserID { get => userID; set => userID = value; }
        public int TripID { get => tripID; set => tripID = value; }
    }
}