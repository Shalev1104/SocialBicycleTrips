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
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Model
{
    [Serializable]
    [Table("Trips")]
    public class Trip : BaseEntity, iChangeable.TripChangeable
    {

        private Location startingLocation;
        private Location finalLocation;
        private DateTime dateTime;
        private string notes;

        [Ignore]
        private Participants participants { get; set; }
        [Ignore]
        private TripManager tripManager { get; set; }

        public Trip()
        {

        }
        public Trip(Location startingLocation, Location finalLocation, DateTime dateTime, string notes, TripManager tripManager)
        {
            this.startingLocation = startingLocation;
            this.finalLocation    = finalLocation;
            this.dateTime         = dateTime;
            this.notes            = notes;
            this.tripManager      = tripManager;

            participants = new Participants(); 
            participants.Add(new Participant(tripManager.Id, this.Id));
        }

        public Location StartingLocation { get => startingLocation; set => startingLocation = value; }
        public Location FinalLocation { get => finalLocation; set => finalLocation = value; }
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public string Notes { get => notes; set => notes = value; }
        public Participants Participants { get => participants; set => participants = value; }
        public TripManager TripManager { get => tripManager; set => tripManager = value; }

        public void ChangeDateAndTime(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public void UpdateDetails(string updatedDetails)
        {
            notes = updatedDetails;
        }

        public void UpdateLocation(string startup, string endup)
        {
            
        }
    }
}