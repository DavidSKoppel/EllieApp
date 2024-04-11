using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EllieApp.Platforms.Android.MessageForegroundService;

namespace EllieApp.Platforms.Android
{
    [Service]
    public class NetworkForegroundService : Service
    {
        Timer timer = null;
        int myId = (new object()).GetHashCode();
        int BadgeNumber = 0;
        private readonly IBinder binder = new LocalBinder();
        public class LocalBinder : Binder
        {
            public MessageForegroundService GetService()
            {
                return this.GetService();
            }
        }
        public override IBinder? OnBind(Intent? intent)
        {
            throw new NotImplementedException();
        }
        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            var input = intent.GetStringExtra("inputExtra");

            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetAction("USER_TAPPED_NOTIFIACTION");

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent,
                PendingIntentFlags.Immutable);

            var notification = new NotificationCompat.Builder(this,
                    MainApplication.ChannelId)
                .SetContentText(input)
                .SetSmallIcon(Resource.Drawable.splash)
                .SetContentIntent(pendingIntent);

            StartForeground(myId, notification.Build(), ForegroundService.TypeDataSync);

            // You can stop the service from inside the service by calling StopSelf();

            timer = new Timer(Timer_Elapsed, notification, 0, 60000);

            return StartCommandResult.Sticky;
        }

        private async void Timer_Elapsed(object? state)
        {
            AndroidServiceManager.IsRunning = true;

            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync("https://dog.ceo/api/breeds/image/random");

            BadgeNumber++;
            string data = $"Data: {response}";
            var notification = (NotificationCompat.Builder)state;
            notification.SetNumber(BadgeNumber);
            notification.SetContentTitle(data);
            notification.SetContentText(data);
            StartForeground(myId, notification.Build());
        }
    }
}