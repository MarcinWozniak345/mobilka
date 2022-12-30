using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using WezLekApp.Activities;
using WezLekApp.Models;
using static Android.Media.Audiofx.DynamicsProcessing;
using static Java.Util.Jar.Attributes;

namespace WezLekApp.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtlogin { get; set; }
        public Button odrzuc { get; set;}
        public Button przyjmij { get; set;}
    }
   
    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<ShowInvitationsDto> lista;
        public CustomAdapter(Activity activity, List<ShowInvitationsDto> lista)
        {
            this.activity = activity;
            this.lista = lista;
        }
        public override int Count
        { 
            get 
            { 
                return lista.Count; 
            } 
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override long GetItemId(int position)
        {
            return 1;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.
                LayoutInflater.Inflate(Resource.Layout.ShowInvitationTemplate, parent, false);

            var txtlogin = view.FindViewById<TextView>(Resource.Id.login);
            var buttonOdrzuc = view.FindViewById<Button>(Resource.Id.odrzuc);
            var buttonPrzyjmij = view.FindViewById<Button>(Resource.Id.przyjmij);
            buttonOdrzuc.Click += buttonOdrzuc_Click;
            buttonPrzyjmij.Click += buttonPrzyjmij_Click;
            view.Tag = new ViewHolder() { txtlogin = txtlogin, odrzuc = buttonOdrzuc, przyjmij = buttonPrzyjmij};

            return view;
        }
        private async void buttonOdrzuc_Click(object sender, System.EventArgs e)
        {
            var zmienna = new ConfirmInvitationDto()
            {
                name = txtlogin.text,
            };
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/account/register");
                var json = JsonConvert.SerializeObject(zmienna);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, data);

                string result = response.Content.ReadAsStringAsync().Result;
                //StartActivity(new Intent(this, typeof(LoginActivity)));
            }


        }
        private async void buttonPrzyjmij_Click(object sender, System.EventArgs e)
        {
            var zmienna = new ConfirmInvitationDto()
            {

            };
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://inz-api-app.azurewebsites.net/api/account/register");
                var json = JsonConvert.SerializeObject(newUser);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, data);

                string result = response.Content.ReadAsStringAsync().Result;
               // StartActivity(new Intent(this, typeof(LoginActivity)));
            }


        }
    }
}