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
    public class WyslijZaproszenieActivity : AppCompatActivity
    {
        Button inviteButton;
        EditText txtLogin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WyslijZaproszenie);
            txtLogin = FindViewById<EditText>(Resource.Id.LoginText);
            inviteButton = FindViewById<Button>(Resource.Id.InviteButton);
            inviteButton.Click += inviteButton_Click;
            // Create your application here
        }
        private async void inviteButton_Click(object sender, System.EventArgs e)
        {

            using (var client = new HttpClient())
            {
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                
                client.DefaultRequestHeaders.Add("Authorization", pref.GetString("Token", String.Empty));
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/friend/addfriend");
                var json = JsonConvert.SerializeObject(txtLogin.Text);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, data);

                string result = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    Android.App.AlertDialog alert = dialog.Create();
                    alert.SetTitle("Udało się");
                    alert.SetMessage("Wysłano zaproszenie");
                    alert.SetButton("OK", (c, ev) =>
                    {
                        StartActivity(new Intent(this, typeof(MainActivity)));
                    });
                    alert.Show();
                    
                }
                else
                {
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    Android.App.AlertDialog alert = dialog.Create();
                    alert.SetTitle("Ups");
                    alert.SetMessage(result);
                    alert.SetButton("OK", (c, ev) =>
                    {
                        
                    });
                    alert.Show();
                }
            }


        }
    }
}