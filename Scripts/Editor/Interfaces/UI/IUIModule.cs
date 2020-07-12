using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Interfaces
{
    interface IUIModule
    {
        string Name { get; set; }
        string Description { get; set; }

        List<ISubTool> SubTools { get; set; }
        bool IsExpanded { get; set; }        
        string GameConfigurationString { get; set; }
        void Draw();
    }
}