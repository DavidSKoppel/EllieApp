using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;
using AndroidX.Core.App;

namespace EllieApp.Platforms.Android
{
    [Service]
    public class MessageForegroundService : Service
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
        public override IBinder OnBind(Intent? intent)
        {
            return binder;
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

            StartForeground(myId, notification.Build(), ForegroundService.TypeSpecialUse);

            // You can stop the service from inside the service by calling StopSelf();


            timer = new Timer(Timer_Elapsed, notification, 0, 60000);

            return StartCommandResult.Sticky;
        }

        private void Timer_Elapsed(object? state)
        {
            AndroidServiceManager.IsRunning = true;

            BadgeNumber++;
            string timeString = $"Time: {DateTime.Now.ToLongTimeString()}";
            var notification = (NotificationCompat.Builder)state;
            notification.SetNumber(BadgeNumber);
            notification.SetContentTitle(timeString);
            notification.SetContentText(timeString);
            StartForeground(myId, notification.Build());
        }
    }
}
