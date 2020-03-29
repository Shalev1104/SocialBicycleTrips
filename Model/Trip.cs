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
        private DateTime   date;
        private TimeSpan   time;
        private string     about;
        private List<User> participants;

        public Trip(string startingLocation, string finalLocation, DateTime date, TimeSpan time, string about, User tripCreator)
        {
            this.startingLocation = startingLocation;
            this.finalLocation    = finalLocation;
            this.date             = date;
            this.time             = time;
            this.about            = about;

            participants.Add(tripCreator);
        }

        public string StartingLocation { get => startingLocation; set => startingLocation = value; }
        public string FinalLocation { get => finalLocation; set => finalLocation = value; }
        public DateTime DateTime { get => date; set => date = value; }
        public string About { get => about; set => about = value; }
        public List<User> Participants { get => participants; set => participants = value; }

        public void AddParticipant(User user)
        {
            participants.Add(user);
        }

        public void ChangeDateAndTime(DateTime dateTime, TimeSpan time)
        {
            date      = dateTime;
            this.time = time;
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