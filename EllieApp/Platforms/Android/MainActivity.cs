using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using EllieApp.Models;
using EllieApp.Platforms.Android;
using System.Net.Http.Json;
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
            //serviceIntent.PutExtra("AlarmId", id);
            StopService(serviceIntent);
        }
    }
}
