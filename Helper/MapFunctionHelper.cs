using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Maps.Android;
using Java.Util;
using Newtonsoft.Json;

namespace Helper
{
    public class MapFunctionHelper
    {
        string mapkey;
        GoogleMap googleMap;
        public double distance;
        public MapFunctionHelper(string mapkey,GoogleMap googleMap)
        {
            this.mapkey = mapkey;
            this.googleMap = googleMap;
        }
        public string GetGeoCodeUrl(double lat, double lng)
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lng + "&key=" + mapkey;
            return url;
        }
        public async Task<string> GetGeoJsonAsync(string url)
        {
            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string result = await client.GetStringAsync(url);
            return result;
        }
        public async Task<string> FindCordinateAddress(LatLng position)
        {
            string url = GetGeoCodeUrl(position.Latitude, position.Longitude);
            string json = "";
            string placeAddress = "";

            json = await GetGeoJsonAsync(url);
            if (!string.IsNullOrEmpty(json))
            {
                var geoCodeData = JsonConvert.DeserializeObject<GeocodingParser>(json);
                if (geoCodeData.status.Contains("ZERO"))
                {
                    if(geoCodeData.results[0] != null)
                    {
                        placeAddress = geoCodeData.results[0].formatted_address;
                    }
                }
            }
            return placeAddress;
        }
        public string getCityNameFromAddress(string address)
        {
            string[] value = address.Split(",");
            int count = value.Length;
            return value[count - 3];
        }
        public async Task<string> GetDirectionJsonAsync(LatLng location,LatLng destination)
        {
            string str_origin = "origin=" + location.Latitude + "," + location.Longitude;
            string str_destination = "destination=" + destination.Latitude + "," + destination.Longitude;
            string mode = "mode=driving";
            string parameters = str_origin + "&" + str_destination + "&" + "&" + mode + "&key=";
            string output = "json";
            string key = mapkey;
            string url = "https://maps.googleapis.com/maps/api/directions/" + output + "?" + parameters + key;
            string json = "";
            json = await GetGeoJsonAsync(url);
            return json;
        }

        public void DrawTripOnMap(string json)
        {
            var directionData = JsonConvert.DeserializeObject<DirectionParser>(json);
            var points = directionData.routes[0].overview_polyline.points;
            var line = PolyUtil.Decode(points);
            ArrayList routeList = new ArrayList();
            foreach(LatLng item in line)
            {
                routeList.Add(item);
            }
            PolylineOptions polylineOptions = new PolylineOptions().AddAll(routeList).InvokeWidth(10).InvokeColor(Color.Teal).InvokeStartCap(new SquareCap()).InvokeEndCap(new SquareCap()).InvokeJointType(JointType.Round).Geodesic(true);
            Android.Gms.Maps.Model.Polyline polyline = googleMap.AddPolyline(polylineOptions);
            LatLng firstPoint = line[0];
            LatLng lastPoint = line[line.Count-1];

            MarkerOptions firstLocationMarkerOptions = new MarkerOptions();
            firstLocationMarkerOptions.SetPosition(firstPoint);
            firstLocationMarkerOptions.SetTitle("starting location");
            firstLocationMarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));

            MarkerOptions lastLocationMarkerOptions = new MarkerOptions();
            lastLocationMarkerOptions.SetPosition(lastPoint);
            lastLocationMarkerOptions.SetTitle("last location");
            lastLocationMarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));

            Marker firstLocationMarker = googleMap.AddMarker(firstLocationMarkerOptions);
            Marker lastLocationMarker = googleMap.AddMarker(lastLocationMarkerOptions);

            double southlng = directionData.routes[0].bounds.southwest.lng;
            double southlat = directionData.routes[0].bounds.southwest.lat;
            double northlng = directionData.routes[0].bounds.northeast.lng;
            double northlat = directionData.routes[0].bounds.northeast.lat;

            LatLng southwest = new LatLng(southlat, southlng);
            LatLng northeast = new LatLng(northlat, northlng);

            LatLngBounds tripBound = new LatLngBounds(southwest, northeast);
            googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(tripBound, 470));
            googleMap.SetPadding(40, 70, 40, 70);
            firstLocationMarker.ShowInfoWindow();

            double distanceMeters = directionData.routes[0].legs[0].distance.value;
            distance = (distanceMeters / 1000);
        }
    }
}