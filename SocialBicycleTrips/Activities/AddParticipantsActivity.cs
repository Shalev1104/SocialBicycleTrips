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
                for (int i = 0; i < selected.Count; i++)
                {
                    trip.Participants.Insert(new Participant(selected[i].Id, trip.Id));
                }
                SetResult(Android.App.Result.Ok);
            }
            Finish();
        }

        public void UploadUpdatedList()
        {
            addParticipantsAdapter = new Adapters.AddParticipantsAdapter(this, Resource.Layout.activity_addParticipants,trip.Participants.GetAllMyFriendsParticipants(user.Id,trip.Id), users);
            lvAddParticipants.Adapter = addParticipantsAdapter;
        }
    }
}