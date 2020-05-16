using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using Model;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        private Spinner mapstyle;
        private List<string> mapStyles;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);
            SetViews();
            GenerateMapStyles();
            // Create your application here
        }
        public void SetViews()
        {
            mapstyle = FindViewById<Spinner>(Resource.Id.spnMapStyle);

        }
        public void GenerateMapStyles()
        {
            mapStyles = new List<string>();
            mapStyles.Add(Model.Settings.MapStyle);

            if (Model.Settings.MapStyle != "Standard")
            {
                mapStyles.Add("Standard");
            }
            if (Model.Settings.MapStyle != "Silver")
            {
                mapStyles.Add("Silver");
            }
            if (Model.Settings.MapStyle != "Retro")
            {
                mapStyles.Add("Retro");
            }
            if (Model.Settings.MapStyle != "Dark")
            {
                mapStyles.Add("Dark");
            }
            if (Model.Settings.MapStyle != "Night")
            {
                mapStyles.Add("Night");
            }
            if (Model.Settings.MapStyle != "Aubergine")
            {
                mapStyles.Add("Aubergine");
            }

            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this,Android.Resource.Layout.SimpleSpinnerItem, mapStyles);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            mapstyle.Adapter = dataAdapter;
            mapstyle.ItemSelected += Mapstyle_ItemSelected;
        }

        private void Mapstyle_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Model.Settings.MapStyle = mapStyles[e.Position];
        }
    }
}