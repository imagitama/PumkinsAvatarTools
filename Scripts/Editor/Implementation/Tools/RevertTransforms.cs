using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Implementation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_reverTransforms", "tools")]
    class RevertTransforms : SubToolBase
    {
        public override SettingsContainer Settings => null;

        public RevertTransforms()
        {
            Name = "Revert Transforms";
            Description = "Reverts the location, rotation and scale of your avatar back to prefab";
        }

        protected override bool Prepare(GameObject target)
        {
            if(!base.Prepare(target))
                return false;
            var prefType = PrefabUtility.GetPrefabAssetType(target);
            if(prefType == PrefabAssetType.MissingAsset || prefType == PrefabAssetType.NotAPrefab)
            {
                Debug.Log("Not a prefab");
                return false;
            }
            return true;
        }

        protected override bool DoAction(GameObject target)
        {
            Type transType = typeof(Transform);
            var overrides = PrefabUtility.GetObjectOverrides(target)
                ?.Where(t => t.instanceObject.GetType() == transType && t.instanceObject);

            foreach(var o in overrides)
                o.Revert();

            return true;
        }
    }
}
