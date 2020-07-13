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
        /// The ID of the parent module this tool should be put into.
        /// </summary>
        string ParentModuleID { get; set; }

        /// <summary>
        /// Name of the game configuration to look for. Must match a value in AvatarTools.GameConfiguration enum.
        /// For example: "VRChat" will only work in vrchat, "All" or "Generic" will work everywhere
        /// </summary>
        string GameConfigurationString { get; set; }

        /// <summary>
        /// Whether or not to allow Update() to run
        /// </summary>
        bool AllowUpdate { get; set; }

        /// <summary>
        /// The order of which this will tool will be drawn in the UI
        /// </summary>
        int OrderInUI { get; set; }

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
