using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Model
{
    [Serializable]
    public class Location
    {
        private string address;
        private string name;
        private double latitude;
        private double longitude;

        public string Address { get => address; set => address = value; }
        public string Name { get => name; set => name = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public Location(string name, string address, double latitude, double longitude)
        {
            this.name = name;
            this.address = address;
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public Location(string name, string address, LatLng lat)
        {
            this.name = name;
            this.address = address;
            latitude = lat.Latitude;
            longitude = lat.Longitude;
        }
        public Location(string address, double latitude, double longitude)
        {
            this.address = address;
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public Location(string address, LatLng lat)
        {
            this.address = address;
            latitude = lat.Latitude;
            longitude = lat.Longitude;
        }
    }
}