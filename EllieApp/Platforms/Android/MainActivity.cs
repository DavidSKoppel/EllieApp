using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using EllieApp.Platforms.Android;

namespace EllieApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public MainActivity()
        {
            AndroidServiceManager.MainActivity = this;
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            ProcessIntent(intent);
        }

        private void ProcessIntent(Intent? intent)
        {
            if(intent != null)
            {
                var action = intent.Action;
                if(action == "USER_TAPPED_NOTIFICATION")
                {

                }
            }
        }
        public void StartNetworkService()
        {
            var serviceIntent = new Intent(this, typeof(NetworkForegroundService));
            serviceIntent.PutExtra("inputExtra", "ForegroundService");
            StartService(serviceIntent);
        }

        public void StopNetworkService()
        {
            var serviceIntent = new Intent(this, typeof(NetworkForegroundService));
            StopService(serviceIntent);
        }

        public void StartMessageService()
        {
            var serviceIntent = new Intent(this, typeof(MessageForegroundService));
            serviceIntent.PutExtra("inputExtra", "ForegroundService");
            StartService(serviceIntent);
        }

        public void StopMessageService()
        {
            var serviceIntent = new Intent(this, typeof(MessageForegroundService));
            StopService(serviceIntent);
        }
    }
}
