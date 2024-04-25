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
        User user = new User();
        public static List<Alarm> globalAlarms = new List<Alarm>();
        public MainActivity()
        {
            /*bool isLoggedIn = Preferences.Get("isLoggedIn", defaultValue: false);
            if(isLoggedIn)
            {
                user = new User()
                {
                    Id = Convert.ToInt32(Preferences.Get("id", defaultValue: null)),
                    FirstName = Preferences.Get("firstName", defaultValue: null),
                    LastName = Preferences.Get("lastName", defaultValue: null),
                    Points = Convert.ToInt32(Preferences.Get("points", defaultValue: null)),
                    Token = Preferences.Get("token", defaultValue: null)
                };
            }*/
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
                    double userPoints = Convert.ToDouble(Preferences.Get("points", defaultValue: "1337"));
                    var content = new StringContent("{\r\n    \"points\": "+ userPoints + 20 +"\r\n}", null, "application/json");
                    HttpResponseMessage response =
                    await httpClient.PutAsync("https://totally-helpful-krill.ngrok-free.app/User?id=7", content);
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
