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

        public static void StartMyMessageService(int id)
        {
            if (MainActivity == null) return;
            MainActivity.StartMessageService(id);
        }

        public static void StopMyMessageService(int id)
        {
            if (MainActivity == null) return;
            MainActivity.StopMessageService(id);
            IsRunning = false;
        }
    }
}
