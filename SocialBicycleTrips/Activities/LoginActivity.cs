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
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Firebase.Auth;
using Firebase;
using Android.Gms.Tasks;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity, IOnSuccessListener, IOnFailureListener
    {
        private EditText email;
        private EditText password;
        private Button login;
        private Button signup;
        private ImageButton googleLogin;
        private ImageButton facebookLogin;
        private Users users;

        GoogleSignInOptions gso;
        GoogleApiClient googleApiClient;
        FirebaseAuth firebaseAuth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            SetViews();
            // Create your application here
        }
        public void SetViews()
        {
            email = FindViewById<EditText>(Resource.Id.edtLoginEmail);
            password = FindViewById<EditText>(Resource.Id.edtLoginPW);
            login = FindViewById<Button>(Resource.Id.btnLogin);
            signup = FindViewById<Button>(Resource.Id.btnSignup);
            googleLogin = FindViewById<ImageButton>(Resource.Id.btnGoogleLogin);
            facebookLogin = FindViewById<ImageButton>(Resource.Id.btnFacebookLogin);

            login.Click += Login_Click;
            signup.Click += Signup_Click;
            googleLogin.Click += GoogleLogin_Click;
            facebookLogin.Click += FacebookLogin_Click;

            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn).RequestIdToken("994671586450-p7s56itpn9kcb2him4pf7cnkvkashhmf.apps.googleusercontent.com").RequestEmail().Build();
            googleApiClient = new GoogleApiClient.Builder(this).AddApi(Auth.GOOGLE_SIGN_IN_API, gso).Build();
            googleApiClient.Connect();
            firebaseAuth = GetFirebaseAuth();
        }

        private void FacebookLogin_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GoogleLogin_Click(object sender, EventArgs e)
        {
            Intent intent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            StartActivityForResult(intent, 1);
        }

        private FirebaseAuth GetFirebaseAuth()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseAuth mAuth;

            if(app == null)
            {
                var options = new FirebaseOptions.Builder().SetProjectId("socialbicycletrips").SetApplicationId("socialbicycletrips").SetApiKey("AIzaSyAGWOt-4kO9ACMD7ZA2GMqhnXMUTbgs6ho").SetDatabaseUrl("https://socialbicycletrips.firebaseio.com").SetStorageBucket("socialbicycletrips.appspot.com").Build();
                app = FirebaseApp.InitializeApp(this, options);
                mAuth = FirebaseAuth.Instance;
            }
            else
            {
                mAuth = FirebaseAuth.Instance;
            }
            return mAuth;
        }

        private void Signup_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));
            StartActivityForResult(intent, 0);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == 0)
            {
                bool dataSetChanged = false;

                if (resultCode == Result.Ok)
                {
                    User user = Serializer.ByteArrayToObject(data.GetByteArrayExtra("user")) as User;
                    if (!users.Exists(user))
                    {
                        users.Add(user);
                        dataSetChanged = true;
                    }
                    Intent transfer = new Intent(this, typeof(MainActivity));

                    if (dataSetChanged)
                    {
                        users.Sort();
                    }
                    else
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDiag1 = new Android.Support.V7.App.AlertDialog.Builder(this);

                        alertDiag1.SetTitle("Exists");
                        alertDiag1.SetMessage("Friend already exists");

                        alertDiag1.SetCancelable(true);

                        alertDiag1.SetPositiveButton("OK", (senderAlert, args) => { alertDiag1.Dispose(); });

                        Dialog diagl = alertDiag1.Create();
                        diagl.Show();
                    }
                    StartActivity(transfer);
                }
            }
            else if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if(result.IsSuccess)
                {
                    GoogleSignInAccount account = result.SignInAccount;
                    LoginWithFirebase(account);
                }
            }
        }

        private void LoginWithFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            firebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(this).AddOnFailureListener(this);
        }

        private void Login_Click(object sender, EventArgs e)
        {
            if (IsTyped())
            {
                if (IsExist())
                {

                }
                else
                {
                    Toast.MakeText(this, "email or password incorrect", ToastLength.Long).Show();
                }
            }
        }
        public bool IsTyped()
        {
            return email.Text != null && password.Text != null && !email.Text.Equals("") && !password.Text.Equals("");
        }
        public bool IsExist()
        {
            return false;
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            User user = new User();
            user.Name = firebaseAuth.CurrentUser.DisplayName;
            user.Email = firebaseAuth.CurrentUser.Email;
            user.PhoneNumber = firebaseAuth.CurrentUser.PhoneNumber;
            user.Image = firebaseAuth.CurrentUser.PhotoUrl.Path;
            //user.DateTime = firebaseAuth.CurrentUser;
            Toast.MakeText(this, "login succesfull", ToastLength.Long);
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            Toast.MakeText(this, "login failed", ToastLength.Long);
        }
    }
}