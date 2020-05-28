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
using Helper;
using Model;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "AddFriendsActivity")]
    public class AddFriendsActivity : Activity
    {
        private EditText nonFriendSearchManager;
        private ListView lvAddFriends;
        private Users users;
        private Adapters.MyFriendsAdapter friendsAdapter;
        private User user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_addFriend);
            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
            users = new Users().GetAllUsers();
            SetViews();
            UploadUpdatedList();
            // Create your application here
        }
        public void SetViews()
        {
            nonFriendSearchManager = FindViewById<EditText>(Resource.Id.edtAddFriendsSearchManager);
            lvAddFriends = FindViewById<ListView>(Resource.Id.lvAddFriends);

            nonFriendSearchManager.TextChanged += NonFriendSearchManager_TextChanged;
            lvAddFriends.ItemClick += LvAddFriends_ItemClick;
        }

        private void LvAddFriends_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                Intent intent = new Intent(this, typeof(Activities.ProfileActivity));
                intent.PutExtra("profile", Serializer.ObjectToByteArray(users.GetUserByID(user.MyFriends.GetAllMyUnFriends(user.Id)[e.Position].FriendID)));
                intent.PutExtra("user", Serializer.ObjectToByteArray(user));
                StartActivity(intent);
            }
            catch
            {
                Toast.MakeText(this, "current friend has been added and cannot be clicked", ToastLength.Long).Show();
            }
        }

        private void NonFriendSearchManager_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            List<MyFriend> searchedFriends = (from friend in user.MyFriends.GetAllMyUnFriends(user.Id) where users.GetUserByID(friend.FriendID).Name.Contains(nonFriendSearchManager.Text, StringComparison.OrdinalIgnoreCase) select friend).ToList<MyFriend>();
            MyFriends myUnFriends = new MyFriends();
            if (searchedFriends != null)
            {
                myUnFriends.AddRange(searchedFriends);
            }
            users.Sort();
            friendsAdapter = new Adapters.MyFriendsAdapter(this, Resource.Layout.activity_peopleList, myUnFriends, users);
            lvAddFriends.Adapter = friendsAdapter;
        }

        private void UploadUpdatedList()
        {
            users.Sort();
            friendsAdapter = new Adapters.MyFriendsAdapter(this, Resource.Layout.activity_peopleList, user.MyFriends.GetAllMyUnFriends(user.Id), users);
            lvAddFriends.Adapter = friendsAdapter;
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (Intent.HasExtra("user") && Settings.RememberMe)
            {
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                ISharedPreferencesEditor editor = pref.Edit();
                editor.PutString("user", Android.Util.Base64.EncodeToString(Serializer.ObjectToByteArray(user), Android.Util.Base64.Default));
                editor.PutInt("userId", user.Id);
                editor.PutInt("OngoingTrips", user.UpcomingTrips);
                editor.PutInt("CompletedTrips", user.CompletedTrips);
                editor.Apply();
            }
        }
    }
}