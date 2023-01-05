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
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Xml;
using WezLekApp.BroadCast;
using WezLekApp.Models;
using static Android.Gms.Common.Apis.Api;
using Context = Android.Content.Context;

namespace WezLekApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class WprowadzLekv2Activity : AppCompatActivity
    {
        EditText numberOfTablets;
        EditText dose;
        EditText dailyDosage;
        EditText hours;
        Button wprowadzLek;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WprowadzLekv2);
            numberOfTablets = FindViewById<EditText>(Resource.Id.numberOfTablets);
            dose = FindViewById<EditText>(Resource.Id.dose);
            dailyDosage = FindViewById<EditText>(Resource.Id.DailyDosage);
            hours = FindViewById<EditText>(Resource.Id.Hours);
            wprowadzLek = FindViewById<Button>(Resource.Id.NowyLek);
            wprowadzLek.Click += wprowadzLek_Click;
            // Create your application here
        }
        private async void wprowadzLek_Click(object sender, System.EventArgs e)
        {
            List<Time> lista = new List<Time>();
            string[] subs = hours.Text.Split(':', ',');
            for (int i = 1; i < subs.Length; i += 2)
            {
                var godzina = new Time()
                {
                    Hour = int.Parse(subs[i - 1]),
                    Minute = int.Parse(subs[i])
                };
                lista.Add(godzina);
            }
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

            //client.DefaultRequestHeaders.Add("Authorization", pref.GetString("Token", String.Empty));
            var lek = new AddMedicamentDto()
            {
                Name = pref.GetString("Lek", String.Empty),
                NumberOfTablets = int.Parse(numberOfTablets.Text),
                Dose = dose.Text,
                DailyDosage = int.Parse(dailyDosage.Text),
                Time = lista
            };


            using (var client = new HttpClient())
            {
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/medicament/addmedicament");
                var json = JsonConvert.SerializeObject(lek);
                client.DefaultRequestHeaders.Add("Authorization", pref.GetString("Token", String.Empty));
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, data);

                string result = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    Android.App.AlertDialog alert = dialog.Create();
                    alert.SetTitle("Udało się");
                    alert.SetMessage("dodano lek");
                    alert.SetButton("OK", (c, ev) =>
                    {
                        int tablets = int.Parse(numberOfTablets.Text);

                        int i = 0, k = 1;
                        while (tablets != 0)
                        {
                            DateTime time = DateTime.Now.AddDays(k);
                            for (int j = 0; j < lek.Time.Count(); j++)
                            {
                                DateTime time2 = new DateTime(time.Year, time.Month, time.Day, lek.Time[j].Hour, lek.Time[j].Minute, 0);
                                tablets--;
                                if (tablets == 0) break;
                                StartAlarm(true,true,time2);
                            }
                            k++;
                        }
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

        private void StartAlarm(bool isNotification, bool isRepeating, DateTime time)
        {
            AlarmManager manager = (AlarmManager)GetSystemService(Context.AlarmService);
            Intent myIntent;
            PendingIntent pendingIntent;

            myIntent = new Intent(this, typeof(AlarmToastReceiver));
            pendingIntent = PendingIntent.GetBroadcast(this, 0, myIntent, 0);


            
        }


    }
}