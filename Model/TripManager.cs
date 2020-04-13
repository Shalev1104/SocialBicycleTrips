﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Refractored.Controls;

namespace Model
{
    public class TripManager
    {
        private string image;
        private string name;

        public TripManager()
        {

        }
        public TripManager(string image, string name)
        {
            this.image = image;
            this.name = name;
        }

        public string Image { get => image; set => image = value; }
        public string Name { get => name; set => name = value; }
    }
}