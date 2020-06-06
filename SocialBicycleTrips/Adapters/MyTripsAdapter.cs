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
        private Users users;


        public MyTripsAdapter(Context context, int resource, MyTrips myTrips,Trips trips,Users users) : base(context, resource, myTrips)
        {
            this.context = context;
            this.resource = resource;
            this.myTrips = myTrips;
            this.trips = trips;
            this.users = users;
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
                Trip trip = trips.GetTripByID(myTrip.TripID);
                TripManager manager = Serializer.ByteArrayToObject(trip.TripManager) as TripManager;
                Model.Location startingLocation = Serializer.ByteArrayToObject(trip.StartingLocation) as Model.Location;
                Model.Location destination = Serializer.ByteArrayToObject(trip.FinalLocation) as Model.Location;
                myTripsHolder.txtName.Text = manager.Name;
                if(!users.GetUserByID(manager.Id).IsSocialNetworkLogon())
                {
                    myTripsHolder.profileImage.SetImageBitmap(BitMapHelper.Base64ToBitMap(manager.Image));
                }
                else
                {
                    Glide.With(Context).Load(manager.Image).Error(Resource.Drawable.StandardProfileImage).Into(myTripsHolder.profileImage);
                }
                myTripsHolder.txtNotes.Text = trip.Notes;
                myTripsHolder.dayTime.Text = trip.DateTime.DayOfWeek.ToString() + " , " + trip.DateTime.ToString("h: mm tt");
                myTripsHolder.date.Text = trip.DateTime.ToString("MM/dd/yyyy");
                if (startingLocation.Name != null)
                {
                    myTripsHolder.txtStartup.Text = startingLocation.Name;
                }
                else
                {
                    myTripsHolder.txtStartup.Text = startingLocation.Address;
                }
                if (destination.Name != null)
                {
                    myTripsHolder.txtEndup.Text = destination.Name;
                }
                else
                {
                    myTripsHolder.txtEndup.Text = destination.Address;
                }
                myTripsHolder.txtParticipants.Text = (new Participants().GetAllParticipants(trip.Id).Count() + 1).ToString();
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