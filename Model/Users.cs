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
    public class Users : BaseList<User>
    {
        public override bool Exists(User user, bool forChange = false)
        {
            bool userExists;

            if(!forChange)
            {
                userExists = base.Exists(item => item.UserName.Equals(user.UserName) && item.Email.Equals(user.Email));
            }
            else
            {
                userExists = base.Exists(item => item.UserName.Equals(user.UserName) && item.Email.Equals(user.Email) && item.Id != user.Id);
            }
            return userExists;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.Name.CompareTo(item2.Name));
        }
    }
}