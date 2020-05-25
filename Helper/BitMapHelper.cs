using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bumptech.Glide;
using Java.IO;
using Java.Net;

namespace Helper
{
    public class BitMapHelper
    {
        public static string BitMapToBase64(Bitmap bitmap)
        {
            var str = "";
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);

                var bytes = stream.ToArray();
                str = Convert.ToBase64String(bytes);
            }

            return str;
        }

        public static Bitmap Base64ToBitMap(string img)
        {
            byte[] imageBytes = Base64.Decode(img, Base64Flags.Default);

            Bitmap decodedImage = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            return decodedImage;
        }
    }
}