using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using Org.Apache.Commons.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using WezLekApp.Models;

namespace WezLekApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        EditText txtlogin;
        EditText txtpassword;
        Button btnlogin;
        Button btnregister;
        ImageView imageview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

            SetContentView(Resource.Layout.Login);
            imageview = FindViewById<ImageView>(Resource.Id.image_view);
            txtlogin = FindViewById<EditText>(Resource.Id.LoginText);
            txtpassword = FindViewById<EditText>(Resource.Id.PasswordText);
            btnlogin = FindViewById<Button>(Resource.Id.loginButton);
            btnregister = FindViewById<Button>(Resource.Id.RegisterButton);
            Bitmap bitmap = BitmapFactory.DecodeResource(ApplicationContext.Resources, Resource.Drawable.logo);
            imageview.SetImageBitmap(bitmap);
            btnlogin.Click += Btnlogin_Click;
            btnregister.Click += BtnRegister_Click;
            // Create your application here
        }
        private void BtnRegister_Click(object sender, System.EventArgs e)
        {
            StartActivity(new Intent(this, typeof(RegistrationActivity)));
        }
        private async void Btnlogin_Click(object sender, System.EventArgs e)
        {
            var User = new LoginDto()
            {
                Login = txtlogin.Text,
                Password = txtpassword.Text               
            };
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/account/login");
                var json = JsonConvert.SerializeObject(User);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, data);
                string result = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                    ISharedPreferencesEditor edit = pref.Edit();
                    edit.PutString("Token", "Bearer "+ result);
                    
                    edit.Apply();

                    StartActivity(new Intent(this, typeof(MainActivity)));
                }
                else 
                {
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    Android.App.AlertDialog alert = dialog.Create();
                    alert.SetTitle("Ups");
                    alert.SetMessage(result);
                    alert.SetButton("OK", (c, ev) =>
                    {
                        //StartActivity(new Intent(this, typeof(LoginActivity)));
                    });
                    alert.Show();
                }
            }
            
            
        }

    }
}