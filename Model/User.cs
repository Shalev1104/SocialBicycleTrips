﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Helper;
using SQLite;
using Dal;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions;

namespace Model
{
    [Serializable]
    [Table("Users")]
    public class User : TripManager, iChangeable.UserChangeable
    {
        private string email;
        private string password;
        private DateTime birthday;
        private string phoneNumber;
        private int completedTrips;
        private int upcomingTrips;
        private MyFriends myFriends { get; set; }
        private MyTrips myTrips { get; set; }

        public User()
        {
            myFriends = new MyFriends();
            myTrips = new MyTrips();
        }

        public User(string name, string email, string password, string image, DateTime birthday, string phoneNumber) : base(image,name)
        {
            this.email    = email;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.birthday = birthday;

            completedTrips = 0;
            upcomingTrips = 0;
            myFriends = new MyFriends();
            myTrips = new MyTrips();
        }

        public User(string name, string email, string password, DateTime birthday, string phoneNumber) : base("",name)
        {
            this.email    = email;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.birthday = birthday;

            //image = BitMapHelper.BitMapToBase64(((BitmapDrawable) Resource.Drawable.common_google_signin_btn_icon_dark).Bitmap);

            completedTrips = 0;
            upcomingTrips = 0;
            myFriends = new MyFriends();
            myTrips = new MyTrips();
        }
        public User(string name, string email,  string image, string phoneNumber) : base(image,name) //Social Media Login
        {
            this.email = email;
            this.phoneNumber = phoneNumber;

            completedTrips = 0;
            upcomingTrips = 0;
            myFriends = new MyFriends();
            myTrips = new MyTrips();
        }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        [Ignore]
        public MyFriends MyFriends { get => myFriends; set => myFriends = value; }
        [Ignore]
        public MyTrips MyTrips { get => myTrips; set => myTrips = value; }
        public int CompletedTrips { get => completedTrips; set => completedTrips = value; }
        public int UpcomingTrips { get => upcomingTrips; set => upcomingTrips = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public DateTime DateTime { get => birthday; set => birthday = value; }

        public int CalculateAge()
        {
            int age = DateTime.Now.Year - birthday.Year;

            if ((birthday.Month > DateTime.Now.Month) || (birthday.Month == DateTime.Now.Month && birthday.Day > DateTime.Now.Day))
                age--;

            return age;
        }

        public void AddFriend()
        {
            //myFriends.Add(friend);
        }
        
        public bool IsSocialMediaLogon()
        {
            return password == null;
        }
        public void InviteFriendToTrip(MyFriend myFriend)
        {
            //
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