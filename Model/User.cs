using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Model
{
    public class User : BaseEntity,Inviteable,iChangeable.UserChangeable
    {
        private string        name;
        private string        userName;
        private string        email;
        private string        password;
        private DateTime      dateTime;
        private string        image;
        private MyFriends myFriends;
        private MyTrips myTrips;

        public User()
        {

        }

        public User(string name, string userName, string email, string password, string image)
        {
            this.name     = name;
            this.userName = userName;
            this.email    = email;
            this.password = password;
            this.image    = image;
        }

        public User(string name, string userName, string email, string password)
        {
            this.name     = name;
            this.userName = userName;
            this.email    = email;
            this.password = password;

            //image         = Helper.BitMapHelper.BitMapToBase64(Resource.Drawable.);
        }

        public string Name { get => name; set => name = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Image { get => image; set => image = value; }
        public MyFriends MyFriends { get => myFriends; set => myFriends = value; }
        public MyTrips MyTrips { get => myTrips; set => myTrips = value; }

        public void AddFriend(User friend)
        {
            //myFriends.Add(friend);
        }

        public void InviteFriendToTrip(Trip trip, User user)
        {
            trip.AddParticipant(user);
        }

        public void ChangeName(string name)
        {
            this.name = name;
        }

        public void ChangePassword(string newPassword)
        {
            password = newPassword;
        }

        public void ChangeProfilePicture(string image)
        {
            this.image = image;
        }
    }
}