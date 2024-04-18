using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;
using AndroidX.Core.App;
using EllieApp.Models;

namespace EllieApp.Platforms.Android
{
    [Service]
    public class MessageForegroundService : Service
    {
        Timer timer = null;
        int myId = (new object()).GetHashCode();
        int BadgeNumber = 0;
        private readonly IBinder binder = new LocalBinder();
        private Alarm thisAlarm;

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

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            var idInput = intent.GetIntExtra("AlarmId", -1);
            thisAlarm =  MainActivity.globalAlarms.FirstOrDefault(a => a.id == idInput);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
#pragma warning disable CA1416
                var serviceChannel =
                    new NotificationChannel(MainApplication.ChannelId + idInput,
                        "Background Service Channel",
                    NotificationImportance.High);

                if (GetSystemService(NotificationService)
                    is NotificationManager manager)
                {
                    manager.CreateNotificationChannel(serviceChannel);
                }
#pragma warning restore CA1416
            }

            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.PutExtra("Id", idInput);
            notificationIntent.SetAction("USER_TAPPED_NOTIFICATION");

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent,
                PendingIntentFlags.Immutable);

            var notification = new NotificationCompat.Builder(this,
                    MainApplication.ChannelId + idInput)
                .SetContentText("Message")
                .SetSmallIcon(Resource.Drawable.splash)
                .SetContentIntent(pendingIntent);

            StartForeground(idInput, notification.Build(), ForegroundService.TypeSpecialUse);

            // You can stop the service from inside the service by calling StopSelf();

            timer = new Timer(Timer_Elapsed, notification, 0, 60000);

            return StartCommandResult.Sticky;
        }

        private void Timer_Elapsed(object? state)
        {
            AndroidServiceManager.IsRunning = true;

            BadgeNumber++;
            var notification = (NotificationCompat.Builder)state;
            notification.SetNumber(BadgeNumber);
            notification.SetContentTitle(thisAlarm.name);
            notification.SetContentText(thisAlarm.description);
            StartForeground(myId, notification.Build());
        }
    }
}
