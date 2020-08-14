#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.UnityTools.Interfaces
{
    public interface ISubTool
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
        /// Should call Prepare() => Execute() => Finish()
        /// </summary>
        /// <param name="target">Target GameObject</param>
        /// <returns>True if everything succeeded</returns>
        bool TryExecute(GameObject target);
        
        /// <summary>
        /// Runs every editor update, used when tool still needs to do stuff when it's UI isn't being drawing
        /// </summary>
        void Update();
    }
}
#endif