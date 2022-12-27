using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WezLekApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        EditText txtlogin;
        EditText txtpassword;
        Button btnlogin;
        Button btnregister;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            txtlogin = FindViewById<EditText>(Resource.Id.LoginText);
            txtpassword = FindViewById<EditText>(Resource.Id.PasswordText);
            btnlogin = FindViewById<Button>(Resource.Id.loginButton);
            btnregister = FindViewById<Button>(Resource.Id.RegisterButton);
            btnlogin.Click += Btnlogin_Click;
            btnregister.Click += BtnRegister_Click;
            // Create your application here
        }
        private void BtnRegister_Click(object sender, System.EventArgs e)
        {
            SetContentView(Resource.Layout.Registration);
        }
        private void Btnlogin_Click(object sender, System.EventArgs e)
        {
            //HttpClient client = new HttpClient();
            //var uri = new Uri("https://inz-api-app.azurewebsites.net");


            //SetContentView(Resource.Layout.activity_main);

            // throw new System.NotImplementedException();
            SetContentView(Resource.Layout.activity_main);
        }

    }
}