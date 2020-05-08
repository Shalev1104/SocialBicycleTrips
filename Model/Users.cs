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
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace Model
{
    public class Users : BaseList<User>
    {
        public override bool Exists(User user, bool forChange = false)
        {
            bool userExists;

            if(!forChange)
            {
                userExists = base.Exists(item => item.Email.Equals(user.Email));
            }
            else
            {
                userExists = base.Exists(item => item.Email.Equals(user.Email) && item.Id != user.Id);
            }
            return userExists;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.Name.CompareTo(item2.Name));
        }
        public Users GetAllUsers()
        {
            Users users = new Users();
            List<User> usersList = DbTable<User>.SelectAll();

            if (usersList != null)
            {
                users.AddRange(usersList);
            }

            return users;
        }

        public int Insert(User user)
        {
            return DbTable<User>.Insert(user);
        }

        public int Update(User user)
        {
            return DbTable<User>.Update(user);
        }

        public int Delete(User user)
        {
            return DbTable<User>.Delete(user);
        }

        public User GetUserByID(int id)
        {
            User user = null;
            foreach (User found in this)
            {
                if (found.Id.Equals(id))
                {
                    user = found;
                    break;
                }
            }
            return user;
        }
    }
}