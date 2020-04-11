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
using Model;
using Helper;
using Dal;
using Android.Provider;
using Android.Graphics;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        private ImageButton profile;
        private EditText name;
        private EditText email;
        private EditText password;
        private EditText passwordConfirmation;
        private EditText birthday;
        private EditText phoneNumber;
        private Button register;
        private Bitmap bitmap;
        private User user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signup);
            SetViews();
            // Create your application here
        }
        public void SetViews()
        {
            profile              = FindViewById<ImageButton>(Resource.Id.imgbtnProfile);
            name                 = FindViewById<EditText>(Resource.Id.edtName);
            email                = FindViewById<EditText>(Resource.Id.edtEmail);
            password             = FindViewById<EditText>(Resource.Id.edtPW);
            passwordConfirmation = FindViewById<EditText>(Resource.Id.edtPWConfirmation);
            birthday             = FindViewById<EditText>(Resource.Id.edtBirthday);
            phoneNumber          = FindViewById<EditText>(Resource.Id.edtPhoneNumber);
            register             = FindViewById<Button>(Resource.Id.btnRegister);

            profile.Click += Profile_Click;
            register.Click += Register_Click;
        }

        private void Register_Click(object sender, EventArgs e)
        {
            if (IsVaild())
            {
                string[] dateParts = birthday.Text.Split(new char[] { '/', '.', '-', ' ' });
                DateTime dateTime = new DateTime(Convert.ToInt32((dateParts[2])), Convert.ToInt32((dateParts[1])), Convert.ToInt32((dateParts[0])));
                string image = BitMapHelper.BitMapToBase64(bitmap);

                if (profile.Background != null)
                {
                    user = new User(name.Text, email.Text, password.Text, image, dateTime, phoneNumber.Text);
                }
                else
                {
                    user = new User(name.Text, email.Text, password.Text, dateTime, phoneNumber.Text);
                }
                Intent intent = new Intent();
                intent.PutExtra("user", Serializer.ObjectToByteArray(user));
                SetResult(Result.Ok, intent);
                Finish();
            }
        }

        private void Profile_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 1);
        }
        protected override void OnActivityResult(int requestCode,
        [GeneratedEnum] Result resultCode,
        Intent data)
        {
            if (resultCode == Result.Ok)
            {
                base.OnActivityResult(requestCode, resultCode, data);

                bitmap = (Bitmap)data.Extras.Get("data");

                profile.SetImageBitmap(bitmap);
            }
        }

        public bool IsVaild()
        {
            if(!(name != null && !name.Text.Equals("")))
            {
                Toast.MakeText(this, "Type your name", ToastLength.Long).Show();
                return false;
            }
            if (!(email != null && !email.Text.Equals("")))
            {
                Toast.MakeText(this, "Type your email", ToastLength.Long).Show();
                return false;
            }
            if (!(password != null && !password.Text.Equals("")))
            {
                Toast.MakeText(this, "Type your password", ToastLength.Long).Show();
                return false;
            }
            if (!(passwordConfirmation != null && !passwordConfirmation.Text.Equals("")))
            {
                Toast.MakeText(this, "Re-type Password", ToastLength.Long).Show();
                return false;
            }
            if (!(birthday != null && !birthday.Text.Equals("")))
            {
                Toast.MakeText(this, "Type your birthday", ToastLength.Long).Show();
                return false;
            }
            if (!(phoneNumber != null && !phoneNumber.Text.Equals("")))
            {
                Toast.MakeText(this, "Type your phone number", ToastLength.Long).Show();
                return false;
            }
            if (!password.Text.Equals(passwordConfirmation.Text))
            {
                Toast.MakeText(this, "passwords does not match", ToastLength.Long).Show();
                return false;
            }
            return true;
        }
    }
}