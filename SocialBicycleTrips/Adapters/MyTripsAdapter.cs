﻿using System;
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
    public class MyTripsAdapter : ArrayAdapter<MyTrip>
    {
        private Context context;
        private MyTrips myTrips;
        private int resource;
        private LayoutInflater inflater;

        // ViewHolder הכרזה על אובייקט
        private MyTripsHolder myTripsHolder;

        private MyTrip myTrip;

        private Trips trips;


        public MyTripsAdapter(Context context, int resource, MyTrips myTrips,Trips trips) : base(context, resource, myTrips)
        {
            this.context = context;
            this.resource = resource;
            this.myTrips = myTrips;
            this.trips = trips;
            inflater = ((Activity)context).LayoutInflater;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);

                myTripsHolder = new MyTripsHolder();

                myTripsHolder.profileImage = convertView.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.imgProfileItem);
                myTripsHolder.txtName = convertView.FindViewById<TextView>(Resource.Id.txtNameItem);
                myTripsHolder.txtNotes = convertView.FindViewById<TextView>(Resource.Id.txtNotesItem);
                myTripsHolder.dayTime = convertView.FindViewById<TextView>(Resource.Id.txtDayTimeItem);
                myTripsHolder.date = convertView.FindViewById<TextView>(Resource.Id.txtDateItem);
                myTripsHolder.txtStartup = convertView.FindViewById<TextView>(Resource.Id.txtStartupItem);
                myTripsHolder.txtEndup = convertView.FindViewById<TextView>(Resource.Id.txtEndupItem);
                myTripsHolder.txtParticipants = convertView.FindViewById<TextView>(Resource.Id.txtParticipantsItem);

                convertView.Tag = myTripsHolder;
            }
            else
            {
                myTripsHolder = (MyTripsHolder)convertView.Tag;
            }

            myTrip = GetItem(position);

            if (myTrip != null)
            {
                Trip trip = trips.GetTripByID(myTrip.Id);
                myTripsHolder.txtName.Text = trip.TripManager.Name;
                myTripsHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(trip.TripManager.Image));
                myTripsHolder.txtNotes.Text = trip.Notes;
                myTripsHolder.dayTime.Text = trip.DateTime.DayOfWeek.ToString() + " " + trip.DateTime.TimeOfDay.ToString();
                myTripsHolder.date.Text = trip.DateTime.Date.ToString();
                myTripsHolder.txtStartup.Text = trip.StartingLocation.Name;
                myTripsHolder.txtEndup.Text = trip.FinalLocation.Name;
                myTripsHolder.txtParticipants.Text = trip.Participants.Count().ToString();
            }

            return convertView;
        }

        private class MyTripsHolder : Java.Lang.Object
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