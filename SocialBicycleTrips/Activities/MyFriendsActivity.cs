using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Helper;
using Model;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "MyFriendsActivity")]
    public class MyFriendsActivity : Activity
    {
        private ListView lvMyFriends;
        private EditText searchManager;
        private Users users;
        private Adapters.MyFriendsAdapter friendsAdapter;
        private User user;
        private FloatingActionButton btnAddFriend;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_myFriends);
            SetViews();
            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
            users = new Users().GetAllUsers();
            UploadUpdatedList();
            // Create your application here
        }
        public void SetViews()
        {
            lvMyFriends = FindViewById<ListView>(Resource.Id.lvMyFriends);
            btnAddFriend = FindViewById<FloatingActionButton>(Resource.Id.btnAddAFriend);
            searchManager = FindViewById<EditText>(Resource.Id.edtMyFriendsSearchManager);

            btnAddFriend.Click += BtnAddFriend_Click;
            searchManager.TextChanged += SearchManager_TextChanged;
            lvMyFriends.ItemClick += LvMyFriends_ItemClick;
            lvMyFriends.ItemLongClick += LvMyFriends_ItemLongClick;
        }

        private void LvMyFriends_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Android.Support.V7.App.AlertDialog.Builder alertDiag = new Android.Support.V7.App.AlertDialog.Builder(this);

            alertDiag.SetTitle("Confirm delete");
            alertDiag.SetMessage("Are you sure you want to delete your friend?");

            alertDiag.SetCancelable(true);

            alertDiag.SetPositiveButton("Delete", (senderAlert, args)
                   => {
                       MyFriend myFriend = user.MyFriends.GetAllMyFriends(user.Id)[e.Position];

                       user.MyFriends.Delete(myFriend);
                       Toast.MakeText(this, "Deleted", ToastLength.Long).Show();
                       UploadUpdatedList();

                       alertDiag.Dispose();
                   });

            alertDiag.SetNegativeButton("Cancel", (senderAlert, args)
             => {
                 alertDiag.Dispose();
             });

            Dialog diag = alertDiag.Create();
            diag.Show();

        }
        private void LvMyFriends_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activities.ProfileActivity));
            intent.PutExtra("profile", Serializer.ObjectToByteArray(users.GetUserByID(user.MyFriends.GetAllMyFriends(user.Id)[e.Position].FriendID)));
            intent.PutExtra("user", Serializer.ObjectToByteArray(user));
            StartActivity(intent);
        }

        private void SearchManager_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            List<MyFriend> searchedFriends = (from friend in user.MyFriends.GetAllMyFriends(user.Id) where users.GetUserByID(friend.FriendID).Name.Contains(searchManager.Text, StringComparison.OrdinalIgnoreCase) select friend).ToList<MyFriend>();
            MyFriends myFriends = new MyFriends();
            if(searchedFriends != null)
            {
                myFriends.AddRange(searchedFriends);
            }
            users.Sort();
            friendsAdapter = new Adapters.MyFriendsAdapter(this, Resource.Layout.activity_peopleList, myFriends, users);
            lvMyFriends.Adapter = friendsAdapter;
        }
        private void UploadUpdatedList()
        {
            users.Sort();
            friendsAdapter = new Adapters.MyFriendsAdapter(this, Resource.Layout.activity_peopleList,user.MyFriends.GetAllMyFriends(user.Id),users);
            lvMyFriends.Adapter = friendsAdapter;
        }

        private void BtnAddFriend_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activities.AddFriendsActivity));
            intent.PutExtra("user", Serializer.ObjectToByteArray(user));
            StartActivity(intent);
        }
    }
}