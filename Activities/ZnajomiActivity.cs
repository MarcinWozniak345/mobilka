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
using static Java.Util.Jar.Attributes;

namespace WezLekApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ZnajomiActivity : AppCompatActivity
    {
        ListView znajomi;
        List<string> lista = new List<string>();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Znajomi);
            znajomi = FindViewById<ListView>(Resource.Id.list_znajomi);
            znajomi.ItemClick += znajomi_ItemClick;
            using (var client = new HttpClient())
            {
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

                client.DefaultRequestHeaders.Add("Authorization", pref.GetString("Token", String.Empty));
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/friend/showfriends");
                var response = await client.GetAsync(uri);
                string result = response.Content.ReadAsStringAsync().Result;
                var wynik = JsonConvert.DeserializeObject<List<string>>(result);



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (var i in wynik)
                    {
                        lista.Add(i);
                    }
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lista);

                    znajomi.Adapter = adapter;

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
            // Create your application here
        }
        void znajomi_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();
            edit.PutString("Znajomy",lista[e.Position]);

            edit.Apply();
            Intent intent = new Intent(this, typeof(LekiZnajomegoActivity));
            StartActivity(intent);
        }
    }
}