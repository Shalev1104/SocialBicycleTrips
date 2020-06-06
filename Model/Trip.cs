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

namespace Model
{
    [Serializable]
    [Table("Trips")]
    public class Trip : BaseEntity
    {

        private byte[] startingLocation;
        private byte[] finalLocation;
        private DateTime dateTime;
        private string notes;
        private Participants participants;
        private byte[] tripManager;

        public Trip()
        {

        }
        public Trip(byte[] startingLocation, byte[] finalLocation, DateTime dateTime, string notes, byte[] tripManager)
        {
            this.startingLocation = startingLocation;
            this.finalLocation    = finalLocation;
            this.dateTime         = dateTime;
            this.notes            = notes;
            this.tripManager      = tripManager;

            participants = new Participants(); 
        }
        public byte[] StartingLocation { get => startingLocation; set => startingLocation = value; }
        public byte[] FinalLocation { get => finalLocation; set => finalLocation = value; }
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public string Notes { get => notes; set => notes = value; }

        [Ignore]
        public Participants Participants { get => participants; set => participants = value; }
        public byte[] TripManager { get => tripManager; set => tripManager = value; }
    }
}