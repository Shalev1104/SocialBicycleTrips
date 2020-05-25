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
using Android.Provider;
using Android.Graphics;
using Android.Media;
using Java.Lang;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Telephony;

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
        private Dialog dialog;
        private LinearLayout cameraFrame;
        private ImageButton btnCamera;
        private LinearLayout galleryFrame;
        private ImageButton btnGallery;
        private Button btnCancel;
        private Bitmap bitmap;
        private Users users;
        private bool permissionGranted = false;
        readonly string[] permissionPhoneCall = { Manifest.Permission.CallPhone };
        private Model.PhoneCallReceiver callReceiver;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);
            SetViews();
            myFriends = new MyFriends().GetAllMyFriends();
            users = new Users().GetAllUsers();
            callReceiver = new Model.PhoneCallReceiver();
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
            profileImage.LongClick += ProfileImage_LongClick;
            phoneNumber.Click += PhoneNumber_Click;
        }

        protected override void OnResume()
        {
            base.OnResume();

            IntentFilter callIntentFilter = new IntentFilter(TelephonyManager.ActionPhoneStateChanged);

            RegisterReceiver(callReceiver, callIntentFilter);
        }


        protected override void OnPause()
        {
            UnregisterReceiver(callReceiver);
            base.OnPause();
        }

        private void PhoneNumber_Click(object sender, EventArgs e)
        {
            if(!phoneNumber.Text.Equals("not added") && !Intent.HasExtra("myself"))
            {
                MakePhoneCall();
            }
        }

        private bool MakePhoneCall()
        {
            if(ContextCompat.CheckSelfPermission(this,Manifest.Permission.CallPhone) != Permission.Granted)
            {
                permissionGranted = false;
                ActivityCompat.RequestPermissions(this, permissionPhoneCall, 0);
            }
            else
            {
                permissionGranted = true;
                StartActivity(new Intent(Intent.ActionCall, Android.Net.Uri.Parse("tel:" + phoneNumber.Text)));
                SendBroadcast(new Intent(ApplicationContext, typeof(Model.PhoneCallReceiver)));
            }
            return permissionGranted;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if(requestCode == 0)
            {
                if (grantResults.Length == 1)
                {
                    if(grantResults[0] == (int)Permission.Granted)
                    {
                        Toast.MakeText(this, "permission was granted", ToastLength.Long).Show();
                        MakePhoneCall();
                    }
                    else
                    {
                        Toast.MakeText(this, "permission was denied", ToastLength.Long).Show();
                    }
                }
            }
        }
        private void ProfileImage_LongClick(object sender, View.LongClickEventArgs e)
        {
            if (Intent.HasExtra("myself"))
            {
                PerformCustomDialog();
            }
        }
        public void PerformCustomDialog()
        {
            dialog = new Dialog(this);
            dialog.SetContentView(Resource.Layout.activity_cameraOptions);
            dialog.SetCancelable(true);
            dialog.SetTitle("Update Image");

            cameraFrame = dialog.FindViewById<LinearLayout>(Resource.Id.frameCamera);
            galleryFrame = dialog.FindViewById<LinearLayout>(Resource.Id.frameGallery);
            btnCamera = dialog.FindViewById<ImageButton>(Resource.Id.btnCameraProfile);
            btnGallery = dialog.FindViewById<ImageButton>(Resource.Id.btnGalleryProfile);
            btnCancel = dialog.FindViewById<Button>(Resource.Id.btnCancelImage);

            cameraFrame.Click += Camera_Click;
            btnCamera.Click += Camera_Click;

            btnGallery.Click += Gallery_Click;
            galleryFrame.Click += Gallery_Click;

            btnCancel.Click += BtnCancel_Click;
            dialog.Show();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            dialog.Dismiss();
        }

        private void Gallery_Click(object sender, EventArgs e)
        {
            StartActivityForResult(new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri), 2);
        }

        private void Camera_Click(object sender, EventArgs e)
        {
            StartActivityForResult(new Intent(MediaStore.ActionImageCapture), 1);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                if (resultCode == Android.App.Result.Ok)
                {
                    bitmap = (Bitmap)data.Extras.Get("data");
                    profileImage.SetImageBitmap(bitmap);
                    userlogon.Image = BitMapHelper.BitMapToBase64(bitmap);
                    users.Update(userlogon);
                    dialog.Dismiss();
                    Toast.MakeText(this, "Image has been updated", ToastLength.Long).Show();
                }
            }
            else if (requestCode == 2)
            {
                if (resultCode == Android.App.Result.Ok && data != null)
                {
                    bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, data.Data);
                    profileImage.SetImageBitmap(bitmap);
                    userlogon.Image = BitMapHelper.BitMapToBase64(bitmap);
                    users.Update(userlogon);
                    dialog.Dismiss();
                    Toast.MakeText(this, "Image has been updated", ToastLength.Long).Show();
                }
            }
        }

        public void UploadUserDetails()
        {
            name.Text = profile.Name;
            if (!profile.IsSocialMediaLogon()) {
                profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(profile.Image));
            }
            else
            {
                Glide.With(this).Load(profile.Image).Error(Resource.Drawable.StandardProfileImage).Into(profileImage);
            }
            completedTrips.Text = profile.CompletedTrips.ToString();
            upcomingTrips.Text = profile.UpcomingTrips.ToString();
            if(profile.PhoneNumber != null)
            {
                phoneNumber.Text = profile.PhoneNumber;
            }
            else
            {
                phoneNumber.Text = "not added";
            }

            if (profile.DateTime.Date == null)
            {
                age.Text = "not added";
            }
            else
            {
                age.Text = profile.CalculateAge().ToString();
            }

            upcomingTrips.Text = new MyTrips().CountOnGoingTrips(profile.Id).ToString();
            completedTrips.Text = new MyTrips().CountCompletedTrips(profile.Id).ToString();
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