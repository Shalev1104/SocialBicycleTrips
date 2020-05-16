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
                isFriendExists = base.Exists(item => item.FriendID.Equals(friend.FriendID) && item.UserID.Equals(friend.UserID));
            }
            else
            {
                isFriendExists = base.Exists(item => item.FriendID.Equals(friend.FriendID) && item.UserID.Equals(friend.UserID) && item.Id != friend.Id);
            }
            return isFriendExists;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.FriendID.CompareTo(item2.FriendID));
        }

        public MyFriends GetAllMyFriends(int userID)
        {
            MyFriends myFriends = new MyFriends();
            List<MyFriend> myFriendsList = DbTable<MyFriend>.SelectAll();
            myFriendsList.Remove(new MyFriend(userID, userID));

            for (int i = 0; i < myFriendsList.Count; i++)
            {
                if (!myFriendsList[i].UserID.Equals(userID))
                {
                    myFriendsList.RemoveAt(i);
                }
            }

            if (myFriendsList != null)
            {
                myFriends.AddRange(myFriendsList);
            }

            return myFriends;
        }

        public MyFriends GetAllMyUnFriends(int userID)
        {
            Users users = new Users().GetAllUsers();
            users.Remove(users.GetUserByID(userID));
            MyFriends myFriendsList = new MyFriends().GetAllMyFriends(userID);

            for (int i = 0; i < myFriendsList.Count; i++)
            {
                    users.Remove(users.GetUserByID(myFriendsList[i].FriendID));
            }

            MyFriends myUnFriends = new MyFriends();
            foreach(User unfriend in users)
            {
                myUnFriends.Add(new MyFriend(unfriend.Id, userID));
            }

            return myUnFriends;
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