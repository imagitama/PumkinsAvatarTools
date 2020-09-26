#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.TerrainAPI;

namespace Pumkin.AvatarTools.Implementation.Settings
{
    internal class DynamicBoneCopier_Settings : CopierSettingsContainerBase
    {
        [DrawToggleLeft]
        public bool removeOldBones = false;
        [DrawToggleLeft]
        public bool createGameObjects = false;
    }
}
#endif