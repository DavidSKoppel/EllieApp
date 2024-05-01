using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using EllieApp.Models;
using EllieApp.Platforms.Android;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace EllieApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static List<Alarm> globalAlarms = new List<Alarm>();
        public MainActivity()
        {
            AndroidServiceManager.MainActivity = this;
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            ProcessIntent(intent);
        }

        private async void ProcessIntent(Intent? intent)
        {
            if(intent != null)
            {
                var action = intent.Action;
                if(action == "USER_TAPPED_NOTIFICATION")
                {
                    StopMessageService();
                    HttpClient httpClient = new HttpClient();
                    double userPoints = Convert.ToDouble(Preferences.Get("points", defaultValue: "1337"));
                    userPoints += 20;
                    var content = new StringContent("{\r\n    \"points\": "+ userPoints +"\r\n}", null, "application/json");
                    HttpResponseMessage response =
                    await httpClient.PutAsync("https://totally-helpful-krill.ngrok-free.app/User?id=7", content);
                    Toast.MakeText(this, "Alarm Stopped", ToastLength.Short).Show();
                }
            }
        }
        public void StartNetworkService()
        {
            var serviceIntent = new Intent(this, typeof(NetworkForegroundService));
            //serviceIntent.PutExtra("inputExtra", "ForegroundService");
            StartService(serviceIntent);
        }

        public void StopNetworkService()
        {
            var serviceIntent = new Intent(this, typeof(NetworkForegroundService));
            StopService(serviceIntent);
        }

        internal void StartMessageService(string alarm)
        {
            var serviceIntent = new Intent(this, typeof(MessageForegroundService));
            serviceIntent.PutExtra("Alarm", alarm);
            StartService(serviceIntent);
        }

        internal void StopMessageService()
        {
            var serviceIntent = new Intent(this, typeof(MessageForegroundService));
            StopService(serviceIntent);
        }

        public void StartAlarms(List<Alarm> alarms)
        {
            string activeAlarms = "0";
            foreach (var alarm in alarms)
            {
                var jsonAlarm = JsonSerializer.Serialize(alarm);
                var intent = new Intent(this, typeof(CustomReceiver));
                intent.SetAction("AlarmReceived");
                intent.PutExtra("Alarm", jsonAlarm);
                var pendingIntent = PendingIntent.GetBroadcast(this, alarm.id, intent, PendingIntentFlags.Mutable);
                var alarmManager = (AlarmManager)this.GetSystemService(Context.AlarmService);

                Java.Util.Calendar calendar = new Java.Util.Calendar.Builder().SetCalendarType("iso8601").Build();
                calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();

                // Set the alarm to trigger at 12:30 PM
                calendar.Set(Java.Util.CalendarField.HourOfDay, alarm.activatingTime.Hour);
                calendar.Set(Java.Util.CalendarField.Minute, alarm.activatingTime.Minute);
                calendar.Set(Java.Util.CalendarField.Second, alarm.activatingTime.Second);
                //long interval = 10000;
                if (calendar.TimeInMillis < Java.Lang.JavaSystem.CurrentTimeMillis())
                {
                    // If the time has passed, add one day to the calendar to schedule the alarm for tomorrow
                    calendar.Add(Java.Util.CalendarField.DayOfMonth, 1);
                }

                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);
                //alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + interval, pendingIntent);
                activeAlarms += "-" + alarm.id.ToString();
            }

            Preferences.Set("activeAlarms", activeAlarms);
        }

        public void StopAlarms()
        {
            int id = 0;
            var intent = new Intent(this, typeof(CustomReceiver));
            intent.SetAction("AlarmReceived");

            string alarmIds = Preferences.Get("activeAlarms", "0-0");
            string[] numberStrings = alarmIds.Split('-');

            // Convert each substring to an integer and add it to a list
            List<int> numbers = new List<int>();
            foreach (string numberStr in numberStrings)
            {
                numbers.Add(int.Parse(numberStr));
            }

            AlarmManager alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);

            foreach (int number in numbers)
            {
                var pendingIntent = PendingIntent.GetBroadcast(this, number, intent, PendingIntentFlags.Mutable | PendingIntentFlags.UpdateCurrent);
                alarmManager.Cancel(pendingIntent);
            }
            
            var internetIntent = new Intent(this, typeof(MainActivity));
            intent.SetAction("UPDATE_ALARMS");
            var pendingInternetIntent = PendingIntent.GetBroadcast(this, 180811, internetIntent, PendingIntentFlags.Mutable | PendingIntentFlags.UpdateCurrent);

            alarmManager.Cancel(pendingInternetIntent);
        }
    }
}
