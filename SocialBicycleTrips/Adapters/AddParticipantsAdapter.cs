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

namespace SocialBicycleTrips.Adapters
{
    public class AddParticipantsAdapter : ArrayAdapter<Participant>
    {

        private Context context;
        private Participants participants;
        private int resource;
        private LayoutInflater inflater;

        // ViewHolder הכרזה על אובייקט
        private AddParticipantsHolder addParticipantsHolder;

        private Participant participant;

        private Users users;
        private Users selectedUsers;


        public AddParticipantsAdapter(Context context, int resource, Participants participants, Users users) : base(context, resource, participants)
        {
            this.context = context;
            this.resource = resource;
            this.participants = participants;
            this.users = users;
            selectedUsers = new Users();
            inflater = ((Activity)context).LayoutInflater;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);

                addParticipantsHolder = new AddParticipantsHolder();

                addParticipantsHolder.profileImage = convertView.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.profileImageAddParticipant);
                addParticipantsHolder.txtName = convertView.FindViewById<TextView>(Resource.Id.NameAddParticipant);
                addParticipantsHolder.isChecked = convertView.FindViewById<CheckBox>(Resource.Id.chboxisChecked);


                addParticipantsHolder.isChecked.Click += delegate
                {
                    selectedUsers.Add(users.GetUserByID(GetItem(position).UserID));
                };
                convertView.Tag = addParticipantsHolder;
            }
            else
            {
                addParticipantsHolder = (AddParticipantsHolder)convertView.Tag;
            }

            participant = GetItem(position);

            if (participant != null)
            {
                User friendHasntJoinedToTrip = users.GetUserByID(participant.UserID);
                addParticipantsHolder.txtName.Text = friendHasntJoinedToTrip.Name;
                try
                {
                    addParticipantsHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(friendHasntJoinedToTrip.Image));
                }
                catch
                {
                    addParticipantsHolder.profileImage.SetImageBitmap(BitMapHelper.DownloadImageByUrl(friendHasntJoinedToTrip.Image));
                }
            }

            return convertView;
        }

        public Users GetSelectedUsers()
        {
            return selectedUsers;
        }

        private class AddParticipantsHolder : Java.Lang.Object
        {
            public Refractored.Controls.CircleImageView profileImage;
            public TextView txtName;
            public CheckBox isChecked;
        }
    }
}