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
    [Table("myFriend")]
    public class MyFriend : BaseEntity
    {
        int friendID;
        int userID;

        public MyFriend()
        {

        }
        public MyFriend(int friendID, int userID)
        {
            this.friendID = friendID;
            this.userID = userID;
        }

        public bool IsTripParticipant(int tripID)
        {
            MyTrips myTrips = new MyTrips().GetAllMyTrips(userID);
            for (int i = 0; i < myTrips.Count; i++)
            {
                if (myTrips[i].TripID.Equals(tripID))
                {
                    return true;
                }
            }
            return false;
        }

        public int FriendID { get => friendID; set => friendID = value; }
        public int UserID { get => userID; set => userID = value; }
    }
}