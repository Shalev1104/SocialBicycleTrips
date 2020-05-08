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
using SQLite;

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

        public MyFriends GetAllMyFriends()
        {
            MyFriends myFriends = new MyFriends();
            List<MyFriend> myFriendsList = DbTable<MyFriend>.SelectAll();

            if (myFriendsList != null)
            {
                myFriends.AddRange(myFriendsList);
            }

            return myFriends;
        }

        public int Insert(MyFriend myFriend)
        {
            return DbTable<MyFriend>.Insert(myFriend);
        }

        public int Update(MyFriend myFriend)
        {
            return DbTable<MyFriend>.Update(myFriend);
        }

        public int Delete(MyFriend myFriend)
        {
            return DbTable<MyFriend>.Delete(myFriend);
        }
    }
}