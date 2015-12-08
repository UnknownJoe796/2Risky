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
        /// <summary>
        /// Contains all of the log events, sorted from newest to oldest
        /// </summary>
        public static List<string> Events = new List<string>();

        /// <summary>
        /// timestamps an event and adds it to the event log
        /// </summary>
        /// <param name="description"></param>
        public static void AddEvent(string description)
        {
            System.Diagnostics.Debug.WriteLine(description);
            var now = DateTime.Now.ToString("h:mm:ss");
            Events.Insert(0,now + ": " + description);
        }
    }
}
