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
using Helper;
using Model;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "AddPhoneAndBirthdayActivity")]
    public class AddPhoneAndBirthdayActivity : Activity
    {
        private EditText phoneField;
        private Button btnBirthday;
        private Button btnSave;
        private DatePickerDialog datePicker;
        private DateTime birthday;
        private User user;
        private Users users;
        private bool bool1;
        private bool bool2;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_addPhoneAndBirthday);
            user = Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user")) as User;
            SetViews();
            ShowMissedFields();
            users = new Users().GetAllUsers();
            // Create your application here
        }

        private void ShowMissedFields()
        {
            if (user.PhoneNumber != null)
            {
                phoneField.Visibility = ViewStates.Gone;
                bool1 = true;
            }
            else
            {
                bool1 = false;
            }
            if (!user.CalculateAge().ToString().Equals("2019"))
            {
                btnBirthday.Visibility = ViewStates.Gone;
                bool2 = true;
            }
            else
            {
                bool2 = false;
            }
        }

        public void SetViews()
        {
            phoneField = FindViewById<EditText>(Resource.Id.btnAddPhoneNumber);
            btnBirthday = FindViewById<Button>(Resource.Id.btnAddBirthday);
            btnSave = FindViewById<Button>(Resource.Id.btnSavePhoneBirthday);
            btnBirthday.Click += BtnBirthday_Click;
            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(phoneField != null && !phoneField.Text.Equals("") && phoneField.Text.Length == 10)
            {
                user.PhoneNumber = phoneField.Text;
                Toast.MakeText(this, "Added Successfully", ToastLength.Long).Show();
                phoneField.Visibility = ViewStates.Gone;
                users.Update(user);
                OnStop();
                bool1 = true;
            }
            if(!btnBirthday.Text.Equals("Pick birthday") && birthday < DateTime.Today)
            {
                user.DateTime = birthday;
                Toast.MakeText(this, "Added Successfully", ToastLength.Long).Show();
                btnBirthday.Visibility = ViewStates.Gone;
                users.Update(user);
                OnStop();
                bool1 = true;
            }
            if(bool1 && bool2)
            {
                StartActivity(new Intent(this, typeof(MainActivity)).PutExtra("user", Serializer.ObjectToByteArray(user)));
            }
            else if(!bool1 && !bool2)
            {
                Toast.MakeText(this, "invalid fields", ToastLength.Long).Show();
            }
        }

        private void BtnBirthday_Click(object sender, EventArgs e)
        {
            datePicker = new DatePickerDialog(this, OnDateClick, DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day);
            datePicker.Show();
        }

        private void OnDateClick(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            birthday = new DateTime(e.Date.Year, e.Date.Month, e.Date.Day);
            btnBirthday.Text = e.Date.Day + "/" + e.Date.Month + "/" + e.Date.Year;
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
    }
}