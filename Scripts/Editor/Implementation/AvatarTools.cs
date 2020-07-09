using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools
{
    static class AvatarTools
    {
        public static GameObject SelectedAvatar { get; set; }
        public static ILogHandler LogHandler { get; set; } = new Implementation.Logging.LogHandler();

        public static void Log(string msg, UnityEngine.Object context)
        {
            LogHandler.LogFormat(LogType.Log, context, msg, new string[0]);
        }
    }
}

