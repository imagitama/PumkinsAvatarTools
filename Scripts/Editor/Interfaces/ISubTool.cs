#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface ISubTool : ISubItem
    {
        /// <summary>
        /// Whether or not to allow Update() to run
        /// </summary>
        bool AllowUpdate { get; set; }

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