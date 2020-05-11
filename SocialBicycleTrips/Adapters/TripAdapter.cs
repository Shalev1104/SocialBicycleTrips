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

namespace SocialBicycleTrips.Adapters
{
    public class TripAdapter : ArrayAdapter<Trip>
    {
        private Context context;
        private Trips trips;
        private int resource;
        private LayoutInflater inflater;

        // ViewHolder הכרזה על אובייקט
        private TripsHolder tripsHolder;

        private Trip trip;


        public TripAdapter(Context context, int resource, Trips trips) : base(context, resource, trips)
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

                tripsHolder = new TripsHolder();

                tripsHolder.profileImage = convertView.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.imgProfileItem);
                tripsHolder.txtName = convertView.FindViewById<TextView>(Resource.Id.txtNameItem);
                tripsHolder.txtNotes = convertView.FindViewById<TextView>(Resource.Id.txtNotesItem);
                tripsHolder.dayTime = convertView.FindViewById<TextView>(Resource.Id.txtDayTimeItem);
                tripsHolder.date = convertView.FindViewById<TextView>(Resource.Id.txtDateItem);
                tripsHolder.txtStartup = convertView.FindViewById<TextView>(Resource.Id.txtStartupItem);
                tripsHolder.txtEndup = convertView.FindViewById<TextView>(Resource.Id.txtEndupItem);
                tripsHolder.txtParticipants = convertView.FindViewById<TextView>(Resource.Id.txtParticipantsItem);

                convertView.Tag = tripsHolder;
            }
            else
            {
                tripsHolder = (TripsHolder)convertView.Tag;
            }

            trip = GetItem(position);

            if (trip != null)
            {
                tripsHolder.txtName.Text = trip.TripManager.Name;
                tripsHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(trip.TripManager.Image));
                tripsHolder.txtNotes.Text = trip.Notes;
                tripsHolder.dayTime.Text = trip.DateTime.DayOfWeek.ToString() + " " + trip.DateTime.TimeOfDay.ToString();
                tripsHolder.date.Text = trip.DateTime.Date.ToString();
                tripsHolder.txtStartup.Text = trip.StartingLocation.Name;
                tripsHolder.txtEndup.Text = trip.FinalLocation.Name;
                tripsHolder.txtParticipants.Text = trip.Participants.Count().ToString();
            }

            return convertView;
        }

        private class TripsHolder : Java.Lang.Object
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