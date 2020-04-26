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
    public class Participants : BaseList<Participant>
    {
        public override bool Exists(Participant participant, bool forChange = false)
        {
            bool isAlreadyJoinedToTrip;
            if(!forChange)
            {
                isAlreadyJoinedToTrip = base.Exists(item => item.UserID.Equals(participant.UserID) && item.TripID.Equals(participant.TripID));
            }
            else
            {
                isAlreadyJoinedToTrip = base.Exists(item => item.UserID.Equals(participant.UserID) && item.TripID.Equals(participant.TripID) && item.Id != participant.Id);
            }
            return isAlreadyJoinedToTrip;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.UserID.CompareTo(item2.UserID));
        }
    }
}