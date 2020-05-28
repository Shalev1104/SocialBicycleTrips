using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Helper;
using Model;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "ChangePasswordActivity")]
    public class ChangePasswordActivity : Activity
    {
        private EditText oldPW;
        private EditText newPW;
        private EditText newPWRetyper;
        private Button btnSave;
        private User user;
        private Users users;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_changePassword);
            SetViews();
            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
            users = new Users().GetAllUsers();
            // Create your application here
        }
        public void SetViews()
        {
            oldPW = FindViewById<EditText>(Resource.Id.edtOldPassword);
            newPW = FindViewById<EditText>(Resource.Id.edtNewPassword);
            newPWRetyper = FindViewById<EditText>(Resource.Id.edtRetypeNewPassword);
            btnSave = FindViewById<Button>(Resource.Id.btnUpdatePassword);

            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (IsOK())
            {
                Toast.MakeText(this, "password has been changed", ToastLength.Long).Show();
                user.Password = newPW.Text;
                users.Update(user);
                StartActivity(new Intent(this, typeof(MainActivity)).PutExtra("user", Serializer.ObjectToByteArray(user)));
            }
            
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (Intent.HasExtra("user") && Settings.RememberMe)
            {
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                ISharedPreferencesEditor editor = pref.Edit();
                editor.PutString("user", Android.Util.Base64.EncodeToString(Serializer.ObjectToByteArray(user), Android.Util.Base64.Default));
                editor.PutInt("userId", user.Id);
                editor.PutInt("OngoingTrips", user.UpcomingTrips);
                editor.PutInt("CompletedTrips", user.CompletedTrips);
                editor.Apply();
            }
        }
        public bool IsOK()
        {
            oldPW.Background.ClearColorFilter();
            newPW.Background.ClearColorFilter();
            newPWRetyper.Background.ClearColorFilter();

            if (!(oldPW != null && !oldPW.Text.Equals("")))
            {
                oldPW.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                Toast.MakeText(this, "Type your current password", ToastLength.Long).Show();
                return false;
            }
            if (!oldPW.Text.Equals(user.Password))
            {
                oldPW.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                Toast.MakeText(this, "Incorrect old password", ToastLength.Long).Show();
                return false;
            }
            if (!(newPW != null && !newPW.Text.Equals("")))
            {
                newPW.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                Toast.MakeText(this, "Type new password", ToastLength.Long).Show();
                return false;
            }
            if (newPW.Text.Length < 8)
            {
                newPW.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                Toast.MakeText(this, "password must be at least 8 digits", ToastLength.Long).Show();
                return false;
            }
            if (!(newPWRetyper != null && !newPWRetyper.Text.Equals("")))
            {
                newPWRetyper.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                Toast.MakeText(this, "Retype new password", ToastLength.Long).Show();
                return false;
            }
            if (!newPWRetyper.Text.Equals(newPW.Text))
            {
                newPWRetyper.Background.SetColorFilter(new Color(Color.Red), PorterDuff.Mode.SrcIn);
                Toast.MakeText(this, "passwords are not the same", ToastLength.Long).Show();
                return false;
            }
            return true;
        }
    }
}