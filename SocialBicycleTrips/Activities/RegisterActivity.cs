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
using Android.Graphics.Drawables;
using Java.Sql;
using System.Net.Mail;

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

        private Dialog dialog;
        private LinearLayout cameraFrame;
        private ImageButton btnCamera;
        private LinearLayout galleryFrame;
        private ImageButton btnGallery;
        private Button btnCancel;
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

                if (bitmap != null)
                {
                    string image = BitMapHelper.BitMapToBase64(bitmap);
                    user = new User(name.Text, email.Text, password.Text, image, dateTime, phoneNumber.Text);
                }
                else
                {
                    user = new User(name.Text, email.Text, password.Text, dateTime, phoneNumber.Text);
                }
                Intent intent = new Intent();
                intent.PutExtra("user", Serializer.ObjectToByteArray(user));
                SetResult(Android.App.Result.Ok,intent);
                Finish();
            }
        }

        private void Profile_Click(object sender, EventArgs e)
        {
            PerformCustomDialog();
        }

        public void PerformCustomDialog()
        {
            dialog = new Dialog(this);
            dialog.SetContentView(Resource.Layout.activity_cameraOptions);
            dialog.SetCancelable(true);

            cameraFrame  = dialog.FindViewById<LinearLayout>(Resource.Id.frameCamera);
            galleryFrame = dialog.FindViewById<LinearLayout>(Resource.Id.frameGallery);
            btnCamera    = dialog.FindViewById<ImageButton>(Resource.Id.btnCameraProfile);
            btnGallery   = dialog.FindViewById<ImageButton>(Resource.Id.btnGalleryProfile);
            btnCancel    = dialog.FindViewById<Button>(Resource.Id.btnCancelImage);

            cameraFrame.Click  += Camera_Click;
            btnCamera.Click    += Camera_Click;

            btnGallery.Click   += Gallery_Click;
            galleryFrame.Click += Gallery_Click;

            btnCancel.Click    += BtnCancel_Click;
            dialog.Show();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            dialog.Dismiss();
        }

        private void Gallery_Click(object sender, EventArgs e)
        {
            StartActivityForResult(new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri), 2);
        }

        private void Camera_Click(object sender, EventArgs e)
        {
            StartActivityForResult(new Intent(MediaStore.ActionImageCapture), 1);
        }

        protected override void OnActivityResult(int requestCode,
        [GeneratedEnum] Android.App.Result resultCode,
        Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                if (resultCode == Android.App.Result.Ok)
                {
                    bitmap = (Bitmap)data.Extras.Get("data");

                    profile.SetImageBitmap(bitmap);

                    dialog.Dismiss();
                }
            }
            if (requestCode == 2)
            {
                if (resultCode == Android.App.Result.Ok && data != null)
                {
                    bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, data.Data);

                    profile.SetImageBitmap(bitmap);

                    dialog.Dismiss();
                }
            }
        }

        public bool IsVaild()
        {
            name.Background.ClearColorFilter();
            email.Background.ClearColorFilter();
            password.Background.ClearColorFilter();
            passwordConfirmation.Background.ClearColorFilter();
            birthday.Background.ClearColorFilter();
            phoneNumber.Background.ClearColorFilter();

            if (!(name != null && !name.Text.Equals("")))
            {
                Toast.MakeText(this, "Type your name", ToastLength.Long).Show();
                name.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                return false;
            }

            if (!IsValidEmail())
            {
                Toast.MakeText(this, "invalid email address", ToastLength.Long).Show();
                email.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                return false;
            }

            if (password != null && !password.Text.Equals(""))
            {
                if (password.Text.Length < 8)
                {
                    Toast.MakeText(this, "Password must be over 7 digits", ToastLength.Long).Show();
                    password.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                    return false;
                }
            }
            else
            {
                Toast.MakeText(this, "Type your password", ToastLength.Long).Show();
                password.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                return false;
            }
            if (!(passwordConfirmation != null && !passwordConfirmation.Text.Equals("")))
            {
                Toast.MakeText(this, "Re-type Password", ToastLength.Long).Show();
                passwordConfirmation.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                return false;
            }
            if (!password.Text.Equals(passwordConfirmation.Text))
            {
                Toast.MakeText(this, "passwords does not match", ToastLength.Long).Show();
                passwordConfirmation.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                return false;
            }
            if (!(birthday != null && !birthday.Text.Equals("")))
            {
                Toast.MakeText(this, "Type a date", ToastLength.Long).Show();
                birthday.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                return false;
            }
            if (!(phoneNumber != null && !phoneNumber.Text.Equals("") && phoneNumber.Text.Length == 10))
            {
                Toast.MakeText(this, "invaild phone number", ToastLength.Long).Show();
                phoneNumber.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                return false;
            }
            return true;
        }
        public bool IsValidEmail()
        {
            try
            {
                MailAddress addr = new MailAddress(email.Text);
                return addr.Address == email.Text;
            }
            catch
            {
                return false;
            }
        }
    }
}