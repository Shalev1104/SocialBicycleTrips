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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);
            SetViews();
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

            GenerateUser();
            UploadUserDetails();
            if(isFriend())
            {
                addFriend.Visibility = ViewStates.Invisible;
            }
        }

        public void UploadUserDetails()
        {
            name.Text = profile.Name;
            profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(profile.Image));
            completedTrips.Text = profile.CompletedTrips.ToString();
            upcomingTrips.Text = profile.UpcomingTrips.ToString();
            phoneNumber.Text = profile.PhoneNumber;
            age.Text = profile.CalculateAge().ToString();
        }

        public void GenerateUser()
        {
            profile = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("profile")) as User;
            userlogon = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
        }

        public bool isFriend()
        {
            MyFriend isMyFriend = new MyFriend(profile.Id);
            if(userlogon.MyFriends.Exists(isMyFriend) || profile.Equals(userlogon))
                return true;
            return false;
        }

        private void AddFriend_Click(object sender, EventArgs e)
        {
            userlogon.MyFriends.Add(new MyFriend(profile.Id));
            Toast.MakeText(this, "Friend has been added succesfully", ToastLength.Long).Show();
        }
    }
}