using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InfinityPlanets
{
    class Logger
    {
        static string Log = "Placeholder path";
        public static void Setup(string Path)
        {
            Log = Path + "\\Infinity\\Logs\\INFINITY-PLNTS.log";
            File.WriteAllText(Log, "-----Log initiated for INFINITY-PLANETS.-----\nExecutable at: " + Environment.CurrentDirectory + "Infinity-Planets.exe. Run by " + Environment.UserName + ".\nGameData path: " + Path + "; Local time: " + DateTime.Now + "; UTC: " + DateTime.UtcNow + ".\n---------------------------------------------");
        }
        public static void Lawg(string Message)
        {
            File.AppendAllText(Log, "\n" + Message);
        }
        /// <summary>
        /// Returns a string to be logged
        /// </summary>
        /// <param name="Message"- The message to be logged></param>
        /// <returns></returns>
        public static string LogMessage(string Message)
        {
            string LogMsg = ("[LOG_" + DateTime.UtcNow + "_UTC]: " + Message);
            return LogMsg;
        }
    }
}