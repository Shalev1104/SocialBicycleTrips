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
using Xamarin.Facebook.Login.Widget;
using Android.Gms.Common;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace SocialBicycleTrips.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity, IOnSuccessListener, IOnFailureListener,IFacebookCallback
    {
        private EditText email;
        private EditText password;
        private Button login;
        private Button signup;
        private SignInButton googleLogin;
        private LoginButton facebookLogin;
        private Users users;
        private UsersDB usersDB;

        GoogleSignInOptions gso;
        GoogleApiClient googleApiClient;

        FirebaseAuth firebaseAuth;
        private bool usingFirebase;

        ICallbackManager callbackManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signin);
            SetViews();
            // Create your application here
            usersDB = new UsersDB();
            users = usersDB.GetAllUsers();
        }
        public void SetViews()
        {
            email = FindViewById<EditText>(Resource.Id.edtLoginEmail);
            password = FindViewById<EditText>(Resource.Id.edtLoginPW);
            login = FindViewById<Button>(Resource.Id.btnLogin);
            signup = FindViewById<Button>(Resource.Id.btnSignup);
            googleLogin = FindViewById<SignInButton>(Resource.Id.btnGoogleLogin);
            facebookLogin = FindViewById<LoginButton>(Resource.Id.btnFacebookLogin);

            login.Click += Login_Click;
            signup.Click += Signup_Click;
            googleLogin.Click += GoogleLogin_Click;

            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn).RequestIdToken("994671586450-p7s56itpn9kcb2him4pf7cnkvkashhmf.apps.googleusercontent.com").RequestEmail().Build();
            googleApiClient = new GoogleApiClient.Builder(this).AddApi(Auth.GOOGLE_SIGN_IN_API, gso).Build();
            googleApiClient.Connect();

            facebookLogin.SetReadPermissions(new List<string> { "public_profile", "email" });
            callbackManager = CallbackManagerFactory.Create();
            facebookLogin.RegisterCallback(callbackManager, this);
            firebaseAuth = GetFirebaseAuth();

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

                if (resultCode == Result.Ok)
                {
                    User user = Serializer.ByteArrayToObject(data.GetByteArrayExtra("user")) as User;
                    if (!users.Exists(user))
                    {
                        users.Add(user);
                        usersDB.Insert(user);
                        Toast.MakeText(this, "Registeration successfull", ToastLength.Long).Show();
                        users.Sort();
                    }
                    else
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDiag1 = new Android.Support.V7.App.AlertDialog.Builder(this);

                        alertDiag1.SetTitle("Exists");
                        alertDiag1.SetMessage("User already exists");

                        alertDiag1.SetCancelable(true);

                        alertDiag1.SetPositiveButton("OK", (senderAlert, args) => { alertDiag1.Dispose(); });

                        Dialog diagl = alertDiag1.Create();
                        diagl.Show();
                    }
                    Intent transfer = new Intent(this, typeof(MainActivity));
                    transfer.PutExtra("user", Serializer.ObjectToByteArray(user));
                    StartActivity(transfer);
                }
            }
            else if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if(result.IsSuccess)
                {
                    GoogleSignInAccount account = result.SignInAccount;
                    LoginWithGoogleFirebase(account);
                }
            }
            else
            {
                callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            }
        }

        private void LoginWithGoogleFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            firebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(this).AddOnFailureListener(this);
        }

        private void LoginWithFacebookFirebase(LoginResult loginResult)
        {
            var credentials = FacebookAuthProvider.GetCredential(loginResult.AccessToken.Token);
            firebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(this).AddOnFailureListener(this);
        }

        private void Login_Click(object sender, EventArgs e) 
        {
            if (IsTyped())
            {
                User user = IsLogin();
                if (user != null)
                {
                    Navigate(user);
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

        public User IsLogin() // first condition for social media login and the second for the form login
        {
            User user = null;
            foreach (User found in users)
            {
                if ((found.IsSocialMediaLogon() &&  found.Email.Equals(firebaseAuth.CurrentUser.Email)) || (!found.IsSocialMediaLogon() && found.Email.Equals(email.Text) && found.Password.Equals(password.Text)))
                {
                    user = found;
                    break;
                }
            }
            return user;
        }
        public void Navigate(User user)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("user", Serializer.ObjectToByteArray(user));
            Toast.MakeText(this, "login succesfull", ToastLength.Long).Show();
            StartActivity(intent);
        }


        // Google & Facebook
        public void OnSuccess(Java.Lang.Object result)
        {
            if (result is LoginResult) // for facebook
            {
                if (!usingFirebase)
                {
                    usingFirebase = true;
                    LoginResult loginResult = result as LoginResult;
                    LoginWithFacebookFirebase(loginResult);
                }
                else
                {
                    usingFirebase = false;
                    User user = IsLogin();
                    if (user == null)
                    {
                        user = new User(firebaseAuth.CurrentUser.DisplayName, firebaseAuth.CurrentUser.Email, firebaseAuth.CurrentUser.PhotoUrl.Path, firebaseAuth.CurrentUser.PhoneNumber);
                        users.Add(user);
                        usersDB.Insert(user);
                    }
                    Navigate(user);
                }
            }

            else // for google
            {
                User user = IsLogin();
                if (user == null)
                {
                    user = new User(firebaseAuth.CurrentUser.DisplayName, firebaseAuth.CurrentUser.Email, firebaseAuth.CurrentUser.PhotoUrl.Path, firebaseAuth.CurrentUser.PhoneNumber);
                    users.Add(user);
                    usersDB.Insert(user);
                }
                Navigate(user);
            }
        }
        public void OnFailure(Java.Lang.Exception e)
        {
            Toast.MakeText(this, "login failed", ToastLength.Long).Show();
        }

        // Facebook :
        public void OnCancel()
        {
            
        }

        public void OnError(FacebookException error)
        {
            Toast.MakeText(this, "error,try again", ToastLength.Long);
        }
    }
}