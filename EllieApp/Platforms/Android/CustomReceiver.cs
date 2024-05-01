using Android;
using Android.App;
using Android.Content;
using Android.Widget;
using AndroidX.Core.Content;

namespace EllieApp.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true, Permission = Manifest.Permission.ReceiveBootCompleted)]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.LowPriority, Categories = new[] { Intent.CategoryDefault })]
    public class CustomReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent.Action == "AlarmReceived")
            {
                Toast.MakeText(context, "Alarm Firing", ToastLength.Short).Show();
                var alarm = intent.GetStringExtra("Alarm");
                AndroidServiceManager.StartMyMessageService(alarm);
                /*var serviceIntent = new Intent(context,
                typeof(MessageForegroundService));

                ContextCompat.StartForegroundService(context,
                    serviceIntent);*/
            }else if (intent.Action == "UPDATE_ALARMS")
            {
                AndroidServiceManager.StartMyNetworkService();
            }
            else if (intent.Action == Intent.ActionBootCompleted)
            {
                Toast.MakeText(context, "Boot completed, getting Alarms", ToastLength.Short).Show();
                AndroidServiceManager.StartMyNetworkService();
                /*
                var serviceIntent = new Intent(context,
                typeof(MessageForegroundService));

                ContextCompat.StartForegroundService(context,
                    serviceIntent);*/
            } 
        }
    }
}
