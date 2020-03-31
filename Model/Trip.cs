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

namespace Model
{
    public class Trip : iChangeable.TripChangeable
    {
        private string     startingLocation;
        private string     finalLocation;
        private DateTime   dateTime;
        private string     about;
        private Participants participants;

        public Trip(string startingLocation, string finalLocation, DateTime date, string about, User tripCreator)
        {
            this.startingLocation = startingLocation;
            this.finalLocation    = finalLocation;
            this.dateTime         = date;
            this.about            = about;

        }

        public string StartingLocation { get => startingLocation; set => startingLocation = value; }
        public string FinalLocation { get => finalLocation; set => finalLocation = value; }
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public string About { get => about; set => about = value; }
        public Participants Participants { get => participants; set => participants = value; }

        public void AddParticipant(User user)
        {
            //participants.Add(user);
        }

        public void ChangeDateAndTime(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public void UpdateDetails(string updatedDetails)
        {
            about = updatedDetails;
        }

        public void UpdateLocation(string startup, string endup)
        {
            startingLocation = startup;
            finalLocation    = endup;
        }
    }
}