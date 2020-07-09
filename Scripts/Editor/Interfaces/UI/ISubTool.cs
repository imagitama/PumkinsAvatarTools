using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    interface ISubTool
    {
        string Name { get; set; }
        string Description { get; set; }
        string CategoryName { get; set; }

        void DrawUI();
        bool Execute(GameObject avatar);
    }
}
