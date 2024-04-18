using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
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
                    StopMessageService(1);
                    HttpClient httpClient = new HttpClient();
                    var content = new StringContent("{\r\n    \"points\": 300\r\n}", null, "application/json");
                    HttpResponseMessage response =
                    await httpClient.PutAsync("https://deep-wealthy-roughy.ngrok-free.app/User?id=7", content);
                    var json = await response.Content.ReadAsStringAsync();
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

        internal void StartMessageService(int id)
        {
            var serviceIntent = new Intent(this, typeof(MessageForegroundService));
            serviceIntent.PutExtra("AlarmId", id);
            StartService(serviceIntent);
        }

        internal void StopMessageService(int id)
        {
            var serviceIntent = new Intent(this, typeof(MessageForegroundService));
            serviceIntent.PutExtra("AlarmId", id);
            StopService(serviceIntent);
        }
    }
}
