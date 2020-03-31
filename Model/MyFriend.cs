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
    public class MyFriend : BaseEntity
    {
        int userID;
        int friendID;
        bool isFriends;

        public MyFriend()
        {

        }
        public MyFriend(int userID, int friendID, bool isFriends)
        {
            this.userID = userID;
            this.friendID = friendID;
            this.isFriends = isFriends;
        }

        public int UserID { get => userID; set => userID = value; }
        public int FriendID { get => friendID; set => friendID = value; }
        public bool IsFriends { get => isFriends; set => isFriends = value; }
    }
}