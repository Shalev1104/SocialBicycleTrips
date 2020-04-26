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

namespace Model
{
    public class UsersDB : DbTable<User>
    {
        public Users GetAllUsers() // converts from list to a class(רבים)
        {
            Users users = new Users();
            List<User> usersList = DbTable<User>.SelectAll();

            if (usersList != null)
            {
                users.AddRange(usersList);
            }

            return users;
        }
    }
}