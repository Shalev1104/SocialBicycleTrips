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
    public class MyFriends : BaseList<MyFriend>
    {
        public override bool Exists(MyFriend friend, bool forChange = false)
        {
            bool isFriendExists;

            if (!forChange)
            {
                isFriendExists = base.Exists(item => item.FriendID.Equals(friend.FriendID));
            }
            else
            {
                isFriendExists = base.Exists(item => item.FriendID.Equals(friend.FriendID) && item.Id != friend.Id);
            }
            return isFriendExists;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.FriendID.CompareTo(item2.FriendID));
        }
    }
}