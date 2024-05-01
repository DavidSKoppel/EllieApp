using EllieApp.Models;
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

        public static void StartMyAlarms(List<Alarm> list)
        {
            if (MainActivity == null) return;
            MainActivity.StartAlarms(list);
        }

        public static void StopMyAlarms()
        {
            if (MainActivity == null) return;
            MainActivity.StopAlarms();
        }

        public static void StartMyNetworkService()
        {
            if (MainActivity == null) return;
            MainActivity.StartNetworkService();
        }

        public static void StopMyNetworkService()
        {
            if (MainActivity == null) return;
            MainActivity.StopNetworkService();
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
        }
    }
}
