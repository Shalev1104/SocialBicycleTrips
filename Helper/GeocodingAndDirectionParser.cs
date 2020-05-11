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

namespace Helper
{
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public IList<string> types { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Result
    {
        public IList<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public IList<string> types { get; set; }
    }

    public class GeocodingParser
    {
        public IList<Result> results { get; set; }
        public string status { get; set; }
    }
    public class GeocodedWaypoint
    {
        public string geocoder_status { get; set; }
        public string place_id { get; set; }
        public IList<string> types { get; set; }

    }
    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }
    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class EndLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class StartLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class Polyline
    {
        public string points { get; set; }
    }
    public class Step
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public EndLocation end_location { get; set; }
        public string html_instructions { get; set; }
        public Polyline polyline { get; set; }
        public StartLocation start_location { get; set; }
        public string travel_mode { get; set; }
        public string maneuver { get; set; }

    }
    public class Leg
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string end_address { get; set; }
        public EndLocation end_location { get; set; }
        public string start_address { get; set; }
        public StartLocation start_location { get; set; }
        public IList<Step> steps { get; set; }
        public IList<object> traffic_speed_entry { get; set; }
        public IList<object> via_waypoint { get; set; }
    }
    public class OverviewPolyline
    {
        public string points { get; set; }
    }
    public class Route
    {
        public Bounds bounds { get; set; }
        public string copyrights { get; set; }
        public IList<Leg> legs { get; set; }
        public OverviewPolyline overview_polyline { get; set; }
        public string summary { get; set; }
        public IList<object> warnings { get; set; }
        public IList<object> waypoint_order { get; set; }
    }
    public class DirectionParser
    {
        public IList<GeocodedWaypoint> geocoded_waypoints { get; set; }
        public IList<Route> routes { get; set; }
        public string status { get; set; }
    }
}