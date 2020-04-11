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

namespace Model
{
    public class TripsAdapter : ArrayAdapter<Trip>
    {
        private Context context;
        private Trips trips;
        private int resource;
        private LayoutInflater inflater;

        // ViewHolder הכרזה על אובייקט
        private ViewHolder viewHolder;

        private Trip trip;


        public TripsAdapter(Context context, int resource, Trips trips) : base(context, resource, trips)
        {
            this.context = context;
            this.resource = resource;
            this.trips = trips;
            inflater = ((Activity)context).LayoutInflater;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);

                viewHolder = new ViewHolder();

                viewHolder.profileImage = convertView.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.imgProfile);
                viewHolder.txtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                viewHolder.txtNotes = convertView.FindViewById<TextView>(Resource.Id.txtNotes);
                viewHolder.dayTime = convertView.FindViewById<ImageView>(Resource.Id.txtDayTime);
                viewHolder.date = convertView.FindViewById<ImageView>(Resource.Id.txtDate);
                viewHolder.txtStartup = convertView.FindViewById<ImageView>(Resource.Id.txtStartup);
                viewHolder.txtEndup = convertView.FindViewById<ImageView>(Resource.Id.txtEndup);
                viewHolder.txtParticipants = convertView.FindViewById<ImageView>(Resource.Id.txtParticipants);

                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)convertView.Tag;
            }

            trip = GetItem(position);

            if (trip != null)
            {
                viewHolder.txtName.Text = trip.TripManager.Name;
                viewHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(trip.TripManager.Image));
                viewHolder.txtNotes.Text = trip.Notes;
                viewHolder.dayTime.Text = trip.DateTime.DayOfWeek.ToString() + " " + trip.DateTime.TimeOfDay.ToString();
                viewHolder.date.Text = trip.DateTime.Date.ToString();
                viewHolder.txtStartup.Text = trip.StartingLocation;
                viewHolder.txtEndup.Text = trip.FinalLocation;
                viewHolder.txtParticipants.Text = trip.Participants.Capacity.ToString();
            }

            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public Refractored.Controls.CircleImageView profileImage;
            public TextView txtName;
            public TextView txtNotes;
            public TextView dayTime;
            public TextView date;
            public TextView txtStartup;
            public TextView txtEndup;
            public TextView txtParticipants;
        }
    }
}