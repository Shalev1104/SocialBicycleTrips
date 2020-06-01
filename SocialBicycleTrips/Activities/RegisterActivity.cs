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
using System.Threading.Tasks;
using Plugin.Media;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        private Refractored.Controls.CircleImageView profile;
        private EditText name;
        private EditText email;
        private EditText password;
        private EditText passwordConfirmation;
        private EditText birthday;
        private EditText phoneNumber;
        private Button register;
        private CheckBox registerationRememberMe;
        private Bitmap bitmap;
        private User user;
        private Users users;

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
            users = new Users().GetAllUsers();
            SetViews();
            // Create your application here
        }
        public void SetViews()
        {
            profile              = FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.imgbtnProfile);
            name                 = FindViewById<EditText>(Resource.Id.edtName);
            email                = FindViewById<EditText>(Resource.Id.edtEmail);
            password             = FindViewById<EditText>(Resource.Id.edtPW);
            passwordConfirmation = FindViewById<EditText>(Resource.Id.edtPWConfirmation);
            birthday             = FindViewById<EditText>(Resource.Id.edtBirthday);
            phoneNumber          = FindViewById<EditText>(Resource.Id.edtPhoneNumber);
            register             = FindViewById<Button>(Resource.Id.btnRegister);
            registerationRememberMe = FindViewById<CheckBox>(Resource.Id.chboxRememberMeRegisteration);

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
                    user = new User(name.Text, email.Text, password.Text, BitMapHelper.BitMapToBase64(BitmapFactory.DecodeResource(Resources, Resource.Drawable.StandardProfileImage)), dateTime, phoneNumber.Text); ;
                }
                if (users.Exists(user))
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDiag = new Android.Support.V7.App.AlertDialog.Builder(this);

                    alertDiag.SetTitle("Exists");
                    alertDiag.SetMessage("Email already exists");

                    alertDiag.SetCancelable(true);

                    alertDiag.SetPositiveButton("OK", (senderAlert, args)
                    => {
                        alertDiag.Dispose();
                    });

                    Dialog diag = alertDiag.Create();
                    diag.Show();

                }
                else
                {
                    Intent intent = new Intent();
                    intent.PutExtra("user", Serializer.ObjectToByteArray(user));
                    if (registerationRememberMe.Checked)
                    {
                        RememberMe();
                    }
                    SetResult(Android.App.Result.Ok, intent);
                    Finish();
                }
            }
        }

        private void RememberMe()
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            ISharedPreferencesEditor editor = pref.Edit();
            editor.PutString("user", Android.Util.Base64.EncodeToString(Serializer.ObjectToByteArray(user), Android.Util.Base64.Default));
            editor.PutInt("userId", user.Id);
            editor.PutInt("OngoingTrips", user.UpcomingTrips);
            editor.PutInt("CompletedTrips", user.CompletedTrips);
            editor.Apply();
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

        private async void Gallery_Click(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Upload not supported on this device", ToastLength.Long).Show();
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 40

            });
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            profile.SetImageBitmap(bitmap);
            dialog.Dismiss();
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
                    Toast.MakeText(this, "password must be at least 8 digits", ToastLength.Long).Show();
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