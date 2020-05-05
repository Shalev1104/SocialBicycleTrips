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
using Helper;
using Dal;

namespace SocialBicycleTrips.Adapters
{
    public class PeopleAdapter : ArrayAdapter<Participant>
    {
        private Context context;
        private Participants participants;
        private int resource;
        private LayoutInflater inflater;

        // ViewHolder הכרזה על אובייקט
        private PeopleHolder peopleHolder;

        private Participant participant;
        private Users users;
        private UsersDB usersDB;


        public PeopleAdapter(Context context, int resource, Participants participants) : base(context, resource, participants)
        {
            this.context = context;
            this.resource = resource;
            this.participants = participants;
            inflater = ((Activity)context).LayoutInflater;

            /*usersDB = new UsersDB();
            users = usersDB.GetAllUsers();*/
            users = new Users();
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);

                peopleHolder = new PeopleHolder();

                peopleHolder.profileImage = convertView.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.profileImageListItem);
                peopleHolder.txtName = convertView.FindViewById<TextView>(Resource.Id.FullNameListItem);

                convertView.Tag = peopleHolder;
            }
            else
            {
                peopleHolder = (PeopleHolder)convertView.Tag;
            }

            participant = GetItem(position);

            if (participant != null)
            {
                try
                {
                    User user = users.GetUserByID(participant.Id);
                    peopleHolder.txtName.Text = user.Name;
                    peopleHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(user.Image));
                }
                catch
                {

                }
            }

            return convertView;
        }

        private class PeopleHolder : Java.Lang.Object
        {
            public Refractored.Controls.CircleImageView profileImage;
            public TextView txtName;
        }
    }
}