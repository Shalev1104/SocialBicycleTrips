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
using Model;
using Dal;
using Helper;
using System.Security.Cryptography;
using Bumptech.Glide;

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
        TripManager manager;
        Model.Location startingLocation;
        Model.Location destination;
        Users users;

        public TripAdapter(Context context, int resource, Trips trips,Users users) : base(context, resource, trips)
        {
            this.context = context;
            this.resource = resource;
            this.trips = trips;
            this.users = users;
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
                manager = Serializer.ByteArrayToObject(trip.TripManager) as TripManager;
                startingLocation = Serializer.ByteArrayToObject(trip.StartingLocation) as Model.Location;
                destination = Serializer.ByteArrayToObject(trip.FinalLocation) as Model.Location;
                tripsHolder.txtName.Text = manager.Name;
                if(!users.GetUserByID(manager.Id).IsSocialMediaLogon())
                {
                    tripsHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(manager.Image));
                }
                else
                {
                    Glide.With(Context).Load(manager.Image).Error(Resource.Drawable.StandardProfileImage).Into(tripsHolder.profileImage);
                }
                tripsHolder.txtNotes.Text = trip.Notes;
                tripsHolder.dayTime.Text = trip.DateTime.DayOfWeek.ToString() + " , " + trip.DateTime.ToString("h: mm tt");
                tripsHolder.date.Text = trip.DateTime.ToString("MM/dd/yyyy");
                if (startingLocation.Name != null)
                {
                    tripsHolder.txtStartup.Text = startingLocation.Name;
                }
                else
                {
                    tripsHolder.txtStartup.Text = startingLocation.Address;
                }
                if (destination.Name != null)
                {
                    tripsHolder.txtEndup.Text = destination.Name;
                }
                else
                {
                    tripsHolder.txtEndup.Text = destination.Address;
                }
                tripsHolder.txtParticipants.Text = (new Participants().GetAllParticipants(trip.Id).Count() + 1).ToString();
                
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