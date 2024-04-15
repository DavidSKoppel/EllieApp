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

        public static void StartMyService()
        {
            if (MainActivity == null) return;
            //MainActivity.StartNetworkService();
            MainActivity.StartMessageService();
        }

        public static void StopMyService()
        {
            if (MainActivity == null) return;
            //MainActivity.StopNetworkService();
            MainActivity.StopMessageService();
            IsRunning = false;
        }
    }
}
