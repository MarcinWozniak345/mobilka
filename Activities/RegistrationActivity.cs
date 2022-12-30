using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using WezLekApp.Models;

namespace WezLekApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class RegistrationActivity : AppCompatActivity
    {
        EditText txtEmail;
        EditText txtLogin;
        EditText txtPassword;
        EditText txtConfirmPassword;
        Button btnregister;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Registration);
            txtLogin = FindViewById<EditText>(Resource.Id.LoginText);
            txtPassword = FindViewById<EditText>(Resource.Id.PasswordText);
            txtEmail = FindViewById<EditText>(Resource.Id.Email);
            txtConfirmPassword = FindViewById<EditText>(Resource.Id.ConfirmPassword);
            btnregister = FindViewById<Button>(Resource.Id.Register);
            btnregister.Click += BtnRegister_Click;
            
            // Create your application here
        }

        private async void BtnRegister_Click(object sender, System.EventArgs e)
        {
            var newUser = new RegisterUserDto()
            {
                Email = txtEmail.Text,
                Login = txtLogin.Text,
                HasloHash= txtPassword.Text,
                PotwierdzHaslo= txtConfirmPassword.Text
            };
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/account/register");
                var json = JsonConvert.SerializeObject(newUser);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, data);

                string result = response.Content.ReadAsStringAsync().Result;
                StartActivity(new Intent(this, typeof(LoginActivity)));
            }


        }
    }
}