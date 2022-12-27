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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WezLekApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class WprowadzLekActivity : Activity
    {
        private ImageView imageview;
        private Button btnProcess;
        private TextView txtView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WprowadzLek);
            //Create your application here
            imageview = FindViewById<ImageView>(Resource.Id.image_view);
            btnProcess = FindViewById<Button>(Resource.Id.btnProcess);
            txtView = FindViewById<TextView>(Resource.Id.txtView);
            Bitmap bitmap = BitmapFactory.DecodeResource(ApplicationContext.Resources, Resource.Drawable.lek);
            imageview.SetImageBitmap(bitmap);
            btnProcess.Click += delegate
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
                        strBuilder.Append(item.Value);
                        strBuilder.Append("/");
                    }
                    txtView.Text = strBuilder.ToString();
                }
            };
        }
    }
}