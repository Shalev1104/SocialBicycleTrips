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
    [Activity(Label = "AddParticipantsActivity")]
    public class AddParticipantsActivity : Activity
    {
        private ListView lvAddParticipants;
        private Button btnAddSelectedUsers;
        private Adapters.AddParticipantsAdapter addParticipantsAdapter;
        private Users users;
        private User user;
        private Trip trip;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_selectParticipants);
            users = new Users().GetAllUsers();
            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
            trip = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("trip")) as Trip;
            SetViews();
            UploadUpdatedList();
            // Create your application here
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
        public void SetViews()
        {
            btnAddSelectedUsers = FindViewById<Button>(Resource.Id.btnAddSelectedFriendToTrip);
            lvAddParticipants = FindViewById<ListView>(Resource.Id.lvAddParticipants);

            btnAddSelectedUsers.Click += BtnAddSelectedUsers_Click;
        }

        private void BtnAddSelectedUsers_Click(object sender, EventArgs e)
        {
            Users selected = addParticipantsAdapter.GetSelectedUsers();
            if(selected != null)
            {
                Intent intent = new Intent().PutExtra("selected", Serializer.ObjectToByteArray(selected));
                SetResult(Android.App.Result.Ok, intent);
            }
            Finish();
        }

        public void UploadUpdatedList()
        {
            addParticipantsAdapter = new Adapters.AddParticipantsAdapter(this, Resource.Layout.activity_addParticipants,new Participants().GetAllMyFriendsParticipants(user.Id,trip.Id), users);
            lvAddParticipants.Adapter = addParticipantsAdapter;
        }
    }
}