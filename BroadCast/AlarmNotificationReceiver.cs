using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WezLekApp.BroadCast
{
    [BroadcastReceiver(Enabled = true)]
    public class AlarmNotificationReceiver:BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

            builder.SetAutoCancel(true)
                .SetDefaults((int)NotificationDefaults.All)
                .SetSmallIcon(Resource.Drawable.logo64x64)
                .SetContentTitle("WezLekApp")
                .SetContentText("Wez lek: ")
                .SetContentInfo("info");

            NotificationManager manager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            manager.Notify(1, builder.Build());

        }
    }
}