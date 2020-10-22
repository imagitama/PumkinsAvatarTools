#if UNITY_EDITOR
using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface ITool : IItem
    {
        /// <summary>
        /// Whether or not to allow Update() to run
        /// </summary>
        bool CanUpdate { get; set; }

        /// <summary>
        /// Should call Prepare() => DoAction() => Finish()
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