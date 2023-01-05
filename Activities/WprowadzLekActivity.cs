using Android;
using Android.App;
using Android.Content;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using WezLekApp.Models;

namespace WezLekApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class WprowadzLekActivity : AppCompatActivity
    {
        ImageView imageview;
        Button btnProcess;
        Button btnWezZdjecie;
        Button dalej;
        TextView txtView;
        Android.Graphics.Bitmap bitmap;
        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WprowadzLek);
            //Create your application here
            imageview = FindViewById<ImageView>(Resource.Id.image_view);
            btnProcess = FindViewById<Button>(Resource.Id.btnProcess);
            btnWezZdjecie = FindViewById<Button>(Resource.Id.wezZdjecie);
            dalej = FindViewById<Button>(Resource.Id.Dalej);
            txtView = FindViewById<TextView>(Resource.Id.txtView);
            //Bitmap bitmap = BitmapFactory.DecodeResource(ApplicationContext.Resources, Resource.Drawable.lek);
            //imageview.SetImageBitmap(bitmap);
            btnWezZdjecie.Click += wezZdjecie_Click; 
            dalej.Click += dalej_Click;
            btnProcess.Click += btnProcess_Click;
            

            RequestPermissions(permissionGroup, 0);
        }
        private void dalej_Click(object sender, System.EventArgs e)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();
            edit.PutString("Lek", txtView.Text);

            edit.Apply();

            StartActivity(new Intent(this, typeof(WprowadzLekv2Activity)));

        }
        private void wezZdjecie_Click(object sender, System.EventArgs e)
        {
            SelectPhoto();

        }
        private void btnProcess_Click(object sender, System.EventArgs e)
        {
            TextRecognizer txtRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
            if (!txtRecognizer.IsOperational)
            {
                Log.Error("Error", "Detector dependencies are not yet available");
            }
            else
            {
                Frame frame = new Frame.Builder().SetBitmap(bitmap).Build();
                SparseArray items = txtRecognizer.Detect(frame);
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < items.Size(); i++)
                {
                    TextBlock item = (TextBlock)items.ValueAt(i);
                    strBuilder.Append(item.Value + " ");
                }
                txtView.Text = strBuilder.ToString();
            }

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        async void SelectPhoto()
        {
            await CrossMedia.Current.Initialize();
            if(!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "pobieranie zdjec niedozwolone", ToastLength.Short).Show();
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 30,
                
            });
            if(file == null)
            {
                return;
            }
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            bitmap = BitmapFactory.DecodeByteArray(imageArray,0,imageArray.Length);
            imageview.SetImageBitmap(bitmap);
        }
    }
}