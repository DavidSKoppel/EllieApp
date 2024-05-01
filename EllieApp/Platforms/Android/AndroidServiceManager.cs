using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllieApp.Platforms.Android
{
    public static class AndroidServiceManager
    {
        public static MainActivity? MainActivity { get; set; }

        public static bool IsRunning { get; set; }

        public static void StartMyNetworkService()
        {
            if (MainActivity == null) return;
            MainActivity.StartNetworkService();
        }

        public static void StopMyNetworkService()
        {
            if (MainActivity == null) return;
            MainActivity.StopNetworkService();
            IsRunning = false;
        }

        public static void StartMyMessageService(string alarm)
        {
            if (MainActivity == null) return;
            MainActivity.StartMessageService(alarm);
        }

        public static void StopMyMessageService()
        {
            if (MainActivity == null) return;
            MainActivity.StopMessageService();
            IsRunning = false;
        }
    }
}
