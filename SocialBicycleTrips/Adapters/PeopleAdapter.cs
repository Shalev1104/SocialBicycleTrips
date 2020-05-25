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
using Bumptech.Glide;

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


        public PeopleAdapter(Context context, int resource, Participants participants,Users users) : base(context, resource, participants)
        {
            this.context = context;
            this.resource = resource;
            this.participants = participants;
            this.users = users;
            inflater = ((Activity)context).LayoutInflater;
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
                User user = users.GetUserByID(participant.UserID);
                peopleHolder.txtName.Text = user.Name;
                if(!user.IsSocialMediaLogon())
                {
                    peopleHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(user.Image));
                }
                else
                {
                    Glide.With(Context).Load(user.Image).Error(Resource.Drawable.StandardProfileImage).Into(peopleHolder.profileImage);
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