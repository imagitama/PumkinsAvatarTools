using Pumkin.UnityTools.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.UnityTools.Implementation.Settings
{
    class Setting : ISetting
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string NameInUI 
        { 
            get => string.IsNullOrEmpty(_nameInUI) ? Name : _nameInUI; 
            set => _nameInUI = value; 
        }
        public string Description { get; set; }        

        private string _nameInUI;

        public Setting(string name, object value, string nameInUI = null, string description = null)
        {
            Name = name;
            Value = value;
            NameInUI = nameInUI;
            Description = description;
        }

        public static implicit operator bool(Setting setting)
        {
            return !ReferenceEquals(setting, null);
        }
    }
}
