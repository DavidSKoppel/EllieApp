using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using EllieApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static EllieApp.Platforms.Android.MessageForegroundService;

namespace EllieApp.Platforms.Android
{
    [Service]
    public class NetworkForegroundService : Service
    {
        int myId = (new object()).GetHashCode();
        private readonly IBinder binder = new LocalBinder();
        private HttpClient? httpClient;
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
            var checkIntent = new Intent(this, typeof(MainActivity));
            checkIntent.SetAction("UPDATE_ALARMS");
            var checkIfIntent = PendingIntent.GetBroadcast(this, 180811, checkIntent, PendingIntentFlags.Immutable | PendingIntentFlags.NoCreate);
            bool isAlarmActive = (checkIfIntent != null);

            if (!isAlarmActive)
            {
                RefreshAlarm();
            }
            else
            {
                StopSelf();
            }

            return StartCommandResult.Sticky;
        }

        private async void RefreshAlarm()
        {
            try
            {
                httpClient = new HttpClient();
                HttpResponseMessage response =
            await httpClient.GetAsync("https://totally-helpful-krill.ngrok-free.app/UserAlarmRelation/GetAlarmsByUserId/id?id=" + Convert.ToInt32(Preferences.Get("id", defaultValue: 1)));
                var json = await response.Content.ReadAsStringAsync();
                var jsonAlarms = JsonSerializer.Deserialize<List<Alarm>>(json);
                response.EnsureSuccessStatusCode();
                AndroidServiceManager.StartMyAlarms(jsonAlarms);

                var intent = new Intent(this, typeof(CustomReceiver));
                intent.SetAction("UPDATE_ALARMS");
                var pendingIntent = PendingIntent.GetBroadcast(this, 180811, intent, PendingIntentFlags.Mutable);
                var alarmManager = (AlarmManager)this.GetSystemService(Context.AlarmService);

                long interval = 43200000; //12 Hours
                //long interval = 20000;
                alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + interval, pendingIntent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                //alarmManager.Cancel(pendingIntent);
                httpClient?.Dispose();
                StopSelf();
            }
        }
    }
}