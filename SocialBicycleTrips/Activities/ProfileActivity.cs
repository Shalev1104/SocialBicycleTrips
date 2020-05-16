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
using Model;
using Dal;
using Helper;
using Bumptech.Glide;
using Square.Picasso;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity
    {
        private ImageButton addFriend;
        private Refractored.Controls.CircleImageView profileImage;
        private TextView name;
        private TextView completedTrips;
        private TextView upcomingTrips;
        private TextView phoneNumber;
        private TextView age;
        private User profile;
        private User userlogon;
        private MyFriends myFriends;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);
            SetViews();
            myFriends = new MyFriends().GetAllMyFriends();
            GenerateUser();
            UploadUserDetails();
            if(userlogon != null)
            {
                if (isFriend())
                {
                    addFriend.Visibility = ViewStates.Invisible;
                }
            }
        }
        public void SetViews()
        {
            name           = FindViewById<TextView> (Resource.Id.txtProfileName);
            addFriend      = FindViewById<ImageButton> (Resource.Id.imgbtnAddFriend);
            profileImage   = FindViewById<Refractored.Controls.CircleImageView> (Resource.Id.imgProfile);
            completedTrips = FindViewById<TextView> (Resource.Id.txtCompletedTourNum);
            upcomingTrips  = FindViewById<TextView> (Resource.Id.txtCloseTourNum);
            phoneNumber    = FindViewById<TextView> (Resource.Id.txtPhoneNumber);
            age            = FindViewById<TextView> (Resource.Id.txtAge);

            addFriend.Visibility = ViewStates.Visible;
            addFriend.Click += AddFriend_Click;
        }

        public void UploadUserDetails()
        {
            name.Text = profile.Name;
            try
            {
                profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(profile.Image));
            }
            catch
            {
                profileImage.SetImageBitmap(BitMapHelper.TransferMediaImages(profile.Image));
                //Picasso.With(this).Load(profile.Image).Into(profileImage);
            }
            completedTrips.Text = profile.CompletedTrips.ToString();
            upcomingTrips.Text = profile.UpcomingTrips.ToString();
            phoneNumber.Text = profile.PhoneNumber;
            try
            {
                age.Text = profile.CalculateAge().ToString();
            }
            catch 
            {
                age.Text = "unknown";
            }
        }

        public void GenerateUser()
        {
            if (Intent.HasExtra("myself"))
            {
                profile = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
                userlogon = profile;
            }
            else if(Intent.HasExtra("user"))
            {
                profile = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("profile")) as User;
                userlogon = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
            }
            else
            {
                profile = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("profile")) as User;
                addFriend.Visibility = ViewStates.Invisible;
            }
        }

        public bool isFriend()
        {
            if (Intent.HasExtra("myself"))
                return true;

            MyFriend isMyFriend = new MyFriend(profile.Id, userlogon.Id);
            //return userlogon.MyFriends.Exists(isMyFriend);
            return IsFriendExists();
        }

        public bool IsFriendExists()
        {
            foreach(MyFriend found in myFriends)
            {
                if (found.UserID.Equals(userlogon.Id) && found.FriendID.Equals(profile.Id))
                {
                    return true;
                }
            }
            return false;
        }

        private void AddFriend_Click(object sender, EventArgs e)
        {
            userlogon.MyFriends.Insert(new MyFriend(profile.Id,userlogon.Id));
            Toast.MakeText(this, "Friend has been added succesfully", ToastLength.Long).Show();
            addFriend.Visibility = ViewStates.Invisible;
        }
    }
}