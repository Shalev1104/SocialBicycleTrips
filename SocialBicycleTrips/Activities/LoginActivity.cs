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
using System.Threading;
using Java.Lang;
using GoogleGson;
using Java.Util;

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
        private User user;
        private bool isSocialFirstConnect = false;

        GoogleSignInOptions gso;
        GoogleApiClient googleApiClient;

        FirebaseAuth firebaseAuth;
        private bool usingFirebase;
        private bool googlePressed = false;
        private CheckBox rememberUser;

        private ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        ICallbackManager callbackManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signin);
            SetViews();
            FacebookSdk.SdkInitialize(ApplicationContext);
            // Create your application here
            users = new Users().GetAllUsers();
        }
        public void SetViews()
        {
            email = FindViewById<EditText>(Resource.Id.edtLoginEmail);
            password = FindViewById<EditText>(Resource.Id.edtLoginPW);
            login = FindViewById<Button>(Resource.Id.btnLogin);
            signup = FindViewById<Button>(Resource.Id.btnSignup);
            googleLogin = FindViewById<SignInButton>(Resource.Id.btnGoogleLogin);
            facebookLogin = FindViewById<LoginButton>(Resource.Id.btnFacebookLogin);
            rememberUser = FindViewById<CheckBox>(Resource.Id.chboxRememberMe);

            login.Click += Login_Click;
            signup.Click += Signup_Click;
            googleLogin.Click += GoogleLogin_Click;

            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn).RequestIdToken("994671586450-p7s56itpn9kcb2him4pf7cnkvkashhmf.apps.googleusercontent.com").RequestEmail().Build();
            googleApiClient = new GoogleApiClient.Builder(this).AddApi(Auth.GOOGLE_SIGN_IN_API, gso).Build();
            googleApiClient.Connect();

            login.Enabled = true;
            facebookLogin.Enabled = true;
            googleLogin.Enabled = true;
            signup.Enabled =true;

            facebookLogin.SetReadPermissions(new List<string> { "public_profile"});
            callbackManager = CallbackManagerFactory.Create();
            facebookLogin.RegisterCallback(callbackManager, this);
            firebaseAuth = GetFirebaseAuth();
            if (IsHereToDisconnect())
            {
                firebaseAuth.SignOut();
                if (IsFacebookLogin())
                {
                    LoginManager.Instance.LogOut();
                    facebookLogin.UnregisterCallback(callbackManager);
                }
                Finish();
            }
        }

        public bool IsFacebookLogin()
        {
            AccessToken accessToken = AccessToken.CurrentAccessToken;
            bool isLoggedIn = accessToken != null && !accessToken.IsExpired;
            return isLoggedIn;
        }

        public bool IsHereToDisconnect()
        {
            return Intent.HasExtra("social media disconnect");
        }
        private void GoogleLogin_Click(object sender, EventArgs e)
        {
            googlePressed = true;
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
        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            login.Enabled = false;
            facebookLogin.Enabled = false;
            googleLogin.Enabled = false;
            signup.Enabled = false;

            base.OnActivityResult(requestCode, resultCode, data);
                if (requestCode == 0)
                {

                    if (resultCode == Android.App.Result.Ok)
                    {
                        user = Serializer.ByteArrayToObject(data.GetByteArrayExtra("user")) as User;
                        if (!users.Exists(user))
                        {
                            Toast.MakeText(this, "Registeration successfull", ToastLength.Long).Show();
                            if (data.HasExtra("checked"))
                            {
                                if(data.GetBooleanExtra("checked",false) == true)
                                {
                                    RememberMe();
                                }
                            }
                            Navigate(user, true);
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
                    }
                }
                else if (requestCode == 1)
                {
                    GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                    if (result.IsSuccess)
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

        private void RememberMe()
        {
            ISharedPreferencesEditor editor = pref.Edit();
            editor.PutString("user", Android.Util.Base64.EncodeToString(Serializer.ObjectToByteArray(user),Android.Util.Base64.Default));
            editor.Apply();
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
                user = IsLogin();
                if (user != null)
                {
                    Toast.MakeText(this, "login succesfull", ToastLength.Long).Show();
                    Navigate(user, false);
                }
                else
                {
                    Toast.MakeText(this, "email or password incorrect", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Type fields", ToastLength.Long).Show();
            }
        }
        public bool IsTyped()
        {
            return email.Text != null && password.Text != null && !email.Text.Equals("") && !password.Text.Equals("");
        }

        public User IsLogin()
        {
            user = null;
            foreach (User found in users)
            {
                if (!found.IsSocialMediaLogon())
                {
                    if (found.Email.Equals(email.Text) && found.Password.Equals(password.Text))
                    {
                        user = found;
                        break;
                    }
                }
            }
            return user;
        }
        public User IsSocialLogin()
        {
            user = null;
            foreach (User found in users)
            {
                if (found.IsSocialMediaLogon())
                {
                    if (found.Email.Equals(firebaseAuth.CurrentUser.Email))
                    {
                        user = found;
                        break;
                    }
                }
            }
            return user;
        }
        public void Navigate(User user, bool toAdd)
        {
                Intent intent = new Intent();
                intent.PutExtra("user", Serializer.ObjectToByteArray(user));
                if (toAdd)
                {
                    intent.PutExtra("toAdd", true);
                }
                SetResult(Android.App.Result.Ok, intent);
                Finish();
        }


        // Google & Facebook
        public void OnSuccess(Java.Lang.Object result)
        {
                if (!googlePressed) // for facebook
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
                        user = IsSocialLogin();
                        if (user == null)
                        {
                            user = new User(firebaseAuth.CurrentUser.DisplayName, firebaseAuth.CurrentUser.Email, "https://graph.facebook.com/" + firebaseAuth.CurrentUser.PhotoUrl.Path + "?type=normal", firebaseAuth.CurrentUser.PhoneNumber);
                            isSocialFirstConnect = true;
                        }
                        if (rememberUser.Checked)
                        {
                            RememberMe();
                        }
                        Toast.MakeText(this, "login succesfull", ToastLength.Long).Show();
                        Navigate(user, isSocialFirstConnect);
                    }
                }

                else // for google
                {
                    user = IsSocialLogin();
                    if (user == null)
                    {
                        user = new User(firebaseAuth.CurrentUser.DisplayName, firebaseAuth.CurrentUser.Email, "https://lh3.googleusercontent.com" + firebaseAuth.CurrentUser.PhotoUrl.Path, firebaseAuth.CurrentUser.PhoneNumber);
                        isSocialFirstConnect = true;
                    }
                    if (rememberUser.Checked)
                    {
                        RememberMe();
                    }
                    Toast.MakeText(this, "login succesfull", ToastLength.Long).Show();
                    Navigate(user, isSocialFirstConnect);
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
            Toast.MakeText(this, "error,try again", ToastLength.Long).Show();
        }
    }
}