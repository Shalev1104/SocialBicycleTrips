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
        public static Bitmap TransferMediaImages(string url)
        {
            Bitmap mIcon11 = null;
            try
            {
                mIcon11 = BitmapFactory.DecodeStream(new Java.Net.URL(url).OpenStream());
            }
            catch (Exception e)
            {
                
            }
            return mIcon11;
        }

        public static Bitmap DownloadImageByUrl(String src)
        {
            try
            {
                HttpWebRequest wreq;
                HttpWebResponse wresp;
                Stream mystream;
                Bitmap bmp;

                bmp = null;
                mystream = null;
                wresp = null;
                try
                {
                    wreq = (HttpWebRequest)WebRequest.Create(src);
                    wreq.AllowWriteStreamBuffering = true;

                    wresp = (HttpWebResponse)wreq.GetResponse();

                    if ((mystream = wresp.GetResponseStream()) != null)
                    {
                        //new bitmap
                    }
                }
                finally
                {
                    if (mystream != null)
                        mystream.Close();

                    if (wresp != null)
                        wresp.Close();
                }
                return (bmp);
            }
            catch
            {
                return null;
            }
        }

    }
}