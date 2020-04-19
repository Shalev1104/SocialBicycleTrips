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
    public class MyFriend : BaseEntity
    {
        int friendID;

        public MyFriend()
        {

        }
        public MyFriend(int friendID)
        {
            this.friendID = friendID;
        }

        public int FriendID { get => friendID; set => friendID = value; }
    }
}