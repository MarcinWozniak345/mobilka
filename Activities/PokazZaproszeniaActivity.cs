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
    public class PokazZaproszeniaActivity : AppCompatActivity
    {
        ListView zaproszenia;
        List<string> lista;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PokazZaproszenia);
            zaproszenia = FindViewById<ListView>(Resource.Id.list_Zaproszenia);
            using (var client = new HttpClient())
            {
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

                client.DefaultRequestHeaders.Add("Authorization", pref.GetString("Token", String.Empty));
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/friend/showinvitations");
                var response = await client.GetAsync(uri);
                string result = response.Content.ReadAsStringAsync().Result;
                var wynik = JsonConvert.DeserializeObject<List<ShowInvitationsDto>>(result);
                lista = new List<string>();


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (var i in wynik)
                    {
                        lista.Add(i.login);
                    }
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lista);

                    zaproszenia.Adapter = adapter;

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
            zaproszenia.ItemClick += zaproszenia_ItemClick;
        }
        void zaproszenia_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            //edit.Apply();
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle(lista[e.Position]);
            alert.SetMessage("czy chcesz przyjac zaproszenie?");
            alert.SetButton("Przyjmij",async (c, ev) =>
            {
                var potwierdz = new ConfirmInvitationDto()
                {
                    name = lista[e.Position],
                    ifconfirm = true
                };


                using (var client = new HttpClient())
                {
                    var uri = new Uri("https://inz-api-app.azurewebsites.net/api/friend/confirmfriend");
                    var json = JsonConvert.SerializeObject(potwierdz);
                    client.DefaultRequestHeaders.Add("Authorization", pref.GetString("Token", String.Empty));
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(uri, data);
                    string result = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                        Android.App.AlertDialog alert = dialog.Create();
                        alert.SetTitle("Udało się");
                        alert.SetMessage("przyjeto zaproszenie");
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
            });
            
            alert.SetButton2("Odrzuc",async (c, ev) =>
            {
                var potwierdz = new ConfirmInvitationDto()
                {
                    name = lista[e.Position],
                    ifconfirm = false
                };


                using (var client = new HttpClient())
                {
                    var uri = new Uri("https://inz-api-app.azurewebsites.net/api/friend/confirmfriend");
                    var json = JsonConvert.SerializeObject(potwierdz);
                    client.DefaultRequestHeaders.Add("Authorization", pref.GetString("Token", String.Empty));
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(uri, data);
                    string result = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                        Android.App.AlertDialog alert = dialog.Create();
                        alert.SetTitle("Udało się");
                        alert.SetMessage("odrzucono zaproszenie");
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
            });
            alert.Show();


        }
    }
}