using System;

namespace Pumkin.UnityTools.Interfaces.Settings
{
    interface ISetting
    {
        string Name { get; set; }
        object Value { get; set; }        
        string NameInUI { get; set; }
        string Description { get; set; }
    }
}