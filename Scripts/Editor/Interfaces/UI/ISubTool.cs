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
        /// <summary>
        /// The name of the tool. Used in the UI.
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// The name of the tool. Used in the UI.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The category name of the tool. Used to group tools in the UI.
        /// </summary>
        string CategoryName { get; set; }

        /// <summary>
        /// Name of the game configuration to look for. Must match a value in AvatarTools.GameConfiguration enum.
        /// For example: "VRChat" will only work in vrchat, "All" or "Generic" will work everywhere
        /// </summary>
        string GameConfigurationString { get; set; }

        bool AllowUpdate { get; set; }

        /// <summary>
        /// Draws the UI
        /// </summary>
        void DrawUI();

        /// <summary>
        /// Executes the tool functionality
        /// </summary>
        /// <param name="target">Target GameObject</param>
        /// <returns>True if succeeded</returns>
        bool Execute(GameObject target);

        /// <summary>
        /// Runs before Execute
        /// </summary>
        /// <param name="target">Target GameObject</param>
        /// <returns>True if succeded, used to check if we should call Execute()</returns>
        bool Prepare(GameObject target);
                
        //void Finalize(GameObject target);
        
        /// <summary>
        /// Runs every editor update, used when tool still needs to do stuff when it's UI isn't being drawing
        /// </summary>
        void Update();
    }
}
