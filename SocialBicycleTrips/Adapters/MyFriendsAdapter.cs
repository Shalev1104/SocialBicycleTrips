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
using Bumptech.Glide;
using Helper;
using Model;

namespace SocialBicycleTrips.Adapters
{
    public class MyFriendsAdapter : ArrayAdapter<MyFriend>
    {
        private Context context;
        private MyFriends myFriends;
        private int resource;
        private LayoutInflater inflater;

        // ViewHolder הכרזה על אובייקט
        private MyFriendsHolder myFriendsHolder;

        private MyFriend myFriend;

        private Users users;


        public MyFriendsAdapter(Context context, int resource, MyFriends myFriends, Users users) : base(context, resource, myFriends)
        {
            this.context = context;
            this.resource = resource;
            this.myFriends = myFriends;
            this.users = users;
            inflater = ((Activity)context).LayoutInflater;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);

                myFriendsHolder = new MyFriendsHolder();

                myFriendsHolder.profileImage = convertView.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.profileImageListItem);
                myFriendsHolder.txtName = convertView.FindViewById<TextView>(Resource.Id.FullNameListItem);

                convertView.Tag = myFriendsHolder;
            }
            else
            {
                myFriendsHolder = (MyFriendsHolder)convertView.Tag;
            }

            myFriend = GetItem(position);

            if (myFriend != null)
            {
                User friend = users.GetUserByID(myFriend.FriendID);
                myFriendsHolder.txtName.Text = friend.Name;
                if(!friend.IsSocialMediaLogon())
                {
                    myFriendsHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(friend.Image));
                }
                else
                {
                    Glide.With(Context).Load(friend.Image).Error(Resource.Drawable.StandardProfileImage).Into(myFriendsHolder.profileImage);
                }
            }

            return convertView;
        }

        private class MyFriendsHolder : Java.Lang.Object
        {
            public Refractored.Controls.CircleImageView profileImage;
            public TextView txtName;
        }
    }
}