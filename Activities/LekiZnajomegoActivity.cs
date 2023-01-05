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
    public class LekiZnajomegoActivity : AppCompatActivity
    {
        ListView leki;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LekiZnajomego);
            leki = FindViewById<ListView>(Resource.Id.list_lekiznajomego);
            using (var client = new HttpClient())
            {
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                string znajomy = pref.GetString("Znajomy", String.Empty);
                client.DefaultRequestHeaders.Add("Authorization", pref.GetString("Token", String.Empty));
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/medicament/showfriendmedicaments/" + znajomy);
                var response = await client.GetAsync(uri);
                string result = response.Content.ReadAsStringAsync().Result;
                var wynik = JsonConvert.DeserializeObject<List<GetMedicamentDto>>(result);
                List<string> lista = new List<string>();


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (var i in wynik)
                    {
                        lista.Add((i.Name + " dzienna dawka: " + i.DailyDosage.ToString() + " w opakowaniu " + i.NumberOfTablets.ToString()));
                    }
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lista);

                    leki.Adapter = adapter;

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
    }
}