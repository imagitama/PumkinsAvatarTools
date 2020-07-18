using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.UnityTools
{
    static class PumkinTools
    {
        public delegate void AvatarChangeHandler(GameObject selection);        
        public static event AvatarChangeHandler AvatarSelectionChanged;        

        static GameObject _selectedAvatar;

        public static GameObject SelectedAvatar 
        {
            get => _selectedAvatar;
            set
            {
                if(_selectedAvatar != value)
                    OnAvatarSelectionChanged(_selectedAvatar);
                _selectedAvatar = value;
            }
        }

        public static void OnAvatarSelectionChanged(GameObject newSelection)
        {
            AvatarSelectionChanged?.Invoke(newSelection);
        }

        public static ILogHandler LogHandler { get; set; } = new Implementation.Logging.LogHandler();

        public static void Log(string msg, UnityEngine.Object context)
        {
            LogHandler.LogFormat(LogType.Log, context, msg, new string[0]);
        }
    }
}

