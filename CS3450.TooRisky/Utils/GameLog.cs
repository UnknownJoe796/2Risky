using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.OnlineId;

namespace CS3450.TooRisky.Utils
{
    public static class GameLog
    {
        public static List<string> Events = new List<string>();

        public static void AddEvent(string description)
        {
            System.Diagnostics.Debug.WriteLine(description);
            var now = DateTime.Now.ToString("h:mm:ss");
            Events.Add(now + ": " + description);
        }
    }
}
