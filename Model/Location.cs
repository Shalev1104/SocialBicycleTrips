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
        private LatLng coordinate;

        public string Address { get => address; set => address = value; }
        public string Name { get => name; set => name = value; }
        public LatLng Coordinate { get => coordinate; set => coordinate = value; }

        public Location(string name, string address, double latitude, double longitude)
        {
            this.name = name;
            this.address = address;
            coordinate = new LatLng(latitude, longitude);
        }
        public Location(string name, string address, LatLng lat)
        {
            this.name = name;
            this.address = address;
            coordinate = new LatLng(lat.Latitude, lat.Longitude);
        }
        public Location(string address, double latitude, double longitude)
        {
            this.address = address;
            coordinate = new LatLng(latitude, longitude);
        }
        public Location(string address, LatLng lat)
        {
            this.address = address;
            coordinate = new LatLng(lat.Latitude, lat.Longitude);
        }
    }
}